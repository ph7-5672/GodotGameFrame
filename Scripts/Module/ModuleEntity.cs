using System;
using System.Collections.Generic;
using System.Linq;
using Frame.Common;
using Godot;
using Object = Godot.Object;
using ValueType = Frame.Common.ValueType;

namespace Frame.Module
{
    public class ModuleEntity : Singleton<ModuleEntity>
    {
        /// <summary>
        /// 实体池。
        /// </summary>
        private readonly Dictionary<EntityType, Queue<Node>> entityPool = new Dictionary<EntityType, Queue<Node>>();

        private readonly Dictionary<ulong, EntityType> entityTypes = new Dictionary<ulong, EntityType>();

        public static void FillPool(EntityType entityType, int size)
        {
            for (var i = 0; i < size; ++i)
            {
                var entity = Spawn(entityType);
                Kill(entity);
            }
        }

        public static Node Spawn(EntityType entityType)
        {
            Node entity;
            if (Instance.entityPool.TryGetValue(entityType, out var pool) && pool.Count > 0)
            {
                entity = pool.Dequeue();
                GameFrame.EntityRoot.AddChild(entity);
            }
            else
            {
                entity = ModuleScene.LoadInstance<Node>($"Entities/{entityType}", GameFrame.EntityRoot);
                Instance.entityTypes.Add(entity.GetInstanceId(), entityType);
            }
            return entity;
        }


        public static void Kill(Node entity)
        {
            if (!entity.IsInsideTree())
            {
                return;
            }

            // 清除所有组件。
            RemoveValues(entity);
            
            var entityType = GetEntityType(entity);
            if (!Instance.entityPool.TryGetValue(entityType, out var pool))
            {
                pool = new Queue<Node>();
                Instance.entityPool.Add(entityType, pool);
            }

            pool.Enqueue(entity);
            
            // 作为游离节点不进行渲染。
            GameFrame.EntityRoot.RemoveChild(entity);
        }

        public static EntityType GetEntityType(Node entity)
        {
            return Instance.entityTypes[entity.GetInstanceId()];
        }

        # region 实体属性

        private readonly Dictionary<ulong, IEntityValue[]> entityValuePool = new Dictionary<ulong, IEntityValue[]>();

        public static bool TryGetValue<T>(Node entity, ValueType valueType, out T value) where T : struct, IEntityValue
        {
            var instanceId = entity.GetInstanceId();
            if (Instance.entityValuePool.TryGetValue(instanceId, out var values))
            {
                var result = values[(int) valueType];
                if (result is T t)
                {
                    value = t;
                    return true;
                }
            }
            value = default;
            return false;
        }
        
        public static void SetValue(Node entity, ValueType valueType, IEntityValue value)
        {
            var instanceId = entity.GetInstanceId();
            if (!Instance.entityValuePool.TryGetValue(instanceId, out var values))
            {
                values = new IEntityValue[Constants.valueTypeArray.Length];
                Instance.entityValuePool.Add(instanceId, values);
            }

            var index = (int) valueType;
            var origin = values[index];
            values[index] = value;
            if (origin == null)
            {
                GameFrame.ForeachLogics(logic => logic.Ready(entity));
            }

            
        }
        
        public static bool HasValue(Node entity, ValueType valueType)
        {
            var instanceId = entity.GetInstanceId();
            if (Instance.entityValuePool.TryGetValue(instanceId, out var values))
            {
                var value = values[(int) valueType];
                return value != null;
            }

            return false;
        }


        public static void RemoveValue(Node entity, ValueType valueType)
        {
            var instanceId = entity.GetInstanceId();
            var index = (int) valueType;
            if (Instance.entityValuePool.TryGetValue(instanceId, out var values))
            {
                values[index] = null;
                GameFrame.ForeachLogics(logic => logic.Dispose(entity));
            }
        }

        public static void RemoveValues(Node entity)
        {
            var instanceId = entity.GetInstanceId();
            if (Instance.entityValuePool.TryGetValue(instanceId, out var values))
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = null;
                    GameFrame.ForeachLogics(logic => logic.Dispose(entity));
                }
            }
        }

        #endregion

    }
}