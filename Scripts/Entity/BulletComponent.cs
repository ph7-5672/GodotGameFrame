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
        public override void _Ready()
        {
            base._Ready();
            // 连接信号。
            Entity.Connect("body_entered", this, nameof(OnBodyEntered));
        }

        protected override void Dispose(bool disposing)
        {
            Entity.Disconnect("body_entered", this, nameof(OnBodyEntered));
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