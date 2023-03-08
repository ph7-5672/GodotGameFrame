using Frame.Common;
using Godot;

namespace Frame.Logic
{
    public class LogicStunByObstacle : LogicBase<KinematicBody2D>
    {
        public override ValueType ValueType => ValueType.StunByObstacle;

        protected override void Ready(KinematicBody2D entity)
        {
            
            entity.LoginBehaviorExecutor<BehaviorTranslate>(Translate);
        }

        protected override void Dispose(KinematicBody2D entity)
        {
            entity.LogoutBehaviorExecutor<BehaviorTranslate>(Translate);
        }
        
        private void Translate(Node entity, BehaviorTranslate behavior)
        {
            if (!(entity is KinematicBody2D body2D))
            {
                return;
            }

            var collision = body2D.GetLastSlideCollision();
            if (collision?.Collider is TileMap tile && tile.CollisionLayer == 8)
            {
                var y = tile.CellSize.y;
                var direction = collision.Normal;
                body2D.MoveAndSlide(direction * behavior.translation.Length() * 2f);
                GameFrame.Timer.StartNew(entity, 1.5f, $"{entity.GetInstanceId()}_stunned");
            }
        }
        
    }
}