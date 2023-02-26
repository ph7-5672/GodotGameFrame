using Frame.Common;
using Godot;

namespace Frame.Entity
{
    public class Player : EntityComponentBase<Node2D>
    {

        public override void _Process(float delta)
        {
            ArrowInput();
            MouseInput();
        }

        private void ArrowInput()
        {
            var arrow = new Vector2
            {
                x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
                y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
            };

            Entity.Behave(new BehaviorMove(arrow));
        }


        private void MouseInput()
        {
            if (Input.IsActionPressed("fire"))
            {
                Entity.Behave(new BehaviorFire());
            }
        }

        
        /*private void ActionInput()
        {
            var ctrl = Input.IsKeyPressed((int)KeyList.Control);
            var shift = Input.IsKeyPressed((int)KeyList.Shift);
            var space = Input.IsKeyPressed((int)KeyList.)
        }*/
    }
}