using System;
using Frame.Common;
using Frame.Module;
using Godot;
using Array = Godot.Collections.Array;

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

        /// <summary>
        /// 移动距离限制。
        /// </summary>
        [Export]
        public float movedRange = -1;
        
        /// <summary>
        /// 走过的距离。
        /// </summary>
        protected float moved;
        
        protected Vector2 velocity;

        protected Value speed;

        /// <summary>
        /// 移动速度。
        /// </summary>
        public Value Speed => speed;

        public override void Reset()
        {
            base.Reset();
            moved = 0f;
            movedRange = -1f;
            
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
            
            if (nameof(movedRange).Equals(e.name))
            {
                movedRange = e.value.final;
            }
        }
        
        protected virtual void Translate(Vector2 translation)
        {
            Entity.Translate(translation);
        }

        protected virtual void Raycast(Vector2 translation)
        {
            if (enableRaycast)
            {
                var from = Entity.Position;
                var to = from + translation;
                var exclude = new Array(Entity);
                var result = Entity.Raycast2D(from, to, exclude, raycastLayer);
                if (result != null && result.Count > 0)
                {
                    Entity.SendEvent(new EventMoverRaycast(result));
                }
                
            }
        }

        protected virtual Vector2 GetTranslation(float delta)
        {
            return velocity * speed.final * delta * Constants.unitMeter;
        }


        public override void _PhysicsProcess(float delta)
        {
            if (Math.Abs(moved - movedRange) < float.Epsilon)
            {
                Entity.SendEvent(new EventMovedToRange(movedRange));
            }

            var translation = GetTranslation(delta);
            var length = translation.Length();
            var expectedMoved = moved + length;
            if (movedRange >= 0 && expectedMoved >= movedRange)
            {
                // 求出实际距离和预算距离的比。
                var scale = (movedRange - moved) / length;
                translation *= scale;
                expectedMoved = movedRange;
            }

            moved = expectedMoved;

            Raycast(translation);
            Translate(translation);
        }
    }
}