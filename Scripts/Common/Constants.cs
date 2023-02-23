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
        public const float unitMeter = 20f;

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
        PlayerInfo
    }

    public enum SceneType
    {
        Test
    }

    public enum DatabaseType
    {
        Guns
    }


}