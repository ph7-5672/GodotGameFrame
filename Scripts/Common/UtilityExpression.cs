using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Godot;
using Expression = System.Linq.Expressions.Expression;

namespace Frame.Common
{
    public static class UtilityExpression
    {
        //缓存表达式树
        private static Dictionary<string, object> expCache = new Dictionary<string, object>();

        private delegate object ReturnValueDelegate(object instance, object[] arguments);
        private delegate void VoidDelegate(object instance, object[] arguments);
        

        #region Assign

        /// <summary>
        /// 表达式设置对象字段或属性的值。
        /// </summary>
        /// <param name="obj">指定对象</param>
        /// <param name="name">字段或属性名</param>
        /// <param name="val">指定值</param>
        /// <typeparam name="TO"></typeparam>
        /// <typeparam name="TV"></typeparam>
        public static void AssignValue<TO, TV>(this TO obj, string name, TV val)
        {
            var key = nameof(AssignValue) + obj + name;
            if (!expCache.TryGetValue(key, out var func))
            {
                var objExp = Expression.Parameter(typeof(TO), "obj");
                var left = AssignValueLeft<TO>(name, objExp);
                var valExp = Expression.Parameter(typeof(TV), "val");
                var assign = Expression.Assign(left, valExp);
                var lambda = Expression.Lambda<Func<TO, TV, TV>>(assign, objExp, valExp);
                func = lambda.Compile();
                expCache.Add(key, func);
            }
            
            var invoke = (Func<TO, TV, TV>) func;
            invoke(obj, val);
        }

        /// <summary>
        /// 表达式加算对象字段或属性的值。
        /// </summary>
        /// <param name="obj">指定对象</param>
        /// <param name="name">字段或属性名</param>
        /// <param name="val">指定值</param>
        /// <typeparam name="TO"></typeparam>
        /// <typeparam name="TV"></typeparam>
        public static void AssignAddValue<TO, TV>(this TO obj, string name, TV val)
        {
            var key = nameof(AssignAddValue) + obj + name;
            if (!expCache.TryGetValue(key, out var func))
            {
                var objExp = Expression.Parameter(typeof(TO), "obj");
                var left = AssignValueLeft<TO>(name, objExp);
                var valExp = Expression.Parameter(typeof(TV), "val");
                var assign = Expression.AddAssign(left, valExp);
                var lambda = Expression.Lambda<Func<TO, TV, TV>>(assign, objExp, valExp);
                func = lambda.Compile();
                expCache.Add(key, func);
            }
            var invoke = (Func<TO, TV, TV>) func;
            invoke(obj, val);
            
        }


        private static MemberExpression AssignValueLeft<T>(string name, ParameterExpression objExp)
        {
            var objType = typeof(T);

            MemberExpression memberExp = null;
                
            var split = name.Split('.');
            foreach (var s in split)
            {
                var field = objType.GetField(s);
                if (field != null)
                {
                    if (memberExp == null)
                    {
                        memberExp = Expression.Field(objExp, field);
                    }
                    else
                    {
                        memberExp = Expression.Field(memberExp, field);
                    }
                        
                    objType = field.FieldType;
                }
                else
                {
                    var property = objType.GetProperty(s);
                    if (property != null)
                    {
                        if (memberExp == null)
                        {
                            memberExp = Expression.Property(objExp, property);
                        }
                        else
                        {
                            memberExp = Expression.Property(memberExp, property);
                        }
                        objType = property.PropertyType;
                    }
                }
                    
                if (memberExp == null)
                {
                    throw new ArgumentException("找不到指定的属性或字段捏");
                }
            }

            return memberExp;
        }

        #endregion


        #region Method
        public static object Call(this object instance, MethodInfo methodInfo, params object[] args)
        {
            ReturnValueDelegate returnValueDelegate;
            var instanceExpression = Expression.Parameter(typeof(object),"instance");
            var argumentsExpression = Expression.Parameter(typeof(object[]),"arguments");
            var argumentExpressions = new List<Expression>();
            var parameterInfos = methodInfo.GetParameters();
            for (var i = 0; i < parameterInfos.Length; ++i)
            {
                var parameterInfo = parameterInfos[i];
                argumentExpressions.Add(Expression.Convert(Expression.ArrayIndex(argumentsExpression, Expression.Constant(i)), parameterInfo.ParameterType));
            }
            var callExpression = Expression.Call(!methodInfo.IsStatic ? Expression.Convert(instanceExpression, methodInfo.ReflectedType) : null, methodInfo, argumentExpressions);
            if (callExpression.Type == typeof(void))
            {
                var voidDelegate = Expression.Lambda<VoidDelegate>(callExpression, instanceExpression, argumentsExpression).Compile();
                returnValueDelegate = (i, a) => { voidDelegate(i, a); return null; };
            }
            else
                returnValueDelegate = Expression.Lambda<ReturnValueDelegate>(Expression.Convert(callExpression, typeof(object)), instanceExpression, argumentsExpression).Compile();

            return returnValueDelegate(instance, args);
        }


        #endregion

        
        

    }
}