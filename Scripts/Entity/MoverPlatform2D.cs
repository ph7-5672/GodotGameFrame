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
        /// <summary>
        /// 默认弹跳力。
        /// </summary>
        [Export]
        public float defaultBounce;

        /// <summary>
        /// 重力。
        /// </summary>
        [Export]
        public float gravity = 9.8f;
        
        /// <summary>
        /// 是否接触地板。
        /// </summary>
        protected bool isOnFloor;
        
        /// <summary>
        /// 吸附。
        /// </summary>
        protected Vector2 snap;


        protected Value bounce;

        /// <summary>
        /// 弹跳力。
        /// </summary>
        public Value Bounce => bounce;


        protected KinematicBody2D kinematicEntity => Entity as KinematicBody2D;

        public override void Reset()
        {
            base.Reset();
            bounce = Value.Zero;
            bounce.basic = defaultBounce;
            
            snap = Vector2.Zero;
            isOnFloor = false;
        }

        public override void _PhysicsProcess(float delta)
        {
            ApplyGravity(delta);
            Translate(delta);
            OnFloorCheck(delta);
        }
        
        
        protected override void OnValueUpdate(object sender, EventValueUpdate e)
        {
            base.OnValueUpdate(sender, e);
            if (nameof(bounce).Equals(e.name))
            {
                bounce = e.value;
            }
        }

        protected override void OnArrowInput(object sender, EventArrowInput e)
        {
            // x轴移动。
            velocity.x = e.arrow.x;
            var arrowY = e.arrow.y;
            
            if (!isOnFloor || arrowY == 0)
            {
                return;
            }

            // y轴跳跃或下蹲。
            if (arrowY < 0)
            {
                velocity.y = -bounce.final;
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

        protected override void Translate(float delta)
        {
            var translation = new Vector2(velocity.x * speed.final, velocity.y);
            translation *= Constants.unitMeter;
            kinematicEntity.MoveAndSlideWithSnap(translation, snap, Vector2.Up, true);
        }
        
        protected void OnFloorCheck(float delta)
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