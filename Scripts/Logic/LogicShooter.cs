using System.Diagnostics;
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
            entity.LoginBehaviorCondition<BehaviorReload>(CanReload);
            entity.LoginBehaviorExecutor<BehaviorShoot>(Shoot);
            entity.LoginBehaviorExecutor<BehaviorReload>(Reload);
        }


        protected override void Dispose(KinematicBody2D entity)
        {
            entity.LogoutBehaviorCondition<BehaviorShoot>(CanShoot);
            entity.LogoutBehaviorCondition<BehaviorReload>(CanReload);
            entity.LogoutBehaviorExecutor<BehaviorShoot>(Shoot);
            entity.LogoutBehaviorExecutor<BehaviorReload>(Reload);
        }


        private bool CanShoot(Node entity, BehaviorShoot behavior)
        {
            var shooter = entity.GetValue<ValueShooter>();
            return !IsCooling(shooter) && !IsReloading(shooter);
        }

        private bool CanReload(Node entity, BehaviorReload behavior)
        {
            var shooter = entity.GetValue<ValueShooter>();
            return shooter.bulletCount < shooter.magazine.intFinal && !IsCooling(shooter) && !IsReloading(shooter);
        }

        protected virtual void Shoot(Node entity, BehaviorShoot behavior)
        {
            var shooter = entity.GetValue<ValueShooter>();

            if (shooter.bulletCount > 0)
            {
                GameFrame.Timer.StartNew(entity, shooter.interval.final, $"{shooter.name}_cooling");
                for (var i = 0; i < behavior.bulletShoot; ++i)
                {
                    var trans = behavior.targetPosition - behavior.muzzlePosition;
                    var direction = trans.Normalized();
                    // 随机扩散。
                    if (direction != Vector2.Zero)
                    {
                        var spread = shooter.spread.final;
                        var angle = UtilityRandom.NextFloat(-spread, spread, 3);

                        var rad = Mathf.Deg2Rad(angle);
                        direction = direction.Rotated(rad);
                    }

                    // 生成子弹实体，并控制它的方向和速度。
                    if (GameFrame.Entity.Spawn(EntityType.Bullet) is Line2D bullet)
                    {
                        bullet.Position = behavior.muzzlePosition;
                        var move2D = new ValueMove2D(ProcessMode.Physics)
                        {
                            velocity = direction * shooter.bulletSpeed.final
                        };
                        bullet.SetValue(move2D);
                        
                        bullet.SetValue(new ValueBullet(20f, shooter.shootLayer));
                        var range = shooter.range.final * Constants.unitMeter;
                        bullet.SetValue(new ValueHealth(range, new Value(range)));
                        bullet.Width = shooter.caliber.final * Constants.unitMeter / 100f;
                    }
                }
                shooter.bulletCount -= behavior.bulletConsume;
            }
            else
            {
                entity.Behave(new BehaviorReload());
            }

            entity.SetValue(shooter);
        }


        private void Reload(Node entity, BehaviorReload behavior)
        {
            var shooter = entity.GetValue<ValueShooter>();
            GameFrame.Timer.StartNew(entity, shooter.reloadTime.final, $"{shooter.name}_reloading");
        }

        public static bool IsReloading(ValueShooter shooter) => GameFrame.Timer.HasTimer($"{shooter.name}_reloading");

        public static bool IsCooling(ValueShooter shooter) => GameFrame.Timer.HasTimer($"{shooter.name}_cooling");


        [Event(EventType.Timeout)]
        public static void OnTimeout(object owner, string timerName, bool isRepeat)
        {
            if (!(owner is KinematicBody2D entity))
            {
                return;
            }

            var shooter = entity.GetValue<ValueShooter>();
            if ($"{shooter.name}_reloading".Equals(timerName))
            {
                shooter.bulletCount = shooter.magazine.intFinal;
            }
            entity.SetValue(shooter);
        }
    }
}