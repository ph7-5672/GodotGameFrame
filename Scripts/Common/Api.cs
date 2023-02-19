namespace Frame.Common
{
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
    }
    
    public interface IData
    {
        /// <summary>
        /// 表格里的编号。
        /// </summary>
        int Id { get; set; }
    }
}