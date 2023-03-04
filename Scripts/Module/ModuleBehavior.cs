using System;
using System.Collections.Generic;
using System.Linq;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class ModuleBehavior : Singleton<ModuleBehavior>
    {
        private readonly Dictionary<ulong, Delegate[]> conditionDict = new Dictionary<ulong, Delegate[]>();

        private readonly Dictionary<ulong, Delegate[]> executorDict = new Dictionary<ulong, Delegate[]>();

        /// <summary>
        /// 注册条件节点。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="condition"></param>
        public static void LoginCondition<T>(Node entity, Condition<T> condition) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            if (!Instance.conditionDict.TryGetValue(key, out var delegates))
            {
                delegates = new Delegate[Constants.behaviorTypeArray.Length];
                delegates[index] = condition;
                Instance.conditionDict.Add(key, delegates);
            }
            else if (delegates[index] is Condition<T> conditions)
            {
                conditions += condition;
                delegates[index] = conditions;
                Instance.conditionDict[key] = delegates;
            }
            else
            {
                delegates[index] = condition;
            }
        }


        public static void LogoutCondition<T>(Node entity, Condition<T> condition) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            if (Instance.conditionDict.TryGetValue(key, out var delegates) &&
                delegates[index] is Condition<T> conditions)
            {
                conditions -= condition;
                delegates[index] = conditions;
                Instance.conditionDict[key] = delegates;
            }
        }

        /// <summary>
        /// 注册执行节点。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="executor"></param>
        public static void LoginExecutor<T>(Node entity, Executor<T> executor) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            if (!Instance.executorDict.TryGetValue(key, out var delegates))
            {
                delegates = new Delegate[Constants.behaviorTypeArray.Length];
                delegates[index] = executor;
                Instance.executorDict.Add(key, delegates);
            }
            else if (delegates[index] is Executor<T> executors)
            {
                executors += executor;
                delegates[index] = executors;
                Instance.executorDict[key] = delegates;
            }
            else
            {
                delegates[index] = executor;
            }
        }


        public static void LogoutExecutor<T>(Node entity, Executor<T> executor) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            if (Instance.executorDict.TryGetValue(key, out var delegates) && delegates[index] is Executor<T> executors)
            {
                executors -= executor;
                delegates[index] = executors;
                Instance.executorDict[key] = delegates;
            }
        }


        public static bool Behave<T>(Node entity, T behavior) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;

            if (Instance.conditionDict.TryGetValue(key, out var conditionDelegates) &&
                conditionDelegates[index] is Condition<T> conditions)
            {
                if (conditions.GetInvocationList().AsParallel()
                    .Select(condition => (bool) condition.DynamicInvoke(entity, behavior)).Any(result => !result))
                {
                    return false;
                }
            }

            if (Instance.executorDict.TryGetValue(key, out var executorDelegates) &&
                executorDelegates[index] is Executor<T> executors)
            {
                executors(entity, behavior);
            }

            return true;
        }
    }
}