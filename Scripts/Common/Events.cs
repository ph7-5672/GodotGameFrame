using Godot;

namespace Frame.Common
{

    public readonly struct ActionInputEvent : IEventArgs
    {
        public readonly Vector2 arrow;

        public ActionInputEvent(Vector2 arrow)
        {
            this.arrow = arrow;
        }
    }

    public readonly struct MouseInputEvent : IEventArgs
    {
        public readonly bool fire;

        public MouseInputEvent(bool fire)
        {
            this.fire = fire;
        }
    }

    public readonly struct ValueChangeEvent : IEventArgs
    {
        public readonly string valueName;
        
        public readonly Value origin;

        public readonly Value current;

        public ValueChangeEvent(string valueName, Value origin, Value current)
        {
            this.valueName = valueName;
            this.origin = origin;
            this.current = current;
        }
    }



    public readonly struct DistanceChangeEvent : IEventArgs
    {
        public readonly float origin;

        public readonly float current;

        public DistanceChangeEvent(float origin, float current)
        {
            this.origin = origin;
            this.current = current;
        }
    }

}