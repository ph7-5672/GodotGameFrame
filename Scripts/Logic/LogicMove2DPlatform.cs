using Frame.Common;
using Godot;

namespace Frame.Logic
{
    public class LogicMove2DPlatform : LogicBase<KinematicBody2D>
    {
        public override ValueType ValueType => ValueType.Move2DPlatform;

        protected override void Ready(KinematicBody2D entity)
        {
            entity.LoginBehaviorExecutor<BehaviorMove>(Move);
        }

        protected override void Dispose(KinematicBody2D entity)
        {
            entity.LogoutBehaviorExecutor<BehaviorMove>(Move);
        }
        
        protected override void PhysicsProcess(KinematicBody2D entity, float delta)
        {
            var move2DPlatform = entity.GetValue<ValueMove2DPlatform>();
            const float gravity = 9.8f;
            move2DPlatform.velocity.y += gravity * delta * Constants.unitMeter;
            
            move2DPlatform.isOnFloor = entity.IsOnFloor();
            if (move2DPlatform.isOnFloor)
            {
                move2DPlatform.velocity.y = Mathf.Min(0f, move2DPlatform.velocity.y);
                move2DPlatform.snap = Vector2.Down * gravity * Constants.unitMeter * delta;
            }
            
            var translation = new Vector2(move2DPlatform.velocity.x * move2DPlatform.speed.final, move2DPlatform.velocity.y);
            translation *= Constants.unitMeter;

            /*if (GameFrame.Timer.HasTimer($"{entity.GetInstanceId()}_stunned"))
            {
                return;
            }*/

            entity.MoveAndSlideWithSnap(translation, move2DPlatform.snap, Vector2.Up, true);

            
            /*var collision = entity.GetLastSlideCollision();
            // TODO 捋一下逻辑：
            // 水平方向碰到障碍会有一个短暂的晕眩。
            // 晕眩过程中无法响应操作（Buff系统）。
            // 可以加一个眩晕特效。
            // 
            if (collision?.Collider is TileMap tile && tile.CollisionLayer == 8)
            {
                //GD.Print("Success");
                var y = tile.CellSize.y;
                var direction = collision.Normal;
                entity.MoveAndSlide(direction * translation.Length() * 2f);
                GameFrame.Timer.StartNew(entity, 1.5f, $"{entity.GetInstanceId()}_stunned");
            }*/
        
            entity.SetValue(move2DPlatform);
        }
        
        private void Move(Node entity, BehaviorMove behavior)
        {
            var move2DPlatform = entity.GetValue<ValueMove2DPlatform>();
            // x轴移动。
            move2DPlatform.velocity.x = behavior.velocity.x;
            var arrowY = behavior.velocity.y;
            
            if (!move2DPlatform.isOnFloor || arrowY == 0)
            {
                entity.SetValue(move2DPlatform);
                return;
            }

            // y轴跳跃或下蹲。
            if (arrowY < 0)
            {
                move2DPlatform.velocity.y = -move2DPlatform.bounce.final * Constants.unitMeter;
                move2DPlatform.snap = Vector2.Zero;
            }
            else
            {
                // TODO 下蹲。
            }
            
            entity.SetValue(move2DPlatform);
            
        }
        
        
    }
}