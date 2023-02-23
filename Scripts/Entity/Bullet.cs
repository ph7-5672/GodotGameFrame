using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 子弹组件，检测到其他实体时销毁。
    /// </summary>
    public class Bullet : EntityComponentBase<Line2D>
    {
        /// <summary>
        /// 拖尾长度。
        /// </summary>
        [Export]
        public float tailLength = 50f;

        protected Vector2 translation;

        protected bool isDying;
        
        public override void Reset()
        {
            Entity.ClearPoints();
            translation = Vector2.Zero;
            isDying = false;
        }

        protected override void SubscribeEvents()
        {
            ModuleEvent.Subscribe<EventMoverRaycast>(OnMoverRaycast, Entity);
            ModuleEvent.Subscribe<EventMovedToRange>(OnMovedToRange, Entity);
            ModuleEvent.Subscribe<EventTranslate>(OnTranslate, Entity);
            ModuleEvent.Subscribe<EventValueUpdate>(OnValueUpdate, Entity);
        }

        protected virtual void OnValueUpdate(object sender, EventValueUpdate e)
        {
            if (e.name.Equals("bulletWidth"))
            {
                Entity.Width = e.value.final;
            }
        }

        protected virtual void OnTranslate(object sender, EventTranslate e)
        {
            if (isDying)
            {
                return;
            }

            translation -= e.translation;
            if (translation.Length() < tailLength)
            {
                Entity.AddPoint(translation);
            }
            
        }

        protected virtual void OnMoverRaycast(object sender, EventMoverRaycast e)
        {
            var collider = e.collider;
            Die();
            ModuleEntity.Kill(collider);
        }
        
        protected virtual void OnMovedToRange(object sender, EventMovedToRange e)
        {
            // 距离限制销毁子弹。
            Die();
        }

        protected virtual void Die()
        {
            isDying = true;
            Entity.SendEvent(new EventArrowInput(Vector2.Zero));
        }

        public override void _Process(float delta)
        {
            if (!isDying)
            {
                return;
            }

            var pointCount = Entity.GetPointCount();
            if (pointCount > 0)
            {
                Entity.RemovePoint(pointCount - 1);
            }
            else
            {
                ModuleEntity.Kill(Entity);
            }
            
            
        }
    }
}