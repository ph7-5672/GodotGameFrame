using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 子弹组件，检测到其他实体时销毁。
    /// </summary>
    public class BulletComponent : BaseEntityComponent
    {
        public float range;

        private Area2D area;
        
        public override void _Ready()
        {
            base._Ready();
            
            EventModule.Subscribe<DistanceChangeEvent>(OnDistanceChange, Entity);

            area = Entity as Area2D;
        }

        private void OnDistanceChange(object sender, DistanceChangeEvent e)
        {
            // 移动距离超过射程后销毁子弹。
            if (e.current >= range)
            {
                EntityModule.Kill(Entity);
            }
        }

        public override void _Process(float delta)
        {
            var bodies = area.GetOverlappingBodies();
            foreach (Node2D body in bodies)
            {
                var zombie = body?.GetComponent<ZombieComponent>();
                if (zombie != null)
                {
                    EntityModule.Kill(Entity);
                    EntityModule.Kill(zombie.Entity);
                }
            }
        }

        void OnBodyEntered(Node body)
        {
            if (body is Node2D node2D)
            {
                var zombie = node2D.GetComponent<ZombieComponent>();
                if (zombie != null)
                {
                    EntityModule.Kill(Entity);
                    EntityModule.Kill(zombie.Entity);
                }
            }
        }
        
        
    }
}