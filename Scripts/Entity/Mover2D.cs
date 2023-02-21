using System.Collections;
using Frame.Common;
using Frame.Module;
using Godot;
using Godot.Collections;

namespace Frame.Entity
{
    /// <summary>
    /// 基本移动组件。
    /// 只提供方向和速度。
    /// </summary>
    public class Mover2D : EntityComponentBase<Node2D>
    {
        [Export]
        public float defaultSpeed;

        /// <summary>
        /// 是否开启射线检测。
        /// </summary>
        [Export]
        public bool enableRaycast;
        
        protected Vector2 velocity;

        protected Value speed;

        /// <summary>
        /// 移动速度。
        /// </summary>
        public Value Speed => speed;

        public override void Reset()
        {
            base.Reset();
            velocity = Vector2.Zero;
            speed = Value.Zero;
            speed.basic = defaultSpeed;
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            ModuleEvent.Subscribe<EventArrowInput>(OnArrowInput, Entity);
            ModuleEvent.Subscribe<EventValueUpdate>(OnValueUpdate, Entity);
        }

        protected virtual void OnArrowInput(object sender, EventArrowInput e)
        {
            velocity = e.arrow;
        }
        
        
        protected virtual void OnValueUpdate(object sender, EventValueUpdate e)
        {
            if (nameof(speed).Equals(e.name))
            {
                speed = e.value;
            }
        }
        
        protected virtual void Translate(float delta)
        {
            var translation = velocity * speed.final * delta * Constants.unitMeter;
            Entity.Translate(translation);
        }

        protected virtual void Raycast(float delta)
        {
            if (enableRaycast)
            {
                var translation = velocity * speed.final * delta * Constants.unitMeter;
                var spaceState = Entity.GetWorld2d().DirectSpaceState;
                var from = Entity.Position;
                var to = from + translation;
                var exclude = new Array(Entity);
                var results = spaceState.IntersectRay(from, to, exclude);
                if (results != null && results.Count > 0)
                {
                    Entity.SendEvent(new EventMoverRaycast(results));
                }

                
            }
        }


        public override void _PhysicsProcess(float delta)
        {
            Raycast(delta);
            Translate(delta);
        }
    }
}