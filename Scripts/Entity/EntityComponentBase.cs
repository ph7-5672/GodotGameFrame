using Frame.Common;
using Godot;

namespace Frame.Entity
{
    public class EntityComponentBase<T> : Node, IEntityComponent where T : Node2D
    {
        public T Entity { get; private set; }

        public override void _Ready()
        {
            Entity = GetParent<T>();
            Reset();
            Init();
        }

        /// <summary>
        /// 订阅所有需要的事件。
        /// </summary>
        protected virtual void Init()
        {
            
        }

        /// <summary>
        /// 重置属性。
        /// </summary>
        public virtual void Reset()
        {
        }
    }
}