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

        /// <summary>
        /// 射线检测的层级。
        /// </summary>
        [Export(PropertyHint.Layers2dPhysics)]
        public uint raycastLayer;
        
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

            if (nameof(raycastLayer).Equals(e.name))
            {
                raycastLayer = (uint) e.value.intFinal;
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
                var from = Entity.Position;
                var to = from + translation;
                var exclude = new Array(Entity);
                var result = Entity.Raycast2D(from, to, exclude, raycastLayer);
                if (result.Count > 0)
                {
                    Entity.SendEvent(new EventMoverRaycast(result));
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