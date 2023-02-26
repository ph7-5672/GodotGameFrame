using Godot;

namespace Frame.Common
{


    public readonly struct BehaviorMove
    {
        public readonly Vector2 velocity;

        public BehaviorMove(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }



    public readonly struct BehaviorFire
    {
    }


    public readonly struct BehaviorTranslate
    {
        public readonly Vector2 translation;

        public readonly float moved;

        public BehaviorTranslate(Vector2 translation, float moved)
        {
            this.translation = translation;
            this.moved = moved;
        }
    }

}