using System;
using System.Collections.Generic;
using Frame.Module;
using Godot;

namespace Frame.Common
{
    public static class UtilityExtension
    {
        public static List<Node> GetComponents(this Node2D entity)
        {
            var result = new List<Node>();
            foreach (var child in entity.GetChildren())
            {
                if (child is IEntityComponent && child is Node node)
                {
                    result.Add(node);
                }
            }
            return result;
        }


        public static T GetComponent<T>(this Node2D entity) where T : IEntityComponent
        {
            foreach (var child in entity.GetChildren())
            {
                if (child is T component)
                {
                    return component;
                }
            }

            return default;
        }


        public static bool IsEntity(this Node node)
        {
            return node.Name.StartsWith("Entity-");
        }


        public static EntityType GetEntityType(this Node2D entity)
        {
            var split = entity.Name.Split('-');
            var typeName = split[1];
            return (EntityType) Enum.Parse(typeof(EntityType), typeName);
        }


        public static void SendEvent<T>(this object sender, T eventArgs) where T : struct, IEventArgs
        {
            ModuleEvent.Send(eventArgs, sender);
        }

    }
}