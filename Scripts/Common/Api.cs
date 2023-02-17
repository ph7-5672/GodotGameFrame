namespace Frame.Common
{
    public interface IEntity
    {
        /// <summary>
        /// 唯一标识。
        /// </summary>
        int Unique { get; set; }
    }
    
    /// <summary>
    /// 实体组件。
    /// </summary>
    public interface IEntityComponent
    {
        
    }

    public interface IEventArgs
    {
    }

    public interface IStage
    {
        void OnEnter();

        void OnExit();

        void OnProcess();
    }
    
    public interface IData
    {
        /// <summary>
        /// 表格里的编号。
        /// </summary>
        int Id { get; set; }
    }
}