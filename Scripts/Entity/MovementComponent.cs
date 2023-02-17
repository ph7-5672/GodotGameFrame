using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 移动组件。
    /// </summary>
    public class MovementComponent : Node, IEntityComponent
    {
        
        
        public override void _Ready()
        {
            EventModule.Subscribe<PlayerInputEvent>(OnPlayerInput, GetParent());
        }

        void OnPlayerInput(object sender, PlayerInputEvent eventArgs)
        {
            GD.Print($"输入:{eventArgs.arrow}");
        }
    }
}