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
        public void LoginCondition<T>(Node entity, Condition<T> condition) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            if (!conditionDict.TryGetValue(key, out var delegates))
            {
                delegates = new Delegate[Constants.behaviorTypeArray.Length];
                delegates[index] = condition;
                conditionDict.Add(key, delegates);
            }
            else if (delegates[index] is Condition<T> conditions)
            {
                conditions += condition;
                delegates[index] = conditions;
                conditionDict[key] = delegates;
            }
            else
            {
                delegates[index] = condition;
            }
            
        }
        
        public void LogoutCondition<T>(Node entity, Condition<T> condition) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            if (conditionDict.TryGetValue(key, out var delegates) &&
                delegates[index] is Condition<T> conditions)
            {
                conditions -= condition;
                delegates[index] = conditions;
                conditionDict[key] = delegates;
            }
        }

        /// <summary>
        /// 注册执行节点。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="executor"></param>
        public void LoginExecutor<T>(Node entity, Executor<T> executor) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            
            if (!executorDict.TryGetValue(key, out var delegates))
            {
                delegates = new Delegate[Constants.behaviorTypeArray.Length];
                delegates[index] = executor;
                executorDict.Add(key, delegates);
            }
            else if (delegates[index] is Executor<T> executors)
            {
                executors += executor;
                delegates[index] = executors;
                executorDict[key] = delegates;
            }
            else
            {
                delegates[index] = executor;
            }
        }

        public void LogoutExecutor<T>(Node entity, Executor<T> executor) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;
            if (executorDict.TryGetValue(key, out var delegates) && delegates[index] is Executor<T> executors)
            {
                executors -= executor;
                delegates[index] = executors;
                executorDict[key] = delegates;
            }
        }


        public bool Behave<T>(Node entity, T behavior) where T : struct, IEntityBehavior
        {
            var key = entity.GetInstanceId();
            var index = (int) new T().Type;

            if (conditionDict.TryGetValue(key, out var conditionDelegates) &&
                conditionDelegates[index] is Condition<T> conditions)
            {
                if (conditions.GetInvocationList().AsParallel()
                    .Select(condition => (bool) condition.DynamicInvoke(entity, behavior)).Any(result => !result))
                {
                    return false;
                }
            }

            if (executorDict.TryGetValue(key, out var executorDelegates) &&
                executorDelegates[index] is Executor<T> executors)
            {
                executors(entity, behavior);
            }

            return true;
        }
        
        [Event(EventType.EntitySpawn)]
        public void OnEntitySpawn(EntityType type, Node entity)
        {
            var key = entity.GetInstanceId();
            if (!conditionDict.TryGetValue(key, out var conditions))
            {
                conditions = new Delegate[Constants.behaviorTypeArray.Length];
                conditionDict.Add(key, conditions);
            }
            
            if (!executorDict.TryGetValue(key, out var executors))
            {
                executors = new Delegate[Constants.behaviorTypeArray.Length];
                executorDict.Add(key, executors);
            }
        }
    }
}