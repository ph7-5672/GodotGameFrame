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

            Entity.SendEvent(new EventArrowInput(arrow));
        }


        private void MouseInput()
        {
            var fire = new bool[5];
            for (var i = 0; i < fire.Length; i++)
            {
                fire[i] = Input.IsMouseButtonPressed(i);
            }
            
            Entity.SendEvent(new EventMouseInput(fire));
        }

        
        /*private void ActionInput()
        {
            var ctrl = Input.IsKeyPressed((int)KeyList.Control);
            var shift = Input.IsKeyPressed((int)KeyList.Shift);
            var space = Input.IsKeyPressed((int)KeyList.)
        }*/
    }
}