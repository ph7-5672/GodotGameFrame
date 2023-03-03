using Godot;

namespace Frame.Common
{

    public interface IEntityLogic
    {
        void Ready(Object entity);
        
        void Dispose(Object entity);

        void Process(Object entity, float delta);

        void PhysicsProcess(Object entity, float delta);
    }

    public interface IEntityValue
    {
        
    }

    public interface IStage
    {
        void OnEnter();

        void OnExit();
    }
    
    public interface ITableData
    {
        /// <summary>
        /// 表格里的编号。
        /// </summary>
        int Id { get; set; }

        void OnParse(string[] line);
    }

    public delegate bool Condition<in T>(Node entity, T behavior) where T : struct;
    
    public delegate void Executor<in T>(Node entity, T behavior) where T : struct;
    
}