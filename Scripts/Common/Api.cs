using Godot;

namespace Frame.Common
{
    /// <summary>
    /// 实体组件。
    /// </summary>
    public interface IEntityComponent
    {
        void Reset();
    }

    public interface IStage
    {
        void OnEnter();

        void OnExit();
    }
    
    public interface IData
    {
        /// <summary>
        /// 表格里的编号。
        /// </summary>
        int Id { get; set; }

        void OnParse(string[] line);
    }

    public interface IEntityData : IData
    {
        string EntityType { get; set; }
    }

    public delegate bool Condition<in T>(Object entity, T behavior) where T : struct;
    
    public delegate void Executor<in T>(Object entity, T behavior) where T : struct;
    
}