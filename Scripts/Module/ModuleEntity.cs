using System;
using System.Collections.Generic;
using System.Linq;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class ModuleEntity : Singleton<ModuleEntity>
    {
        /// <summary>
        /// 实体池。
        /// </summary>
        private readonly Dictionary<EntityType, Queue<Node2D>> entityPool = new Dictionary<EntityType, Queue<Node2D>>();

        public static Node2D Spawn2D(EntityType entityType, Vector2 position = new Vector2())
        {
            Node2D entity;
            if (Instance.entityPool.TryGetValue(entityType, out var pool) && pool.Count > 0)
            {
                entity = pool.Dequeue();
                GameFrame.EntityRoot.AddChild(entity);
            }
            else
            {
                entity = ModuleScene.LoadInstance<Node2D>($"Entities/{entityType}", GameFrame.EntityRoot);
            }

            entity.Name = $"Entity-{entityType}-{Guid.NewGuid()}";
            entity.Position = position;

            SetValuesByDatabase(entityType, entity);
            return entity;
        }


        private static void SetValuesByDatabase(EntityType entityType, Node2D entity)
        {
            foreach (var dictionary in from value in ModuleDatabase.Database.Values
                from dictionary in value
                where dictionary.TryGetValue("entityType", out var type) && type.Equals(entityType.ToString())
                select dictionary)
            {
                foreach (var pair in dictionary)
                {
                    if (float.TryParse(pair.Value, out var f))
                    {
                        entity.SetValue(pair.Key, f);
                    }
                }
            }
        }


        public static void Kill(Node2D entity)
        {
            if (!entity.IsInsideTree())
            {
                return;
            }

            ResetValues(entity);
            
            foreach (var component in entity.GetComponents().Cast<IEntityComponent>())
            {
                component.Reset();
            }
            
            var entityType = entity.GetEntityType();
            if (!Instance.entityPool.TryGetValue(entityType, out var pool))
            {
                pool = new Queue<Node2D>();
                Instance.entityPool.Add(entityType, pool);
            }

            pool.Enqueue(entity);
            // 作为游离节点不进行渲染。
            //GameFrame.EntityRoot.CallDeferred("remove_child", entity);
            GameFrame.EntityRoot.RemoveChild(entity);
        }


        # region 实体属性

        private readonly Dictionary<ulong, Dictionary<string, float>> entityValues =
            new Dictionary<ulong, Dictionary<string, float>>();

        public static float GetValue(Node2D entity, string valueName)
        {
            var instanceId = entity.GetInstanceId();
            if (!Instance.entityValues.TryGetValue(instanceId, out var values))
            {
                return 0f;
            }

            return values.TryGetValue(valueName, out var value) ? value : 0f;
        }


        public static void SetValue(Node2D entity, string valueName, float value)
        {
            var instanceId = entity.GetInstanceId();
            if (!Instance.entityValues.TryGetValue(instanceId, out var values))
            {
                values = new Dictionary<string, float>();
                Instance.entityValues.Add(instanceId, values);
            }

            if (!values.TryGetValue(valueName, out _))
            {
                values.Add(valueName, value);
            }

            values[valueName] = value;
        }


        public static void ResetValues(Node2D entity)
        {
            var instanceId = entity.GetInstanceId();
            if (Instance.entityValues.TryGetValue(instanceId, out var values))
            {
                var keys = values.Keys.ToArray();
                foreach (var key in keys)
                {
                    values[key] = 0f;
                }
            }
        }

        # endregion
    }
}