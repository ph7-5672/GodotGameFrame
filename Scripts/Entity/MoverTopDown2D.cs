using Frame.Common;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 俯视角2d移动。
    /// </summary>
    public class MoverTopDown2D : Mover2D
    {
        protected KinematicBody2D kinematicEntity => Entity as KinematicBody2D;
        
        protected override void Translate(float delta)
        {
            var translation = velocity * speed.final * Constants.unitMeter;
            kinematicEntity.MoveAndSlide(translation);
        }
    }
}