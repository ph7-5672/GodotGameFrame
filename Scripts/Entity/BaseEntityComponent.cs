using Frame.Common;
using Godot;

namespace Frame.Entity
{
    public class BaseEntityComponent : Node, IEntityComponent
    {
        public Node2D Entity { get; private set; }

        public override void _Ready()
        {
            Entity = GetParent<Node2D>();
        }

    }
}