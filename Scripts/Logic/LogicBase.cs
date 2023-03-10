using Frame.Common;
using Godot;

namespace Frame.Logic
{
    public abstract class LogicBase<T> : IEntityLogic where T : Node
    {
        public abstract ValueType ValueType { get; }

        protected virtual void Ready(T entity)
        {
        }

        protected virtual void Dispose(T entity)
        {
        }

        protected virtual void Process(T entity, float delta)
        {
        }

        protected virtual void PhysicsProcess(T entity, float delta)
        {
        }

        public void Ready(Object entity)
        {
            if (entity is T t)
            {
                Ready(t);
            }
        }

        public void Dispose(Object entity)
        {
            if (entity is T t)
            {
                Dispose(t);
            }
        }

        public void Process(Object entity, float delta)
        {
            if (entity is T t)
            {
                Process(t, delta);
            }
        }

        public void PhysicsProcess(Object entity, float delta)
        {
            if (entity is T t)
            {
                PhysicsProcess(t, delta);
            }
        }
    }
}