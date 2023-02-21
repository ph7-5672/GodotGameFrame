using System;
using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 实体生成区域。
    /// </summary>
    public class AreaEntitySpawn2D : Area2D
    {
        /// <summary>
        /// 需要生成的实体类型。
        /// </summary>
        [Export]
        public EntityType entityType;

        /// <summary>
        /// 单次生成最小数量。
        /// </summary>
        [Export]
        public int singleMinCount;
        
        /// <summary>
        /// 单次生成最大数量。
        /// </summary>
        [Export]
        public int singleMaxCount;
        
        /// <summary>
        /// 最大数量限制。
        /// </summary>
        [Export]
        public int globalMaxCount;

        /// <summary>
        /// 最大次数限制。
        /// </summary>
        [Export]
        public int globalMaxTimes;

        /// <summary>
        /// 间隔时间。
        /// </summary>
        [Export]
        public float interval;
        
        private float currentTime;

        /// <summary>
        /// 已经生成的数量。
        /// </summary>
        private int spawnedCount;

        /// <summary>
        /// 已生成次数。
        /// </summary>
        private int spawnedTimes;
        
        private bool canSpawn => currentTime == 0 
                                       && spawnedCount < globalMaxCount 
                                       && spawnedTimes < globalMaxTimes;

        public override void _Process(float delta)
        {
            if (canSpawn)
            {
                Spawn();
            }

            Time(delta);
        }

        readonly Random random = new Random();
        
        void Spawn()
        {
            var count = random.Next(singleMinCount, singleMaxCount);
            // 限制生成次数，不超过设定的最大值。
            count = Mathf.Min(count, globalMaxCount - spawnedCount);

            if (count <= 0)
            {
                return;
            }

            for (var i = 0; i < count; i++)
            {
                ModuleEntity.Spawn(entityType);
            }
            spawnedCount += count;
            ++spawnedTimes;
        }

        /// <summary>
        /// 计时。
        /// </summary>
        void Time(float delta)
        {
            currentTime += delta;
            if (currentTime >= interval)
            {
                currentTime = 0f;
            }
        }
        
        
    }
}