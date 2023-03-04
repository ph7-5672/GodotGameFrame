using System.Collections.Generic;
using Frame.Common;
using Godot;
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

        public void FillPool(EntityType entityType, int size)
        {
            for (var i = 0; i < size; ++i)
            {
                var entity = Load(entityType);
                Kill(entity);
            }
        }

        public Node Spawn(EntityType entityType)
        {
            Node entity;
            if (entityPool.TryGetValue(entityType, out var pool) && pool.Count > 0)
            {
                entity = pool.Dequeue();
                GameFrame.EntityRoot.AddChild(entity);
            }
            else
            {
                entity = GameFrame.Scene.LoadInstance<Node>($"Entities/{entityType}", GameFrame.EntityRoot);
                entityTypes.Add(entity.GetInstanceId(), entityType);
            }
            GameFrame.Event.Send(EventType.EntitySpawn, entityType, entity);
            return entity;
        }

        private Node Load(EntityType entityType)
        {
            var entity = GameFrame.Scene.LoadInstance<Node>($"Entities/{entityType}", GameFrame.EntityRoot);
            entityTypes.Add(entity.GetInstanceId(), entityType);
            return entity;
        }


        public void Kill(Node entity)
        {
            if (!entity.IsInsideTree())
            {
                return;
            }

            // 清除所有组件。
            RemoveValues(entity);
            
            var entityType = GetEntityType(entity);
            if (!entityPool.TryGetValue(entityType, out var pool))
            {
                pool = new Queue<Node>();
                entityPool.Add(entityType, pool);
            }

            pool.Enqueue(entity);
            
            // 作为游离节点不进行渲染。
            GameFrame.EntityRoot.RemoveChild(entity);
        }

        public EntityType GetEntityType(Node entity)
        {
            return entityTypes[entity.GetInstanceId()];
        }

        # region 实体属性

        private readonly Dictionary<ulong, IEntityValue[]> entityValuePool = new Dictionary<ulong, IEntityValue[]>();

        public T GetValue<T>(Node entity) where T : struct, IEntityValue
        {
            var instanceId = entity.GetInstanceId();
            var value = new T();
            var index = (int) value.Type;
            if (entityValuePool.TryGetValue(instanceId, out var values))
            {
                var result = values[index];
                if (result is T t)
                {
                    value = t;
                }
            }
            return value;
        }
        
        public void SetValue(Node entity, IEntityValue value)
        {
            
            var instanceId = entity.GetInstanceId();
            if (!entityValuePool.TryGetValue(instanceId, out var values))
            {
                values = new IEntityValue[Constants.valueTypeArray.Length];
                entityValuePool.Add(instanceId, values);
            }

            var index = (int) value.Type;
            var origin = values[index];
            values[index] = value;
            if (origin == null)
            {
                GameFrame.ForeachLogics(logic => logic.Ready(entity));
            }

            GameFrame.Event.Send(EventType.EntitySetValue, entity, value);
            
        }
        
        public bool HasValue<T>(Node entity) where T : struct, IEntityValue
        {
            return HasValue(entity, new T().Type);
        }

        public bool HasValue(Node entity, ValueType valueType)
        {
            var instanceId = entity.GetInstanceId();
            var index = (int) valueType;
            if (entityValuePool.TryGetValue(instanceId, out var values))
            {
                return values[index] != null;
            }
            return false;
        }

        public void RemoveValue(Node entity, ValueType valueType)
        {
            var instanceId = entity.GetInstanceId();
            var index = (int) valueType;
            if (entityValuePool.TryGetValue(instanceId, out var values))
            {
                values[index] = null;
                GameFrame.ForeachLogics(logic => logic.Dispose(entity));
            }
        }

        public void RemoveValues(Node entity)
        {
            var instanceId = entity.GetInstanceId();
            if (entityValuePool.TryGetValue(instanceId, out var values))
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = null;
                    GameFrame.ForeachLogics(logic => logic.Dispose(entity));
                }
            }
        }

        #endregion

        [Event(EventType.EntitySpawn)]
        public void OnEntitySpawn(EntityType type, Node entity)
        {
            var instanceId = entity.GetInstanceId();
            if (!entityValuePool.TryGetValue(instanceId, out var values))
            {
                values = new IEntityValue[Constants.valueTypeArray.Length];
                entityValuePool.Add(instanceId, values);
            }
        }
    }
}