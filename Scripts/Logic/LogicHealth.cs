using System.Diagnostics;
using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Logic
{
    public class LogicHealth : LogicBase<Node>
    {
        protected override ValueType ValueType => ValueType.Health;

        protected override void Ready(Node entity)
        {
            entity.LoginBehaviorCondition<BehaviorMove>(CanMove);
            entity.LoginBehaviorCondition<BehaviorShoot>(CanShoot);
            
            entity.LoginBehaviorExecutor<BehaviorDamage>(Damage);
        }

        protected override void Dispose(Node entity)
        {
            entity.LogoutBehaviorCondition<BehaviorMove>(CanMove);
            entity.LogoutBehaviorCondition<BehaviorShoot>(CanShoot);
            
            entity.LogoutBehaviorExecutor<BehaviorDamage>(Damage);
        }

        private void Damage(Node entity, BehaviorDamage behavior)
        {
            AddHp(behavior.target, -behavior.value);
        }

        private void AddHp(Node entity, float value)
        {
            if (entity.TryGetValue(ValueType, out ValueHealth health))
            {
                UpdateHp(entity, health, health.point + value);
            }
        }


        private void UpdateHp(Node entity, ValueHealth health, float value)
        {
            if (value <= 0)
            {
                ModuleEvent.Send(EventType.EntityZeroHp, entity);
            }
            health.point = Mathf.Clamp(value, 0f, health.limit.final);
            entity.SetValue(ValueType, health);
        }

        public static bool IsAlive(Node entity)
        {
            if (entity.TryGetValue(ValueType.Health, out ValueHealth value))
            {
                return value.point > 0;
            }
            return true;
        }

        private bool CanShoot(Node entity, BehaviorShoot behavior) => IsAlive(entity);

        private bool CanMove(Node entity, BehaviorMove behavior) => behavior.velocity != Vector2.Zero && IsAlive(entity);
        
        
    }
}