using Godot;

namespace Frame.Common
{

    public readonly struct BehaviorMove : IEntityBehavior
    {
        public readonly Vector2 velocity;

        public BehaviorMove(Vector2 velocity)
        {
            this.velocity = velocity;
        }


        public BehaviorType Type => BehaviorType.Move;
    }

    public readonly struct BehaviorTranslate : IEntityBehavior
    {
        public readonly Vector2 translation;

        public BehaviorTranslate(Vector2 translation)
        {
            this.translation = translation;
        }
        
        public BehaviorType Type => BehaviorType.Translate;
        
    }
    


    public readonly struct BehaviorShoot : IEntityBehavior
    {
        /// <summary>
        /// 枪口位置。
        /// </summary>
        public readonly Vector2 muzzlePosition;
        /// <summary>
        /// 射击位置，直线射击的方向。
        /// </summary>
        public readonly Vector2 targetPosition;
        /// <summary>
        /// 消耗子弹数。
        /// </summary>
        public readonly int bulletConsume;
        /// <summary>
        /// 发射子弹数。
        /// </summary>
        public readonly int bulletShoot;

        public BehaviorShoot(Vector2 muzzlePosition, Vector2 targetPosition, int bulletConsume = 1, int bulletShoot = 1)
        {
            this.muzzlePosition = muzzlePosition;
            this.targetPosition = targetPosition;
            this.bulletConsume = bulletConsume;
            this.bulletShoot = bulletShoot;
        }
        
        public BehaviorType Type => BehaviorType.Shoot;
    }
    
    public readonly struct BehaviorReload : IEntityBehavior
    {
        public BehaviorType Type => BehaviorType.Reload;
    }

    public readonly struct BehaviorChangeGun : IEntityBehavior
    {
        public readonly int gunId;
        public BehaviorChangeGun(int gunId)
        {
            this.gunId = gunId;
        }
        public BehaviorType Type => BehaviorType.ChangeGun;
    }
    


    public readonly struct BehaviorDamage : IEntityBehavior
    {
        public readonly Node target;
        public readonly float value;

        public BehaviorDamage(Node target, float value)
        {
            this.target = target;
            this.value = value;
        }

        public BehaviorType Type => BehaviorType.Damage;
    }

}