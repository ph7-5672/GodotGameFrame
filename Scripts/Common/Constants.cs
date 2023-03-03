using System;

namespace Frame.Common
{
    public static class Constants
    {
        public const string resourceRoot = "res://";
        public const string sceneSuffix = ".tscn";
        public const string databaseSuffix = ".txt";

        /// <summary>
        /// 单位米。
        /// </summary>
        public const float unitMeter = 20f;

        public static readonly ValueType[] valueTypeArray = Enum.GetValues(typeof(ValueType)) as ValueType[];
        public static readonly LogicType[] logicTypeArray = Enum.GetValues(typeof(LogicType)) as LogicType[];
        public static readonly EntityType[] entityTypeArray = Enum.GetValues(typeof(EntityType)) as EntityType[];
        public static readonly DatatableType[] dataTableTypeArray = Enum.GetValues(typeof(DatatableType)) as DatatableType[];
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


    public enum ValueType
    {
        Hero,
        Move2D,
        Move2DPlatform,
        Move2DTopDown,
        Shooter,
        Health,
        Bullet
    }

    public enum LogicType
    {
        Player,
        Move2D,
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

    public enum DatatableType
    {
        Guns,
        Mover,
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
        
        /// <summary>
        /// 实体设置属性。
        /// </summary>
        EntitySetValue,
    }

    public enum ProcessMode
    {
        Idle,
        Physics
    }

    public enum MoveMode
    {
        Translate,
        MoveAndSlide,
        MoveAndSlideWithSnap
    }

}