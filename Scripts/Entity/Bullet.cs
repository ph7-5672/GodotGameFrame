using Frame.Common;
using Frame.Module;
using Godot;
using Godot.Collections;

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

        protected bool isDying;

        /// <summary>
        /// 移动距离限制，小于0时不限制。
        /// </summary>
        public float movedRange => Entity.GetValue("movedRange");
        
        public float raycastWidth => Entity.GetValue("raycastWidth");

        /// <summary>
        /// 射线检测的层级。
        /// </summary>
        public uint raycastLayer => (uint) Entity.GetValue("raycastLayer");
        
        public override void Reset()
        {
            Entity.ClearPoints();
            isDying = false;
        }

        protected override void Init()
        {
            Entity.LoginBehaviorCondition<BehaviorTranslate>(CanTranslate);
            Entity.LoginBehaviorExecutor<BehaviorTranslate>(Translate);
        }

        protected virtual bool CanTranslate(Object entity, BehaviorTranslate behavior)
        {
            return !isDying;
        }

        protected virtual void Translate(Object entity, BehaviorTranslate behavior)
        {
            if (behavior.moved <= tailLength)
            {
                var point = -behavior.translation.Normalized() * behavior.moved;
                Entity.AddPoint(point);
            }

            Raycast(behavior.translation);
            MoveRange(behavior.moved);
        }

        void MoveRange(float moved)
        {
            if (movedRange > 0 && moved >= movedRange)
            {
                Die();
            }

            
        }


        protected virtual void Raycast(Vector2 translation)
        {
            if (raycastWidth <= 0 || Entity.IsKilled())
            {
                return;
            }
            
            var from = Entity.Position;
            var to = from - translation;
            var exclude = new Array(Entity);

            var world2D = Entity.GetWorld2d();
            var result = world2D.Raycast2D(from, to, exclude, raycastLayer);

            if (result == null || result.Count == 0)
            {
                var angle = Mathf.Deg2Rad(90f);
                var topTranslation = translation.Rotated(angle).Normalized() * raycastWidth / 2f;
                var topFrom = from + topTranslation;
                var topTo = to + topTranslation;
                result = world2D.Raycast2D(topFrom, topTo, exclude, raycastLayer);
            }

            if (result == null || result.Count == 0)
            {
                var angle = Mathf.Deg2Rad(-90f);
                var bottomTranslation = translation.Rotated(angle).Normalized() * raycastWidth / 2f;
                var bottomFrom = from + bottomTranslation;
                var bottomTo = to + bottomTranslation;
                result = world2D.Raycast2D(bottomFrom, bottomTo, exclude, raycastLayer);
            }

            if (result != null && result.Count > 0)
            {
                var collider = result["collider"];
                Die();
            }
            
        }
        
        protected virtual void Die()
        {
            isDying = true;
            Entity.Behave(new BehaviorMove(Vector2.Zero));
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