using System;
using Godot;

namespace Frame.Common
{
    public static class UtilityRandom
    {
        
        static readonly Random random = new Random();
        
        public static int Next(int min, int max) => random.Next(min, max);

        /// <summary>
        /// 随机一个浮点数。
        /// </summary>
        /// <param name="min">最小范围</param>
        /// <param name="max">最大范围</param>
        /// <param name="precision">精度</param>
        /// <returns></returns>
        public static float NextFloat(float min, float max, int precision)
        {
            var p = Mathf.Pow(10, precision);
            var i = (int) (min * p);
            var a = (int) (max * p);
            var r = (float) Next(i, a);
            return r / p;
        }

    }
}