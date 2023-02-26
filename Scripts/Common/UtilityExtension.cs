using System;
using System.Collections.Generic;
using Frame.Module;
using Godot;
using Godot.Collections;
using Object = Godot.Object;

namespace Frame.Common
{
    public static class UtilityExtension
    {
        public static List<Node> GetComponents(this Node entity)
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


        public static bool HasComponent<T>(this Node entity) where T : IEntityComponent
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

        
        public static bool IsKilled(this Node entity)
        {
            return !entity.IsInsideTree();
        }

        public static EntityType GetEntityType(this Node entity)
        {
            var split = entity.Name.Split('-');
            var typeName = split[1];
            return (EntityType) Enum.Parse(typeof(EntityType), typeName);
        }

        public static string GetEntityId(this Node entity)
        {
            var split = entity.Name.Split('-');
            return split[2];
        }

        
        /// <summary>
        /// 射线检测第一个实体。
        /// </summary>
        /// <param name="world2D"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="exclude"></param>
        /// <param name="collisionLayer"></param>
        /// <returns></returns>
        public static Godot.Collections.Dictionary Raycast2D(this World2D world2D, Vector2 from, Vector2 to, Godot.Collections.Array exclude, uint collisionLayer = 2147483647)
        {
            var spaceState = world2D.DirectSpaceState;
            return spaceState.IntersectRay(from, to, exclude, collisionLayer);

        }



        public static void LoginBehaviorCondition<T>(this Object entity, Condition<T> condition) where T : struct
        {
            ModuleBehavior<T>.LoginCondition(entity, condition);
        }

        public static void LoginBehaviorExecutor<T>(this Object entity, Executor<T> executor) where T : struct
        {
            ModuleBehavior<T>.LoginExecutor(entity, executor);
        }

        public static bool Behave<T>(this Object entity, T behavior) where T : struct
        {
            return ModuleBehavior<T>.Behave(entity, behavior);
        }



        public static float GetValue(this Node2D entity, string name)
        {
            return ModuleEntity.GetValue(entity, name);
        }


        public static int GetIntValue(this Node2D entity, string name)
        {
            return (int) GetValue(entity, name);
        }

        public static void SetValue(this Node2D entity, string name, float value)
        {
            ModuleEntity.SetValue(entity, name, value);
        }



    }
}