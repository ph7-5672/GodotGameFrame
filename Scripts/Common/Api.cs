using Godot;

namespace Frame.Common
{

    public interface IEntityLogic
    {
        ValueType ValueType { get; }
        
        void Ready(Object entity);
        
        void Dispose(Object entity);

        void Process(Object entity, float delta);

        void PhysicsProcess(Object entity, float delta);
    }

    public interface IEntityValue
    {
        ValueType Type { get; }
    }

    public interface IEntityBehavior
    {
        BehaviorType Type { get; }
    }


    public interface IStage
    {
        void OnEnter();

        void OnExit();
    }


    public delegate bool Condition<in T>(Node entity, T behavior) where T : struct;

    public delegate bool TestCondition(Node entity, IEntityBehavior behavior);
    
    public delegate void Executor<in T>(Node entity, T behavior) where T : struct;
    
}