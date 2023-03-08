using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void LoginBehaviorCondition<T>(this Node entity, Condition<T> condition) where T : struct, IEntityBehavior
        {
            GameFrame.Behavior.LoginCondition(entity, condition);
        }

        public static void LoginBehaviorExecutor<T>(this Node entity, Executor<T> executor) where T : struct, IEntityBehavior
        {
            GameFrame.Behavior.LoginExecutor(entity, executor);
        }
        
        public static void LogoutBehaviorCondition<T>(this Node entity, Condition<T> condition) where T : struct, IEntityBehavior
        {
            GameFrame.Behavior.LogoutCondition(entity, condition);
        }
        
        public static void LogoutBehaviorExecutor<T>(this Node entity, Executor<T> executor) where T : struct, IEntityBehavior
        {
            GameFrame.Behavior.LogoutExecutor(entity, executor);
        }

        public static bool Behave<T>(this Node entity, T behavior) where T : struct, IEntityBehavior
        {
            return GameFrame.Behavior.Behave(entity, behavior);
        }

        public static T GetValue<T>(this Node entity) where T : struct, IEntityValue
        {
            return GameFrame.Entity.GetValue<T>(entity);
        }


        public static void SetValue(this Node entity, IEntityValue value) 
        {
            GameFrame.Entity.SetValue(entity, value);
        }

        public static bool HasValue(this Node entity, ValueType valueType)
        {
            return GameFrame.Entity.HasValue(entity, valueType);
        }

        public static bool HasValue<T>(this Node entity) where T : struct, IEntityValue
        {
            return GameFrame.Entity.HasValue<T>(entity);
        }


        public static BuffType AddFlags(this BuffType type, params BuffType[] types)
        {
            return types.Aggregate(type, (current, buffType) => current | buffType);
        }


        public static BuffType RemoveFlags(this BuffType type, params BuffType[] types)
        {
            return types.Aggregate(type, (current, buffType) => current ^ buffType);
        }

    }
}