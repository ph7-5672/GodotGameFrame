using Frame.Common;
using Godot;

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
            entity.LoginBehaviorExecutor<BehaviorTranslate>(Translate);
        }

        private void Translate(Node entity, BehaviorTranslate behavior)
        {
            if (!(entity is Line2D line2D))
            {
                return;
            }
            
            var value = GetValue<ValueBullet>(entity);

            var lastPosition = Vector2.Zero;
                
            var pointCount = line2D.GetPointCount();
            if (pointCount > 0)
            {
                lastPosition = line2D.GetPointPosition(pointCount - 1);
            }

            var points = line2D.Points;
            var length = 0f;
            for (var i = 1; i < points.Length; i++)
            {
                length += points[i].Length() - points[i - 1].Length();
            }
            
            if (length < value.tail)
            {
                var point = lastPosition - behavior.translation;
                line2D.AddPoint(point);
            }
            
        }
    }
}