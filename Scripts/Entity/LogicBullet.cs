using Frame.Common;
using Frame.Module;
using Godot;
using Godot.Collections;

namespace Frame.Entity
{
    /// <summary>
    /// 子弹组件，检测到其他实体时销毁。
    /// </summary>
    /*public struct LogicBullet : IEntityLogic
    {
        /// <summary>
        /// 拖尾长度。
        /// </summary>
        public const float tailLength = 50f;

        private bool isDying;

        /// <summary>
        /// 移动距离限制，小于0时不限制。
        /// </summary>
        public float movedRange => Entity.GetValue("movedRange");
        
        public float raycastWidth => Entity.GetValue("raycastWidth");

        /// <summary>
        /// 射线检测的层级。
        /// </summary>
        public uint raycastLayer => (uint) Entity.GetValue("raycastLayer");

        public Node Entity { get; set; }

        private Line2D line2D;

        public void Ready()
        {
            line2D = Entity as Line2D;

            Entity.LoginBehaviorCondition<BehaviorTranslate>(CanTranslate);
            Entity.LoginBehaviorExecutor<BehaviorTranslate>(Translate);
        }

        public void Process(float delta)
        {
            if (!isDying)
            {
                return;
            }

            var pointCount = line2D.GetPointCount();
            if (pointCount > 0)
            {
                line2D.RemovePoint(pointCount - 1);
            }
            else
            {
                ModuleEntity.Kill(Entity);
            }
        }

        public void PhysicsProcess(float delta)
        {
        }

        public void Dispose()
        {
            line2D.ClearPoints();
            
            Entity.LogoutBehaviorCondition<BehaviorTranslate>(CanTranslate);
            Entity.LogoutBehaviorExecutor<BehaviorTranslate>(Translate);
        }

        private bool CanTranslate(Node entity, BehaviorTranslate behavior)
        {
            return !isDying;
        }

        private void Translate(Node entity, BehaviorTranslate behavior)
        {
            if (behavior.moved <= tailLength)
            {
                var point = -behavior.translation.Normalized() * behavior.moved;
                line2D.AddPoint(point);
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


        private void Raycast(Vector2 translation)
        {
            if (raycastWidth <= 0 || Entity.IsKilled())
            {
                return;
            }
            
            var from = line2D.Position;
            var to = from - translation;
            var exclude = new Array(Entity);

            var world2D = line2D.GetWorld2d();
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
                Die();
                var collider = result["collider"];
                var entity = (Node2D) collider;
                if (Entity.IsEntity())
                {
                    ModuleEntity.Kill(entity);
                }

            }
            
        }
        
        private void Die()
        {
            isDying = true;
            Entity.Behave(new BehaviorMove(Vector2.Zero));
        }


        
    }*/
}