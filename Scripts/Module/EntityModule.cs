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

        
        
    }
}