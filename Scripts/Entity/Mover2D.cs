using System;
using Frame.Common;
using Godot;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace Frame.Entity
{
    /// <summary>
    /// 基本移动组件。
    /// 只提供方向和速度。
    /// </summary>
    public class Mover2D : EntityComponentBase<Node2D>
    {
        
        /// <summary>
        /// 移动计算方式。
        /// </summary>
        [Export]
        public ClippedCamera.ProcessModeEnum processMode = ClippedCamera.ProcessModeEnum.Physics;

        /// <summary>
        /// 走过的距离。
        /// </summary>
        protected float moved => Entity.GetValue("moved");
        
        protected Vector2 velocity;

        protected float speed => Entity.GetValue("speed");



        public override void Reset()
        {
            base.Reset();
            velocity = Vector2.Zero;
        }

        protected override void Init()
        {
            Entity.LoginBehaviorExecutor<BehaviorMove>(Move);
        }
        
        protected virtual void Move(Object entity, BehaviorMove behavior)
        {
            velocity = behavior.velocity;
        }
        
        protected virtual void Translate(Vector2 translation)
        {
            Entity.Translate(translation);
        }


        protected virtual Vector2 GetTranslation(float delta)
        {
            return velocity * speed * delta * Constants.unitMeter;
        }


        protected virtual void Process(float delta)
        {
            var translation = GetTranslation(delta);
            if (Entity.Behave(new BehaviorTranslate(translation, moved)))
            {
                Translate(translation);
                Entity.SetValue("moved", moved + translation.Length());
            }
        }

        public override void _Process(float delta)
        {
            if (processMode == ClippedCamera.ProcessModeEnum.Idle)
            {
                Process(delta);
            }
        }
        

        public override void _PhysicsProcess(float delta)
        {
            if (processMode == ClippedCamera.ProcessModeEnum.Physics)
            {
                Process(delta);
            }
        }

        
    }
}