using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    public class PlayerComponent : BaseEntityComponent
    {
        public override void _Process(float delta)
        {
            var arrow = new Vector2
            {
                x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
                y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
            };

            EventModule.Send(new ActionInputEvent(arrow), entity);
            
            var fire = Input.IsActionPressed("fire");
            EventModule.Send(new MouseInputEvent(fire), entity);
        }
    }
}