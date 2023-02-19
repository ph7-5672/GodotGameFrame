using System;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class EntityModule : Singleton<EntityModule>
    {

        public static Node2D Spawn(EntityType entityType)
        {
            var name = entityType.ToString();
            var instance = SceneModule.LoadInstance<Node2D>($"Entities/{name}", GameFrame.EntityRoot);
            instance.Name = $"Entity-{entityType}-{Guid.NewGuid()}";
            return instance;
        }

        public static void Kill(Node2D entity)
        {
            // 跟信号一起用就需要延迟调用。
            GameFrame.EntityRoot.CallDeferred("remove_child", entity);
            foreach (Node child in entity.GetChildren())
            {
                child.Dispose();
            }
            entity.Dispose();
        }

    }
}