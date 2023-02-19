using System;
using System.Collections.Generic;
using Frame.Module;
using Godot;

namespace Frame.Common
{
    public static class ExtensionUtility
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


        public static void SetValue(this Node entity, string valueName, ref Value value, Value change)
        {
            if (value != change)
            {
                var origin = value;
                value = change;
                EventModule.Send(new ValueChangeEvent(valueName, origin, change), entity);
            }
        }

        public static Vector2 ToWorldPoint(this Vector2 viewportPosition, Transform2D canvasTransform)
        {
            var affineInverse = canvasTransform.AffineInverse();
            var halfScreen = new Transform2D().Translated(viewportPosition);
            return affineInverse * halfScreen * Vector2.Zero;
        }

    }
}