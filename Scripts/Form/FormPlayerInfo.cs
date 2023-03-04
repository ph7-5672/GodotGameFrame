using Godot;

namespace Frame.Form
{
    public class FormPlayerInfo : Control
    {
        public Label hpLabel;

        public Label bulletLabel;

        public override void _Ready()
        {
            hpLabel = GetNode<Label>(nameof(hpLabel));
            bulletLabel = GetNode<Label>(nameof(bulletLabel));
        }

        public void Refresh(int bulletCount, int magazine)
        {
            bulletLabel.Text = $"Bullet:{bulletCount}/{magazine}";
        }
        
    }
}