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

        protected Vector2 globalPosHistory;
        
        public override void Reset()
        {
            Entity.ClearPoints();
            globalPosHistory = Entity.GlobalPosition;
        }

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


        public override void _Process(float delta)
        {
            var pointCount = Entity.GetPointCount();
            for (var i = 0; i < pointCount; i++)
            {
                var position = Entity.GetPointPosition(i);
                Entity.SetPointPosition(i, position + globalPosHistory - Entity.GlobalPosition);
            }
            
            Entity.AddPoint(Vector2.Zero);
            if (pointCount > 200)
            {
                Entity.RemovePoint(0);
            }

            globalPosHistory = Entity.GlobalPosition;
            
            // TODO 限制两个点间最短距离。

        }
    }
}