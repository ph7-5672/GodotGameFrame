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
        private static readonly Dictionary<EntityType, Queue<Node2D>> entityPool = new Dictionary<EntityType, Queue<Node2D>>();
        
        public static Node2D Spawn(EntityType entityType, Vector2 position = new Vector2())
        {
            Node2D entity;
            if (entityPool.TryGetValue(entityType, out var pool) && pool.Count > 0)
            {
                entity = pool.Dequeue();
                GameFrame.EntityRoot.AddChild(entity);
            }
            else
            {
                var name = entityType.ToString();
                entity = ModuleScene.LoadInstance<Node2D>($"Entities/{name}", GameFrame.EntityRoot);
            }
            entity.Name = $"Entity-{entityType}-{Guid.NewGuid()}";
            entity.Position = position;
            return entity;
        }

        public static void Kill(Node2D entity)
        {
            if (!entity.IsInsideTree())
            {
                return;
            }

            var entityType = entity.GetEntityType();
            if (!entityPool.TryGetValue(entityType, out var pool))
            {
                pool = new Queue<Node2D>();
                entityPool.Add(entityType, pool);
            }
            pool.Enqueue(entity);
            foreach (var component in entity.GetComponents().Cast<IEntityComponent>())
            {
                component.Reset();
            }
            // 作为游离节点不进行渲染。
            //GameFrame.EntityRoot.CallDeferred("remove_child", entity);
            GameFrame.EntityRoot.RemoveChild(entity);
        }

    }
}