using System.Collections;
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
        public readonly Vector2 position;
        public readonly Vector2 normal;
        public readonly int collider_id;
        public readonly Node2D collider;
        public readonly RID rid;
        public readonly object metadata;

        public EventMoverRaycast(Vector2 position, Vector2 normal, int collider_id, CollisionObject2D collider, RID rid, object metadata)
        {
            this.position = position;
            this.normal = normal;
            this.collider_id = collider_id;
            this.collider = collider;
            this.rid = rid;
            this.metadata = metadata;
        }

        public EventMoverRaycast(IDictionary dictionary) : this()
        {
            position = (Vector2) dictionary[nameof(position)];
            normal = (Vector2) dictionary[nameof(normal)];
            collider_id = (int) dictionary[nameof(collider_id)];
            collider = (Node2D) dictionary[nameof(collider)];
            rid = (RID) dictionary[nameof(rid)];
            metadata =  dictionary[nameof(metadata)];
        }
    }

    /// <summary>
    /// 移动距离到达上限。
    /// </summary>
    public readonly struct EventMovedToRange : IEventArgs
    {
        public readonly float range;

        public EventMovedToRange(float range)
        {
            this.range = range;
        }
    }

    /// <summary>
    /// 计时。
    /// </summary>
    public readonly struct EventTimeout : IEventArgs
    {
        public readonly string timerName;

        public EventTimeout(string timerName)
        {
            this.timerName = timerName;
        }
    }

}