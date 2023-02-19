using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    public class ZombieComponent : BaseEntityComponent
    {
        public override void _Process(float delta)
        {
            EventModule.Send(new ActionInputEvent(new Vector2(1f, 0f)), entity);
        }
    }
}