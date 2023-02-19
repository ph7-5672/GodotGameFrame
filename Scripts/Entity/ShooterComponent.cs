using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    public class ShooterComponent : BaseEntityComponent
    {
        /// <summary>
        /// 默认射击范围。
        /// </summary>
        [Export]
        public float defaultRange;
        
        /// <summary>
        /// 射击范围。
        /// </summary>
        private Value range;


        public Value Range
        {
            get => range;
            set => Entity.SetValue(nameof(range), ref range, value);
        }

        /// <summary>
        /// 朝向。
        /// </summary>
        private Vector2 orientation;

        public override void _Ready()
        {
            base._Ready();
            EventModule.Subscribe<MouseInputEvent>(OnMouseInput, Entity);

            Range = new Value(defaultRange);
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
                
                var bullet = EntityModule.Spawn(EntityType.Bullet, Entity.Position);
                var bulletComponent = bullet.GetComponent<BulletComponent>();
                bulletComponent.range = range.final;
                EventModule.Send(new ActionInputEvent(orientation), bullet);
            }
        }
        
        
    }
}