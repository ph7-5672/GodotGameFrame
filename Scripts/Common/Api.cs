namespace Frame.Common
{
    public interface IEntity
    {
        int EntityId { get; set; }
    }

    public interface IData
    {
        /// <summary>
        /// 表格里的编号。
        /// </summary>
        int Id { get; set; }
    }
}