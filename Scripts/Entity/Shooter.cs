using System.Threading;
using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    public class Shooter : EntityComponentBase<KinematicBody2D>
    {
        [Export]
        public float defaultInterval;
        
        [Export]
        public float defaultRange;
        
        protected Value interval;
        
        protected Value range;

        /// <summary>
        /// 朝向。
        /// </summary>
        protected Vector2 orientation;
        
        /// <summary>
        /// 冷却时间读秒。
        /// </summary>
        protected float coolTime;

        /// <summary>
        /// 是否冷却中。
        /// </summary>
        protected bool isCooling;

        /// <summary>
        /// 射击间隔。
        /// </summary>
        public Value Interval => interval;
        
        /// <summary>
        /// 射击范围。
        /// </summary>
        public Value Range => range;


        public override void Reset()
        {
            base.Reset();
            interval = Value.Zero;
            range = Value.Zero;
            orientation = Vector2.Zero;
            coolTime = 0f;
            isCooling = false;

            interval.basic = defaultInterval;
            range.basic = defaultRange;
        }


        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            ModuleEvent.Subscribe<EventMouseInput>(OnMouseInput, Entity);
        }

        protected virtual void OnMouseInput(object sender, EventMouseInput e)
        {
            if (e.fire[1] && !isCooling)
            {
                Shoot();
                isCooling = true;
            }
        }

        protected virtual void Shoot()
        {
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
            bullet.SendEvent(new EventValueUpdate("range", range));
            bullet.SendEvent(new EventValueUpdate("speed", new Value(100f))); // TODO 配置子弹速度。
        }


        public override void _Process(float delta)
        {
            CooldownProcess(delta);
        }

        protected void CooldownProcess(float delta)
        {
            if (!isCooling)
            {
                return;
            }

            coolTime += delta;
            
            if (coolTime >= interval.final)
            {
                coolTime = 0f;
                isCooling = false;
            }
            
        }


    }
}