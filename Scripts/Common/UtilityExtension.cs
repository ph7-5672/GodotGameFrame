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



        public static void LoginBehaviorCondition<T>(this Node entity, Condition<T> condition) where T : struct
        {
            ModuleBehavior<T>.LoginCondition(entity, condition);
        }

        public static void LoginBehaviorExecutor<T>(this Node entity, Executor<T> executor) where T : struct
        {
            ModuleBehavior<T>.LoginExecutor(entity, executor);
        }
        
        public static void LogoutBehaviorCondition<T>(this Node entity, Condition<T> condition) where T : struct
        {
            ModuleBehavior<T>.LogoutCondition(entity, condition);
        }
        
        public static void LogoutBehaviorExecutor<T>(this Node entity, Executor<T> executor) where T : struct
        {
            ModuleBehavior<T>.LogoutExecutor(entity, executor);
        }

        public static bool Behave<T>(this Node entity, T behavior) where T : struct
        {
            return ModuleBehavior<T>.Behave(entity, behavior);
        }

        public static bool TryGetValue<T>(this Node entity, ValueType valueType, out T value)
            where T : struct, IEntityValue
        {
            return ModuleEntity.TryGetValue<T>(entity, valueType, out value);
        }


        public static void SetValue(this Node entity, ValueType valueType, IEntityValue value) 
        {
            ModuleEntity.SetValue(entity, valueType, value);
        }

        public static bool HasValue(this Node entity, ValueType valueType)
        {
            return ModuleEntity.HasValue(entity, valueType);
        }
        

    }
}