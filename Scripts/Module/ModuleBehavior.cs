using System.Collections.Generic;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class ModuleBehavior<T> : Singleton<ModuleBehavior<T>> where T : struct
    {
        private static readonly Dictionary<ulong, Condition<T>> conditionDict = new Dictionary<ulong, Condition<T>>();

        private static readonly Dictionary<ulong, Executor<T>> executorDict =
            new Dictionary<ulong, Executor<T>>();

        /// <summary>
        /// 注册条件节点。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="condition"></param>
        public static void LoginCondition(Node entity, Condition<T> condition)
        {
            var key = entity.GetInstanceId();
            if (conditionDict.TryGetValue(key, out var conditions))
            {
                conditions += condition;
                conditionDict[key] = conditions;
            }
            else
            {
                conditionDict.Add(key, condition);
            }
        }


        public static void LogoutCondition(Node entity, Condition<T> condition)
        {
            var key = entity.GetInstanceId();
            if (conditionDict.TryGetValue(key, out var conditions))
            {
                conditions -= condition;
                conditionDict[key] = conditions;
            }
        }

        /// <summary>
        /// 注册执行节点。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="executor"></param>
        public static void LoginExecutor(Node entity, Executor<T> executor)
        {
            var key = entity.GetInstanceId();
            if (executorDict.TryGetValue(key, out var executors))
            {
                executors += executor;
                executorDict[key] = executors;
            }
            else
            {
                executorDict.Add(key, executor);
            }
        }
        
        
        public static void LogoutExecutor(Node entity, Executor<T> executor)
        {
            var key = entity.GetInstanceId();
            if (executorDict.TryGetValue(key, out var executors))
            {
                executors -= executor;
                executorDict[key] = executors;
            }
        }


        public static bool Behave(Node entity, T behavior)
        {
            var key = entity.GetInstanceId();
            if (conditionDict.TryGetValue(key, out var conditions))
            {
                foreach (var condition in conditions.GetInvocationList())
                {
                    var result = (bool) condition.DynamicInvoke(entity, behavior);
                    if (!result)
                    {
                        return false;
                    }
                }
            }

            if (executorDict.TryGetValue(key, out var executors))
            {
                executors(entity, behavior);
            }

            return true;
        }
    }
}