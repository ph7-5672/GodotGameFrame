using Frame.Common;
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
            var health = entity.GetValue<ValueHealth>();
            UpdateHp(entity, health, health.point + value);
        }


        private void UpdateHp(Node entity, ValueHealth health, float value)
        {
            if (value <= 0)
            {
                GameFrame.Event.Send(EventType.EntityZeroHp, entity);
            }
            health.point = Mathf.Clamp(value, 0f, health.limit.final);
            entity.SetValue(health);
        }

        public static bool IsAlive(Node entity)
        {
            var health = entity.GetValue<ValueHealth>();
            return health.point > 0 && health.limit.final > 0;
        }

        private bool CanShoot(Node entity, BehaviorShoot behavior) => IsAlive(entity);

        private bool CanMove(Node entity, BehaviorMove behavior) => behavior.velocity != Vector2.Zero && IsAlive(entity);
        
        
    }
}