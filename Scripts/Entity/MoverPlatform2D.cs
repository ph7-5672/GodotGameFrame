using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 平台跳跃2d移动。
    /// </summary>
    public class MoverPlatform2D : Mover2D
    {
        
        public const float gravity = 9.8f;
        
        /// <summary>
        /// 是否接触地板。
        /// </summary>
        protected bool isOnFloor;
        
        /// <summary>
        /// 吸附。
        /// </summary>
        protected Vector2 snap;
        
        protected float bounce => Entity.GetValue("bounce");


        protected KinematicBody2D kinematicEntity => Entity as KinematicBody2D;

        public override void Reset()
        {
            base.Reset();
            processMode = ClippedCamera.ProcessModeEnum.Physics;
            snap = Vector2.Zero;
            isOnFloor = false;
        }

        public override void _PhysicsProcess(float delta)
        {
            ApplyGravity(delta);
            OnFloorCheck(delta);
            base._PhysicsProcess(delta);
        }
        
        protected override void Move(Object entity, BehaviorMove behavior)
        {
            // x轴移动。
            velocity.x = behavior.velocity.x;
            var arrowY = behavior.velocity.y;
            
            if (!isOnFloor || arrowY == 0)
            {
                return;
            }

            // y轴跳跃或下蹲。
            if (arrowY < 0)
            {
                velocity.y = -bounce * Constants.unitMeter;
                snap = Vector2.Zero;
            }
            else
            {
                // TODO 下蹲。
            }
            
        }

        protected virtual void ApplyGravity(float delta)
        {
            velocity.y += gravity * delta * Constants.unitMeter;
        }

        protected override void Translate(Vector2 translation)
        {
            kinematicEntity.MoveAndSlideWithSnap(translation, snap, Vector2.Up, true);
        }

        protected override Vector2 GetTranslation(float delta)
        {
            var translation = new Vector2(velocity.x * speed, velocity.y);
            translation *= Constants.unitMeter;
            return translation;
        }

        protected virtual void OnFloorCheck(float delta)
        {
            isOnFloor = kinematicEntity.IsOnFloor();
            if (isOnFloor)
            {
                velocity.y = Mathf.Min(0f, velocity.y);
                snap = Vector2.Down * gravity * Constants.unitMeter * delta;
            }
        }
        
        
    }
}