
using Frame.Common;
using Godot;

namespace Frame.Logic
{
    public class LogicMove2DTopDown : LogicMove2D
    {
        public override ValueType ValueType => ValueType.Move2DTopDown;

        protected override void Translate(Node2D entity, Vector2 translation)
        {
            if (entity is KinematicBody2D body2D)
            {
                body2D.MoveAndSlide(translation, Vector2.Up, true);
            }
        }

        protected override Vector2 GetTranslation(Node2D entity, ValueMove2D value, float delta)
        {
            return value.velocity * Constants.unitMeter;
        }
    }
}