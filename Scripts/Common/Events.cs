using Godot;
using Godot.Collections;

namespace Frame.Common
{

    public readonly struct EventArrowInput : IEventArgs
    {
        public readonly Vector2 arrow;

        public EventArrowInput(Vector2 arrow)
        {
            this.arrow = arrow;
        }
    }

    public readonly struct EventMouseInput : IEventArgs
    {
        /// <summary>
        /// 布尔数组表示鼠标按键的状态。
        /// fire[对应鼠标键]取值，true代表按下。
        /// </summary>
        public readonly bool[] fire;

        public EventMouseInput(bool[] fire)
        {
            this.fire = fire;
        }
    }


    public readonly struct EventActionInput : IEventArgs
    {
        public readonly bool[] action;

        public EventActionInput(bool[] action)
        {
            this.action = action;
        }
    }

    /// <summary>
    /// 数值更新。
    /// </summary>
    public readonly struct EventValueUpdate : IEventArgs
    {
        public readonly string name;
        
        public readonly Value value;

        public EventValueUpdate(string name, Value value)
        {
            this.name = name;
            this.value = value;
        }
    }

    /// <summary>
    /// 移动射线检测。
    /// </summary>
    public readonly struct EventMoverRaycast : IEventArgs
    {
        public readonly Dictionary results;

        public EventMoverRaycast(Dictionary results)
        {
            this.results = results;
        }
    }


}