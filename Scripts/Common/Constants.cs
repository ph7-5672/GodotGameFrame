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
        #region 主角
        Police,
        Doctor,
        Soldier,
        #endregion

        #region 敌人
        Zombie,

        #endregion
        Gun,
        Bullet,
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
        Guns,
        Mover,
        Shooter,
    }


    public enum EventType
    {
        /// <summary>
        /// 计时器计时事件。
        /// </summary>
        Timeout,
        
        /// <summary>
        /// 实体生成事件。
        /// </summary>
        EntitySpawn,
    }

}