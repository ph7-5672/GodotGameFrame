using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 一个shooter对应一把武器。
    /// </summary>
    /*public class Shooter : EntityComponentBase<KinematicBody2D>
    {
        /#1#// <summary>
        /// 持有枪支。
        /// </summary>
        protected readonly List<GunsData> guns = new List<GunsData>();

        /// <summary>
        /// 当前使用的枪支。
        /// </summary>
        protected GunsData activeGun;#1#

        /// <summary>
        /// 朝向。
        /// </summary>
        protected Vector2 orientation;
        
        /// <summary>
        /// 是否冷却中。
        /// </summary>
        protected bool isCooling;

        /// <summary>
        /// 是否换弹中。
        /// </summary>
        protected bool isReloading;


        protected int activeGunId;

        protected string activeGunName;

        /// <summary>
        /// 子弹生效对象层级。
        /// </summary>
        protected uint shootLayer => (uint) Entity.GetValue("shootLayer");
        /// <summary>
        /// 弹匣容量。
        /// </summary>
        protected int magazine => Entity.GetIntValue("magazine");
        /// <summary>
        /// 剩余子弹数量。
        /// </summary>
        protected int bulletCount => Entity.GetIntValue("bulletCount");
        /// <summary>
        /// 扩散角度。
        /// </summary>
        protected float shootSpread => Entity.GetValue("shootSpread");
        /// <summary>
        /// 射击间隔。
        /// </summary>
        protected float shootInterval => Entity.GetValue("shootInterval");
        /// <summary>
        /// 射程。
        /// </summary>
        protected float shootRange => Entity.GetValue("shootRange");
        /// <summary>
        /// 子弹速度。
        /// </summary>
        protected float bulletSpeed => Entity.GetValue("bulletSpeed");
        /// <summary>
        /// 子弹口径。
        /// </summary>
        protected float bulletSpec => Entity.GetValue("bulletSpec");
        /// <summary>
        /// 换弹时间。
        /// </summary>
        protected float reloadTime => Entity.GetValue("reloadTime");
        
        
        
        
        public override void Reset()
        {
            orientation = Vector2.Zero;
            isCooling = false;
            activeGunId = -1;
        }
        
        protected override void Init()
        {
            Entity.LoginBehaviorCondition<BehaviorShoot>(CanFire);
            Entity.LoginBehaviorExecutor<BehaviorShoot>(Fire);
            Entity.LoginBehaviorExecutor<BehaviorChangeGun>(ChangeGun);
        }

        protected virtual void ChangeGun(Node entity, BehaviorChangeGun behavior)
        {
            activeGunId = behavior.gunId;
            var activeGunInfo = ModuleDatabase.GetData(DatatableType.Guns, activeGunId);
            activeGunName = activeGunInfo["name"];
            foreach (var pair in activeGunInfo)
            {
                if (float.TryParse(pair.Value, out var value))
                {
                    Entity.SetValue(pair.Key, value);
                }
            }
            ReloadFinish();
        }

        private void ReloadFinish()
        {
            isReloading = false;
            Entity.SetValue("bulletCount", magazine);
        }


        private bool CanFire(Node entity, BehaviorShoot behavior)
        {
            return !isCooling && activeGunId >= 0;
        }

        [Common.Event(EventType.Timeout)]
        public static void OnTimeout(object owner, string timerName, bool isRepeat)
        {
            if (owner is Shooter shooter)
            {
                shooter.OnTimeout(timerName);
            }
        }

        protected virtual void OnTimeout(string timerName)
        {
            if (activeGunId < 0)
            {
                return;
            }

            if ($"{activeGunName}_cooling".Equals(timerName))
            {
                isCooling = false;
            }
            
            if ($"{activeGunName}_reload".Equals(timerName))
            {
                ReloadFinish();
            }

        }

        protected virtual void Fire(Node entity, BehaviorShoot behavior)
        {
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
            isCooling = true;
            ModuleTimer.StartNew(this, shootInterval, $"{activeGunName}_cooling");
            
            // 根据鼠标位置获取方向。
            var globalMousePosition = Entity.GetGlobalMousePosition();
            var direction = globalMousePosition - Entity.GlobalPosition;
            if (direction != Vector2.Zero)
            {
                orientation = direction.Normalized();
                // 随机扩散。
                var spread = shootSpread;
                var angle = UtilityRandom.NextFloat(-spread, spread, 3);
                
                var rad = Mathf.Deg2Rad(angle);
                orientation = orientation.Rotated(rad);
            }
            
            // 生成子弹实体，并控制它的方向和速度。
            var bullet = ModuleEntity.Spawn(EntityType.Bullet, Entity.GlobalPosition);
            bullet.SetValue("movedRange", shootRange);
            bullet.SetValue("speed", bulletSpeed);
            bullet.SetValue("raycastLayer", shootLayer);
            bullet.SetValue("raycastWidth", bulletSpec);

            bullet.Behave(new BehaviorMove(orientation));
            Entity.SetValue("bulletCount", bulletCount - 1);
        }


        protected virtual void Reload()
        {
            if (isReloading)
            {
                return;
            }
            isReloading = true;
            ModuleTimer.StartNew(this, reloadTime, $"{activeGunName}_reload");
        }




    }*/
}