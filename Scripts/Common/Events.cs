using Godot;

namespace Frame.Common
{
    public readonly struct PlayerInputEvent : IEventArgs
    {
        public readonly Vector2 arrow;

        public PlayerInputEvent(Vector2 arrow)
        {
            this.arrow = arrow;
        }
    }
}