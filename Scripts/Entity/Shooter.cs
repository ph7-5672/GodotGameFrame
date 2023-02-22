using Frame.Common;
using Frame.Module;
using Godot;
using Timer = System.Timers.Timer;

namespace Frame.Entity
{
    /// <summary>
    /// 一个shooter对应一把武器。
    /// </summary>
    public class Shooter : EntityComponentBase<KinematicBody2D>
    {
        [Export]
        public float defaultInterval;
        
        [Export]
        public float defaultRange;

        [Export]
        public float defaultClipSize;

        [Export]
        public float defaultReloadTime;

        /// <summary>
        /// 子弹生效对象层级。
        /// </summary>
        [Export(PropertyHint.Layers2dPhysics)]
        public uint shootLayer;
        
        
        protected Value interval;
        
        protected Value range;

        protected Value clipSize;

        protected Value reloadTime;
        
        
        /// <summary>
        /// 朝向。
        /// </summary>
        protected Vector2 orientation;
        
        /// <summary>
        /// 冷却时间读秒。
        /// </summary>
        protected float coolTick;

        /// <summary>
        /// 是否冷却中。
        /// </summary>
        protected bool isCooling;

        /// <summary>
        /// 剩余子弹数量。
        /// </summary>
        protected int bulletCount;
        
        /// <summary>
        /// 换弹读秒。
        /// </summary>
        protected float reloadTick;

        /// <summary>
        /// 是否换弹中。
        /// </summary>
        protected bool isReloading;

        
        protected Timer reloadTimer;

        /// <summary>
        /// 射击间隔。
        /// </summary>
        public Value Interval => interval;
        
        /// <summary>
        /// 射击范围。
        /// </summary>
        public Value Range => range;
        
        /// <summary>
        /// 弹夹容量。
        /// </summary>
        public Value ClipSize => clipSize;

        /// <summary>
        /// 换弹时间。
        /// </summary>
        public Value ReloadTime => reloadTime;


        public float ReloadProgress => reloadTick / reloadTime.final;
        
        public override void Reset()
        {
            base.Reset();
            interval = Value.Zero;
            range = Value.Zero;
            clipSize = Value.Zero;
            reloadTime = Value.Zero;
            orientation = Vector2.Zero;
            coolTick = 0f;
            isCooling = false;

            interval.basic = defaultInterval;
            range.basic = defaultRange;
            clipSize.basic = defaultClipSize;
            reloadTime.basic = defaultReloadTime;
            bulletCount = clipSize.intFinal;
        }


        protected override void SubscribeEvents()
        {
            ModuleEvent.Subscribe<EventMouseInput>(OnMouseInput, Entity);
            ModuleEvent.Subscribe<EventTimeout>(OnTimeout, Entity);
        }

        protected virtual void OnTimeout(object sender, EventTimeout e)
        {
            if ("shooter_cooling".Equals(e.timerName))
            {
                isCooling = false;
            }
            
            if ("shooter_reload".Equals(e.timerName))
            {
                isReloading = false;
                bulletCount = clipSize.intFinal;
            }

        }

        protected virtual void OnMouseInput(object sender, EventMouseInput e)
        {
            if (!e.fire[1])
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
            ModuleTimer.StartNew(Entity, interval.final, "shooter_cooling");
            
            // 根据鼠标位置获取方向。
            var globalMousePosition = Entity.GetGlobalMousePosition();
            var direction = globalMousePosition - Entity.Position;
            if (direction != Vector2.Zero)
            {
                orientation = direction.Normalized();
            }
            
            // 生成子弹实体，并控制它的方向和速度。
            var bullet = ModuleEntity.Spawn(EntityType.Bullet, Entity.Position);
            bullet.SendEvent(new EventArrowInput(orientation));
            bullet.SendEvent(new EventValueUpdate("movedRange", range));
            bullet.SendEvent(new EventValueUpdate("speed", new Value(50f))); // TODO 配置子弹速度。
            bullet.SendEvent(new EventValueUpdate("raycastLayer", new Value(shootLayer)));

            --bulletCount;
        }


        protected virtual void Reload()
        {
            if (isReloading)
            {
                return;
            }
            isReloading = true;
            ModuleTimer.StartNew(Entity, reloadTime.final, "shooter_reload");
        }




    }
}