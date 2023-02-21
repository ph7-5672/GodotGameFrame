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
        protected float range;

        public override void Reset()
        {
            base.Reset();
            range = 0f;
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            ModuleEvent.Subscribe<EventValueUpdate>(OnValueUpdate, Entity);
            ModuleEvent.Subscribe<EventMoverRaycast>(OnMoverRaycast, Entity);
        }

        protected virtual void OnValueUpdate(object sender, EventValueUpdate e)
        {
            if (nameof(range).Equals(e.name))
            {
                range = e.value.final;
            }

            if ("distanceMoved".Equals(e.name))
            {
                // 移动距离超过射程后销毁子弹。
                if (e.value.final >= range)
                {
                    ModuleEntity.Kill(Entity);
                }
            }
        }

        protected void OnMoverRaycast(object sender, EventMoverRaycast e)
        {
            var collider = e.results["collider"];
            // TODO 根据阵营判断。
            if (collider is Node2D entity)
            {
                var zombie = entity.GetComponent<Zombie>();
                if (zombie != null)
                {
                    ModuleEntity.Kill(Entity);
                    ModuleEntity.Kill(zombie.Entity);
                }
            }
        }

        
        /*
        private void OnDistanceChange(object sender, DistanceChangeEvent e)
        {
            // 移动距离超过射程后销毁子弹。
            if (e.current >= range)
            {
                ModuleEntity.Kill(Entity);
            }
        }*/

        public override void _PhysicsProcess(float delta)
        {
            // 射线检测当前到下个点的
            
            /*var bodies = Entity.GetOverlappingBodies();
            foreach (Node2D body in bodies)
            {
                var zombie = body?.GetComponent<Zombie>();
                if (zombie != null)
                {
                    ModuleEntity.Kill(Entity);
                    ModuleEntity.Kill(zombie.Entity);
                }
            }*/
        }
        
    }
}