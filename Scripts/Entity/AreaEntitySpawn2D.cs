using Frame.Common;
using Frame.Module;
using Godot;

namespace Frame.Entity
{
    /// <summary>
    /// 实体生成区域。
    /// </summary>
    public class AreaEntitySpawn2D : Control
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
        
        private float tick;

        /// <summary>
        /// 已经生成的数量。
        /// </summary>
        private int spawnedCount;

        /// <summary>
        /// 已生成次数。
        /// </summary>
        private int spawnedTimes;
        
        private bool canSpawn => 
            tick == 0 
            && spawnedCount < globalMaxCount 
            && spawnedTimes < globalMaxTimes;

        public override void _Process(float delta)
        {
            if (canSpawn)
            {
                Spawn2D();
            }
            
            Tick(delta);
        }

        void Spawn2D()
        {
            var count = UtilityRandom.Next(singleMinCount, singleMaxCount);
            // 限制生成次数，不超过设定的最大值。
            count = Mathf.Min(count, globalMaxCount - spawnedCount);

            if (count <= 0)
            {
                return;
            }

            for (var i = 0; i < count; i++)
            {
                // 取随机位置。
                var randomPosition = GetRandomPosition();
                ModuleEntity.Spawn2D(entityType, randomPosition);
            }
            spawnedCount += count;
            ++spawnedTimes;
        }

        /// <summary>
        /// 计时。
        /// </summary>
        void Tick(float delta)
        {
            tick += delta;
            if (tick >= interval)
            {
                tick = 0f;
            }
        }
        
        Vector2 GetRandomPosition()
        {
            var min = RectPosition;
            var max = min + RectSize;
            var x = UtilityRandom.Next((int)min.x, (int)max.x);
            var y = UtilityRandom.Next((int)min.y, (int)max.y);
            return new Vector2(x, y);
        }

    }
}