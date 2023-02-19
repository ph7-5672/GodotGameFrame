using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    public class ShooterComponent : BaseEntityComponent
    {
        /// <summary>
        /// 朝向。
        /// </summary>
        private Vector2 orientation;

        public override void _Ready()
        {
            base._Ready();
            EventModule.Subscribe<MouseInputEvent>(OnMouseInput, entity);
        }

        private void OnMouseInput(object sender, MouseInputEvent e)
        {
            if (e.fire)
            {
                var globalMousePosition = entity.GetGlobalMousePosition();
                var direction = globalMousePosition - entity.Position;
                GD.Print(globalMousePosition);
                if (direction != Vector2.Zero)
                {
                    orientation = direction.Normalized();
                }
                
                var bullet = EntityModule.Spawn(EntityType.Bullet);
                // 初始化子弹位置。
                bullet.Position = entity.Position;
                EventModule.Send(new ActionInputEvent(orientation), bullet);
            }
        }
        
        
    }
}