using System.Collections.Generic;
using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 一个shooter对应一把武器。
    /// </summary>
    public class Shooter : EntityComponentBase<KinematicBody2D>
    {
        /// <summary>
        /// 持有枪支。
        /// </summary>
        protected readonly List<GunsData> guns = new List<GunsData>();

        /// <summary>
        /// 当前使用的枪支。
        /// </summary>
        protected GunsData activeGun;
        
        /// <summary>
        /// 子弹生效对象层级。
        /// </summary>
        [Export(PropertyHint.Layers2dPhysics)]
        public uint shootLayer;
        
        /// <summary>
        /// 朝向。
        /// </summary>
        protected Vector2 orientation;
        
        /// <summary>
        /// 是否冷却中。
        /// </summary>
        protected bool isCooling;

        /// <summary>
        /// 剩余子弹数量。
        /// </summary>
        protected int bulletCount;
        
        /// <summary>
        /// 是否换弹中。
        /// </summary>
        protected bool isReloading;

        
        public override void Reset()
        {
            guns.Clear();
            activeGun = null;
            orientation = Vector2.Zero;
            isCooling = false;
            bulletCount = 0;
            
            // 测试
            guns.AddRange(ModuleDatabase.Load<GunsData>(DatabaseType.Guns));
            activeGun = guns[0];
            bulletCount = activeGun.clipSize.intFinal;
        }


        protected override void SubscribeEvents()
        {
            ModuleEvent.Subscribe<EventMouseInput>(OnMouseInput, Entity);
            ModuleEvent.Subscribe<EventTimeout>(OnTimeout, Entity);
        }

        protected virtual void OnTimeout(object sender, EventTimeout e)
        {
            if (activeGun == null)
            {
                return;
            }

            if ($"{activeGun.name}_cooling".Equals(e.timerName))
            {
                isCooling = false;
            }
            
            if ($"{activeGun.name}_reload".Equals(e.timerName))
            {
                isReloading = false;
                bulletCount = activeGun.clipSize.intFinal;
            }

        }

        protected virtual void OnMouseInput(object sender, EventMouseInput e)
        {
            if (!e.fire[1] || activeGun == null)
            {
                return;
            }

            if (bulletCount > 0)
            {
                Shoot();
            }
            else
            {
                Reload();
            }
        }
        

        protected virtual void Shoot()
        {
            if (isCooling)
            {
                return;
            }

            isCooling = true;
            ModuleTimer.StartNew(Entity, activeGun.interval.final, $"{activeGun.name}_cooling");
            
            // 根据鼠标位置获取方向。
            var globalMousePosition = Entity.GetGlobalMousePosition();
            var direction = globalMousePosition - Entity.GlobalPosition;
            if (direction != Vector2.Zero)
            {
                orientation = direction.Normalized();
            }
            
            // 生成子弹实体，并控制它的方向和速度。
            var bullet = ModuleEntity.Spawn(EntityType.Bullet, Entity.GlobalPosition);
            bullet.SendEvent(new EventArrowInput(orientation));
            bullet.SendEvent(new EventValueUpdate("movedRange", activeGun.range));
            bullet.SendEvent(new EventValueUpdate("speed", activeGun.bulletSpeed));
            bullet.SendEvent(new EventValueUpdate("raycastLayer", new Value(shootLayer)));
            bullet.SendEvent(new EventValueUpdate("bulletWidth", new Value(activeGun.caliber)));

            --bulletCount;
        }


        protected virtual void Reload()
        {
            if (isReloading)
            {
                return;
            }
            isReloading = true;
            ModuleTimer.StartNew(Entity, activeGun.reloadTime.final, $"{activeGun.name}_reload");
        }




    }
}