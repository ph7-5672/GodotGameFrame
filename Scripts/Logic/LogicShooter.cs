using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Logic
{
    public class LogicShooter : LogicBase<KinematicBody2D>
    {
        protected override ValueType ValueType => ValueType.Shooter;

        protected override void Ready(KinematicBody2D entity)
        {
            entity.LoginBehaviorCondition<BehaviorShoot>(CanShoot);
            entity.LoginBehaviorExecutor<BehaviorShoot>(Shoot);
        }
        
        
        protected override void Dispose(KinematicBody2D entity)
        {
            entity.LogoutBehaviorCondition<BehaviorShoot>(CanShoot);
            entity.LogoutBehaviorExecutor<BehaviorShoot>(Shoot);
        }
        
        
        private bool CanShoot(Node entity, BehaviorShoot behavior)
        {
            if (entity.TryGetValue(ValueType.Shooter, out ValueShooter value))
            {
                return !value.isCooling;
            }

            return true;
        }
        
        protected virtual void Shoot(Node entity, BehaviorShoot behavior)
        {
            if (!entity.TryGetValue(ValueType.Shooter, out ValueShooter value))
            {
                return;
            }

            if (value.bulletCount > 0)
            {
                var trans = behavior.targetPosition - behavior.muzzlePosition;
                var direction = trans.Normalized();
                
                if (direction != Vector2.Zero)
                {
                    // 随机扩散。
                    var spread = value.spread.final;
                    var angle = UtilityRandom.NextFloat(-spread, spread, 3);
                
                    var rad = Mathf.Deg2Rad(angle);
                    direction = direction.Rotated(rad);
                }
                value.isCooling = true;
                ModuleTimer.StartNew(entity, value.interval.final, $"{value.name}_cooling");
            
                // 生成子弹实体，并控制它的方向和速度。
                if (ModuleEntity.Spawn(EntityType.Bullet) is Node2D bullet)
                {
                    bullet.Position = behavior.muzzlePosition;
                    var move2D = new ValueMove2D(ProcessMode.Idle)
                    {
                        velocity = direction * value.bulletSpeed.final
                    };
                    bullet.SetValue(ValueType.Move2D, move2D);
                    bullet.SetValue(ValueType.Bullet, new ValueBullet(40f));
                }
                //value.bulletCount -= behavior.bulletConsume;
            }
            else
            {
               // Reload(ref value);
            }
            entity.SetValue(ValueType.Shooter, value);
        }

        
        [Event(EventType.Timeout)]
        public static void OnTimeout(object owner, string timerName, bool isRepeat)
        {
            if (!(owner is KinematicBody2D entity))
            {
                //shooter.OnTimeout(timerName);
                return;
            }

            if (!entity.TryGetValue(ValueType.Shooter, out ValueShooter value))
            {
                return;
            }
            
            if ($"{value.name}_cooling".Equals(timerName))
            {
                value.isCooling = false;
            }
            
            entity.SetValue(ValueType.Shooter, value);
            /*if ($"{value.name}_reload".Equals(timerName))
            {
                ReloadFinish();
            }*/
            
        }
        
        
        protected virtual void OnTimeout(string timerName, ValueShooter value)
        {
            

        }
        

        /*protected virtual void Reload(ref ValueShooter value)
        {
            if (isReloading)
            {
                return;
            }
            isReloading = true;
            ModuleTimer.StartNew(this, reloadTime, $"{activeGunName}_reload");
        }
        

        [Event(EventType.Timeout)]
        public static void OnTimeout(object owner, string timerName, bool isRepeat)
        {
            if (owner is Shooter shooter)
            {
                shooter.OnTimeout(timerName);
            }
        }
        
        
        protected virtual void OnTimeout(string timerName, ValueShooter value)
        {
            if ($"{value.name}_cooling".Equals(timerName))
            {
                isCooling = false;
            }
            
            if ($"{value.name}_reload".Equals(timerName))
            {
                ReloadFinish();
            }

        }*/


        
    }
}