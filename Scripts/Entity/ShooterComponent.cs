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
            EventModule.Subscribe<MouseInputEvent>(OnMouseInput, Entity);
        }

        private void OnMouseInput(object sender, MouseInputEvent e)
        {
            if (e.fire)
            {
                var globalMousePosition = Entity.GetGlobalMousePosition();
                var direction = globalMousePosition - Entity.Position;
                if (direction != Vector2.Zero)
                {
                    orientation = direction.Normalized();
                }
                
                var bullet = EntityModule.Spawn(EntityType.Bullet);
                // 初始化子弹位置。
                bullet.Position = Entity.Position;
                EventModule.Send(new ActionInputEvent(orientation), bullet);
            }
        }
        
        
    }
}