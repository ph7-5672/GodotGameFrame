using Frame.Common;
using Godot;

namespace Frame.Logic
{
    public class LogicMove2DPlatform : LogicBase<KinematicBody2D>
    {
        protected override ValueType ValueType => ValueType.Move2DPlatform;

        protected override void Ready(KinematicBody2D entity)
        {
            entity.LoginBehaviorExecutor<BehaviorMove>(Move);
        }

        protected override void Dispose(KinematicBody2D entity)
        {
            entity.LogoutBehaviorExecutor<BehaviorMove>(Move);
        }

        protected override void Process(KinematicBody2D entity, float delta)
        {
        }

        protected override void PhysicsProcess(KinematicBody2D entity, float delta)
        {
            if (!entity.TryGetValue(ValueType.Move2DPlatform, out ValueMove2DPlatform value))
            {
                return;
            }
            
            const float gravity = 9.8f;
            value.velocity.y += gravity * delta * Constants.unitMeter;
            
            value.isOnFloor = entity.IsOnFloor();
            if (value.isOnFloor)
            {
                value.velocity.y = Mathf.Min(0f, value.velocity.y);
                value.snap = Vector2.Down * gravity * Constants.unitMeter * delta;
            }
            
            var translation = new Vector2(value.velocity.x * value.speed.final, value.velocity.y);
            translation *= Constants.unitMeter;
            
            entity.MoveAndSlideWithSnap(translation, value.snap, Vector2.Up, true);
            
            entity.SetValue(ValueType.Move2DPlatform, value);
        }
        
        private void Move(Node entity, BehaviorMove behavior)
        {
            if (!entity.TryGetValue(ValueType.Move2DPlatform, out ValueMove2DPlatform value))
            {
                return;
            }

            // x轴移动。
            value.velocity.x = behavior.velocity.x;
            var arrowY = behavior.velocity.y;
            
            if (!value.isOnFloor || arrowY == 0)
            {
                entity.SetValue(ValueType.Move2DPlatform, value);
                return;
            }

            // y轴跳跃或下蹲。
            if (arrowY < 0)
            {
                value.velocity.y = -value.bounce.final * Constants.unitMeter;
                value.snap = Vector2.Zero;
            }
            else
            {
                // TODO 下蹲。
            }
            
            entity.SetValue(ValueType.Move2DPlatform, value);
            
        }
        
    }
}