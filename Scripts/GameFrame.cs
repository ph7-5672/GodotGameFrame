using Godot;

namespace Frame
{
    public class GameFrame : Node
    {
        /// <summary>
        /// GUI根节点。
        /// </summary>
        public static CanvasLayer FormRoot { get; private set; }

        public static Node EntityRoot { get; private set; }

        public override void _Ready()
        {
            FormRoot = GetNode<CanvasLayer>(nameof(FormRoot));
            EntityRoot = GetNode<Node>(nameof(EntityRoot));
        }
    }
}


