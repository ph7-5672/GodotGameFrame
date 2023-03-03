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
        }

        protected override void Dispose(Node entity)
        {
            entity.LogoutBehaviorCondition<BehaviorMove>(CanMove);
            entity.LogoutBehaviorCondition<BehaviorShoot>(CanShoot);
        }

        private bool IsAlive(Node entity)
        {
            if (entity.TryGetValue(ValueType.Health, out ValueHealth value))
            {
                return value.point > 0;
            }
            return true;
        }

        private bool CanShoot(Node entity, BehaviorShoot behavior) => IsAlive(entity);

        private bool CanMove(Node entity, BehaviorMove behavior) => IsAlive(entity);
        
        
    }
}