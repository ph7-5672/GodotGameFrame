using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Form
{
    public class FormPlayerInfo : Control
    {
        protected Node2D player;

        protected int bulletCount;

        protected int magazine;


        public Node2D Player
        {
            get => player;
            set
            {
                player = value;
                /*ModuleEvent.Subscribe<EventValueUpdate>((sender, e) =>
                {
                    if (nameof(bulletCount).Equals(e.name))
                    {
                        bulletCount = e.value.intFinal;
                        Refresh();
                    }
                    
                    if (nameof(magazine).Equals(e.name))
                    {
                        magazine = e.value.intFinal;
                        Refresh();
                    }
                    
                }, player);*/
                
            }
        }

       
        protected Label hpLabel;

        protected Label bulletLabel;


        public override void _Ready()
        {
            hpLabel = GetNode<Label>(nameof(hpLabel));
            bulletLabel = GetNode<Label>(nameof(bulletLabel));
        }

        protected virtual void Refresh()
        {
            bulletLabel.Text = $"Bullet:{bulletCount}/{magazine}";
        }




    }
}