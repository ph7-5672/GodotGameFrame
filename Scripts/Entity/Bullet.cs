using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 子弹组件，检测到其他实体时销毁。
    /// </summary>
    public class Bullet : EntityComponentBase<Node2D>
    {

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            ModuleEvent.Subscribe<EventMoverRaycast>(OnMoverRaycast, Entity);
            ModuleEvent.Subscribe<EventMovedToRange>(OnMovedToRange, Entity);
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