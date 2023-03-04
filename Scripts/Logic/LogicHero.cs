using Frame.Common;
using Godot;

namespace Frame.Logic
{
    /// <summary>
    /// 英雄逻辑，响应玩家操作。
    /// </summary>
    public class LogicHero : LogicBase<Node2D>
    {
        protected override ValueType ValueType => ValueType.Hero;

        protected override void Process(Node2D entity, float delta)
        {
            ArrowInput(entity);
            ActionInput(entity);
            MouseInput(entity);
        }
        
        private void ArrowInput(Node2D entity)
        {
            var arrow = new Vector2
            {
                x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
                y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
            };

            entity.Behave(new BehaviorMove(arrow));
        }

        private void ActionInput(Node2D entity)
        {
            if (Input.IsActionPressed("reload"))
            {
                entity.Behave(new BehaviorReload());
            }
        }

        private void MouseInput(Node2D entity)
        {
            if (Input.IsActionPressed("fire"))
            {
                var muzzlePosition = entity.GlobalPosition;
                var targetPosition = entity.GetGlobalMousePosition();
                entity.Behave(new BehaviorShoot(muzzlePosition, targetPosition, 1, 1));
            }
        }

       
    }
}