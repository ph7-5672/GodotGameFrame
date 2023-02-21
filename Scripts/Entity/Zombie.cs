using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    public class Zombie : EntityComponentBase<KinematicBody2D>
    {
        

        public override void _Process(float delta)
        {
            ModuleEvent.Send(new EventArrowInput(new Vector2(1f, 0f)), Entity);
        }
    }
}