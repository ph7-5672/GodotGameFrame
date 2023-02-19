using System;
using Frame.Common;
using Frame.Module;
using Frame.Stage;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 移动组件。
    /// </summary>
    public class MovementComponent : BaseEntityComponent
    {

        /// <summary>
        /// 默认移动速度。
        /// </summary>
        [Export]
        public float defaultSpeed;

        /// <summary>
        /// 默认弹跳力。
        /// </summary>
        [Export]
        public float defaultBounce;

        /// <summary>
        /// 重力比例。
        /// </summary>
        [Export]
        public float gravityScale = 1f;

        /// <summary>
        /// 响应输入的方式。
        /// </summary>
        [Export]
        public ResponseInputMode responseInputMode;
        
        /// <summary>
        /// 移动方式。
        /// </summary>
        [Export]
        public EntityMoveMode moveMode;
        
        /// <summary>
        /// 移动速度。
        /// </summary>
        private Value speed;

        public Value Speed
        {
            get => speed;
            set => Entity.SetValue(nameof(speed), ref speed, value);
        }

        private Value bounce;
        
        /// <summary>
        /// 弹跳力。
        /// </summary>
        public Value Bounce 
        { 
            get => bounce;
            set => Entity.SetValue(nameof(bounce), ref bounce, value);
        }

        /// <summary>
        /// 是否接触地板。
        /// </summary>
        public bool IsOnFloor { get; private set; }


        private float distance;
        
        /// <summary>
        /// 走过的距离。
        /// </summary>
        public float Distance
        {
            get => distance;
            private set
            {
                var origin = distance;
                distance = value;
                EventModule.Send(new DistanceChangeEvent(origin, value), Entity);
            }
        }


        private Vector2 velocity;
        
        private Vector2 snap;

        public override void _Ready()
        {
            base._Ready();
            speed.basic = defaultSpeed;
            bounce.basic = defaultBounce;

            distance = 0f;
            
            EventModule.Subscribe<ActionInputEvent>(OnActionInput, Entity);
        }

        void OnActionInput(object sender, ActionInputEvent eventArgs)
        {

            switch (responseInputMode)
            {
                case ResponseInputMode.Platform:
                {
                    velocity.x = eventArgs.arrow.x;
                    if (eventArgs.arrow.y < 0)
                    {
                        Jump();
                    }

                    break;
                }
                
                case ResponseInputMode.TopView:
                    velocity = eventArgs.arrow;
                    
                    break;
            }
        }
        
        public void Run(float x)
        {
            //velocity.x = x;
        }

        public void Jump()
        {
            if (IsOnFloor)
            {
                velocity.y = -bounce.final;
                snap = Vector2.Zero;
            }
        }

        /// <summary>
        /// 根据velocity移动。
        /// </summary>
        void Move(float delta)
        {
            if (moveMode == EntityMoveMode.Translate)
            {
                var translation = velocity * speed.final * 60f * delta;
                Entity.Translate(translation);

                Distance += translation.Length();

            }
            else if (Entity is KinematicBody2D kinematicBody2D)
            {
                var gravity = Constants.gravity * delta * gravityScale;
                velocity.y += gravity;
                var translation = new Vector2(velocity.x * Speed.final, velocity.y) * 60f;
                
                switch (moveMode)
                {
                    case EntityMoveMode.MoveAndCollide:
                        kinematicBody2D.MoveAndCollide(translation * delta);
                        break;
                    
                    case EntityMoveMode.MoveAndSlide:
                        kinematicBody2D.MoveAndSlide(translation, Vector2.Up, true);
                        break;
                    
                    case EntityMoveMode.MoveAndSlideWithSnap:
                        kinematicBody2D.MoveAndSlideWithSnap(translation, snap, Vector2.Up, true);
                        break;
                }

                Distance += (translation * delta).Length();
            }

            
        }

        /// <summary>
        /// 接触地板检测。
        /// </summary>
        void OnFloorCheck()
        {
            if (Entity is KinematicBody2D kinematicBody2D)
            {
                IsOnFloor = kinematicBody2D.IsOnFloor();
                if (IsOnFloor)
                {
                    velocity.y = Mathf.Min(0f, velocity.y);
                    snap = Vector2.Down * Constants.gravity;
                }
            }
        }

        public override void Reset()
        {
            distance = 0f;
            velocity = Vector2.Zero;
        }

        public override void _PhysicsProcess(float delta)
        {
            Move(delta);
            OnFloorCheck();
        }

    }
}