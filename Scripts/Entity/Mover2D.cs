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
        /// 射线检测移动路径的宽度，为0时不检测。
        /// </summary>
        [Export]
        public float raycastWidth;

        /// <summary>
        /// 射线检测的层级。
        /// </summary>
        [Export(PropertyHint.Layers2dPhysics)]
        public uint raycastLayer;

        /// <summary>
        /// 移动距离限制，小于0时不限制。
        /// </summary>
        [Export]
        public float movedRange = -1;

        /// <summary>
        /// 移动计算方式。
        /// </summary>
        [Export]
        public ClippedCamera.ProcessModeEnum processMode = ClippedCamera.ProcessModeEnum.Physics;
        
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
            
            if (nameof(raycastWidth).Equals(e.name))
            {
                raycastWidth = e.value.final;
            }
        }
        
        protected virtual void Translate(Vector2 translation)
        {
            Entity.Translate(translation);
        }

        protected virtual void Raycast(Vector2 translation)
        {
            if (raycastWidth > 0)
            {
                var from = Entity.Position;
                var to = from - translation;
                var exclude = new Array(Entity);
                var result = Entity.Raycast2D(from, to, exclude, raycastLayer);

                if (result == null || result.Count == 0)
                {
                    var topTranslation = translation.Rotated(90f).Normalized() * raycastWidth / 2f;
                    var topFrom = from + topTranslation;
                    var topTo = to + topTranslation;
                    result = Entity.Raycast2D(topFrom, topTo, exclude, raycastLayer);
                }

                if (result == null || result.Count == 0)
                {
                    var bottomTranslation = translation.Rotated(-90f).Normalized() * raycastWidth / 2f;
                    var bottomFrom = from + bottomTranslation;
                    var bottomTo = to + bottomTranslation;
                    result = Entity.Raycast2D(bottomFrom, bottomTo, exclude, raycastLayer);
                }

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


        protected virtual void Process(float delta)
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

            Translate(translation);
            Entity.SendEvent(new EventTranslate(translation, moved));
            Raycast(translation);
        }

        public override void _Process(float delta)
        {
            if (processMode == ClippedCamera.ProcessModeEnum.Idle)
            {
                Process(delta);
            }
        }
        

        public override void _PhysicsProcess(float delta)
        {
            if (processMode == ClippedCamera.ProcessModeEnum.Physics)
            {
                Process(delta);
            }
        }
    }
}