using Frame.Common;
using Godot;

namespace Frame.Entity
{
    public class BaseEntityComponent : Node, IEntityComponent
    {
        protected Node2D entity;
        
        public override void _Ready()
        {
            entity = GetParent<Node2D>();
        }
    }
}