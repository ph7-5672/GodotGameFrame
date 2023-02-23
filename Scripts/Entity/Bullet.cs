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
        public float tailLength = 20f;
        
        public override void Reset()
        {
            Entity.ClearPoints();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            ModuleEvent.Subscribe<EventMoverRaycast>(OnMoverRaycast, Entity);
            ModuleEvent.Subscribe<EventMovedToRange>(OnMovedToRange, Entity);
            ModuleEvent.Subscribe<EventTranslate>(OnTranslate, Entity);
        }

        protected virtual void OnTranslate(object sender, EventTranslate e)
        {
            var translation = e.translation;
            var pointCount = Entity.GetPointCount();

            var pointPos = -translation * pointCount;
            if (pointPos.Length() < tailLength)
            {
                Entity.AddPoint(pointPos);
            }
        }

        protected virtual void OnMoverRaycast(object sender, EventMoverRaycast e)
        {
            var collider = e.collider;
            ModuleEntity.Kill(Entity);
            ModuleEntity.Kill(collider);
        }
        
        protected virtual void OnMovedToRange(object sender, EventMovedToRange e)
        {
            // 距离限制销毁子弹。
            ModuleEntity.Kill(Entity);
        }


    }
}