using Frame.Common;
using Godot;

namespace Frame.Logic
{
    public class LogicMove2D : LogicBase<Node2D>
    {
        public override ValueType ValueType => ValueType.Move2D;

        protected override void Ready(Node2D entity)
        {
            entity.LoginBehaviorExecutor<BehaviorMove>(Move);
        }

        protected override void Dispose(Node2D entity)
        {
            entity.LogoutBehaviorExecutor<BehaviorMove>(Move);
        }

        protected override void Process(Node2D entity, float delta)
        {
            var move2D = entity.GetValue<ValueMove2D>();
            if (move2D.processMode == ProcessMode.Idle)
            {
                ProcessLogic(entity, move2D, delta);
            }
        }

        protected override void PhysicsProcess(Node2D entity, float delta)
        {
            var move2D = entity.GetValue<ValueMove2D>();
            if (move2D.processMode == ProcessMode.Physics)
            {
                ProcessLogic(entity, move2D, delta);
            }
        }

        protected virtual void Translate(Node2D entity, Vector2 translation)
        {
            entity.Translate(translation);
        }

        protected virtual Vector2 GetTranslation(Node2D entity, ValueMove2D value, float delta)
        {
            return value.velocity * delta * Constants.unitMeter;
        }

        private void ProcessLogic(Node2D entity, ValueMove2D value, float delta)
        {
            var translation = GetTranslation(entity, value, delta);
            if (entity.Behave(new BehaviorTranslate(translation)))
            {
                Translate(entity, translation);
            }
        }

        private void Move(Node entity, BehaviorMove behavior)
        {
            var move2D = entity.GetValue<ValueMove2D>();
            move2D.velocity = behavior.velocity;
            entity.SetValue(move2D);
        }

        [Event(EventType.EntityZeroHp)]
        public static void OnZeroHp(Node entity)
        {
            if (!entity.HasValue<ValueMove2D>())
            {
                return;
            }

            var move2D = entity.GetValue<ValueMove2D>();
            move2D.velocity = Vector2.Zero;
            entity.SetValue(move2D);
        }

    }
}