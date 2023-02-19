namespace Frame.Common
{
    public static class Constants
    {
        public const string resourceRoot = "res://";
        public const string sceneSuffix = ".tscn";
        public const string databaseSuffix = ".csv";

        public const float gravity = 9.8f * 3f;

    }

    public enum EntityType
    {
        Player,
        Zombie,
        Bullet
        
    }


    public enum FormType
    {
        Logo,
    }

    public enum SceneType
    {
        Test
    }

    /// <summary>
    /// 实体移动方式。
    /// </summary>
    public enum EntityMoveMode
    {
        /// <summary>
        /// 默认移动方式，无视物理碰撞。
        /// </summary>
        Translate,
        
        /// <summary>
        /// 采用KinematicBody的MoveAndCollide()。
        /// </summary>
        MoveAndCollide,
        
        /// <summary>
        /// 采用KinematicBody的MoveAndSlide()。
        /// </summary>
        MoveAndSlide,
        
        /// <summary>
        /// 采用KinematicBody的MoveAndSlideWithSnap()。
        /// </summary>
        MoveAndSlideWithSnap,
    }

    /// <summary>
    /// 响应输入的方式。
    /// </summary>
    public enum ResponseInputMode
    {
        /// <summary>
        /// 平台跳跃，左右键移动，上键跳跃，下键蹲下。
        /// </summary>
        Platform, 
        
        /// <summary>
        /// 俯视角，上下左右键各自代表一个方向。
        /// </summary>
        TopView,
    }
}