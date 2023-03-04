using Frame.Common;
using Frame.Module;
using Godot;
using Godot.Collections;

namespace Frame.Logic
{
    public class LogicBullet : LogicBase<Line2D>
    {
        protected override ValueType ValueType => ValueType.Bullet;

        protected override void Ready(Line2D entity)
        {
            entity.LoginBehaviorExecutor<BehaviorTranslate>(Translate);
        }

        protected override void Dispose(Line2D entity)
        {
            entity.LogoutBehaviorExecutor<BehaviorTranslate>(Translate);
        }

        private void Translate(Node entity, BehaviorTranslate behavior)
        {
            if (entity.IsKilled())
            {
                return;
            }

            if (!(entity is Line2D line2D))
            {
                return;
            }

            var translation = behavior.translation;
            var pointCount = line2D.GetPointCount();

            // 子弹渐变消失。
            if (!LogicHealth.IsAlive(line2D))
            {
                if (pointCount > 0)
                {
                    line2D.RemovePoint(0);
                }
                else
                {
                    ModuleEntity.Kill(line2D);
                }
                return;
            }
            
            // 子弹拖尾 begin
            var bullet = GetValue<ValueBullet>(entity);
            var lastPosition = Vector2.Zero;
                
            if (pointCount > 0)
            {
                lastPosition = line2D.GetPointPosition(pointCount - 1);
            }

            var points = line2D.Points;
            var length = 0f;
            for (var i = 1; i < points.Length; ++i)
            {
                var gap = points[i] - points[i - 1];
                length += gap.Length();
            }
            
            if (length < bullet.tail)
            {
                var point = lastPosition + translation;
                line2D.AddPoint(point);
            }
            // 子弹拖尾 end
            
            // 移动距离限制。
            line2D.Behave(new BehaviorDamage(line2D, translation.Length()));
            
            // 击中判定。
            var width = line2D.Width;
            var layer = bullet.layer;
            var from = line2D.Position;
            var to = from + translation.Normalized() * length;
            var exclude = new Array(line2D);

            var world2D = line2D.GetWorld2d();
            var result = world2D.Raycast2D(from, to, exclude, layer);

            if (result == null || result.Count == 0)
            {
                var angle = Mathf.Deg2Rad(90f);
                var topTranslation = translation.Rotated(angle).Normalized() * width / 2f;
                var topFrom = from + topTranslation;
                var topTo = to + topTranslation;
                result = world2D.Raycast2D(topFrom, topTo, exclude, layer);
            }

            if (result == null || result.Count == 0)
            {
                var angle = Mathf.Deg2Rad(-90f);
                var bottomTranslation = translation.Rotated(angle).Normalized() * width / 2f;
                var bottomFrom = from + bottomTranslation;
                var bottomTo = to + bottomTranslation;
                result = world2D.Raycast2D(bottomFrom, bottomTo, exclude, layer);
            }

            if (result != null && result.Count > 0)
            {
                var collider = result["collider"];
                var e = (Node2D) collider;
                // 干掉子弹。
                line2D.Behave(new BehaviorDamage(entity, 9999f));
            }
        }
        
        
    }
}