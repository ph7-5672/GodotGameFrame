namespace Frame.Common
{
    public static class Constants
    {
        public const string resourceRoot = "res://";
        public const string sceneSuffix = ".tscn";
        public const string databaseSuffix = ".csv";

        /// <summary>
        /// 单位米。
        /// </summary>
        public const float unitMeter = 16f;
        public const float gravity = 9.8f;

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
    /// 行动类型。
    /// </summary>
    public enum ActionType
    {
        Dodge,
        Slide,
        Defense
    }

}