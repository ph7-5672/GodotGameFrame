using System;
using System.Collections.Generic;
using Frame.Module;
using Godot;
using Godot.Collections;

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


        public static bool HasComponent<T>(this Node2D entity) where T : IEntityComponent
        {
            foreach (var child in entity.GetChildren())
            {
                if (child is T component)
                {
                    return true;
                }
            }

            return false;
        }


        public static bool IsEntity(this Node node)
        {
            return node.Name.StartsWith("Entity-");
        }

        
        public static bool IsKilled(this Node2D entity)
        {
            return !entity.IsInsideTree();
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

        /// <summary>
        /// 射线检测第一个实体。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="exclude"></param>
        /// <param name="collisionLayer"></param>
        /// <returns></returns>
        public static Godot.Collections.Dictionary Raycast2D(this Node2D entity, Vector2 from, Vector2 to, Godot.Collections.Array exclude, uint collisionLayer = 2147483647)
        {
            if (entity.IsKilled())
            {
                return null;
            }

            var spaceState = entity.GetWorld2d().DirectSpaceState;
            return spaceState.IntersectRay(from, to, exclude, collisionLayer);

        }
    }
}