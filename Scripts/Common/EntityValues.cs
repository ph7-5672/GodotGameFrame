using System.Collections.Generic;
using Godot;

namespace Frame.Common
{
    /// <summary>
    /// 基础值类型。
    /// </summary>
    public struct Value
    {
        public float basic;
        public float addition;
        public float multiple;

        public Value(float basic = 0, float addition = 0, float multiple = 0)
        {
            this.basic = basic;
            this.addition = addition;
            this.multiple = multiple;
        }


        public int intBasic => (int) basic;
        public int intAddition => (int) addition;
        public int intMultiple => (int) multiple;


        public float final => (basic) * (multiple + 1f) + addition;
        public int intFinal => (intBasic) * (intMultiple + 1) + intAddition;


        public static Value operator +(Value val1, Value val2)
        {
            var basic = val1.basic + val2.basic;
            var addition = val1.addition + val2.addition;
            var multiple = val1.multiple + val2.multiple;
            return new Value(basic, addition, multiple);
        }

        public static Value operator -(Value val1, Value val2)
        {
            var basic = val1.basic - val2.basic;
            var addition = val1.addition - val2.addition;
            var multiple = val1.multiple - val2.multiple;
            return new Value(basic, addition, multiple);
        }


        public static bool operator ==(Value val1, Value val2)
        {
            return val1.basic.CompareTo(val2.basic) == 0 &&
                   val1.addition.CompareTo(val2.addition) == 0 &&
                   val1.multiple.CompareTo(val2.multiple) == 0;
        }

        public static bool operator !=(Value val1, Value val2) => !(val1 == val2);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Value Zero => new Value();
    }

    public struct ValueMove2D : IEntityValue
    {
        public Vector2 velocity;

        public readonly ProcessMode processMode;

        public ValueMove2D(ProcessMode processMode = ProcessMode.Physics)
        {
            this.processMode = processMode;
            velocity = Vector2.Zero;
        }
    }

    public struct ValueMove2DPlatform : IEntityValue
    {
        public Vector2 velocity;
        public Vector2 snap;
        public bool isOnFloor;
        public Value speed;
        public Value bounce;
    }


    public struct ValueShooter : IEntityValue
    {
        public readonly string name;
        public uint shootLayer;
        public Value interval;
        public Value range;
        public Value magazine;
        public Value reloadTime;
        public Value bulletSpeed;
        public Value caliber;
        public Value damage;
        public Value spread;

        public bool isCooling;
        public int bulletCount;

        public ValueShooter(IReadOnlyList<string> lineCsv) : this()
        {
            name = lineCsv[1];
            shootLayer = uint.Parse(lineCsv[2]);
            interval.basic = float.Parse(lineCsv[3]);
            range.basic = float.Parse(lineCsv[4]);
            magazine.basic = float.Parse(lineCsv[5]);
            reloadTime.basic = float.Parse(lineCsv[6]);
            bulletSpeed.basic = float.Parse(lineCsv[7]);
            caliber.basic = float.Parse(lineCsv[8]);
            damage.basic = float.Parse(lineCsv[9]);
            spread.basic = float.Parse(lineCsv[10]);
        }
    }

    public readonly struct ValueHero : IEntityValue
    {
    }

    public struct ValueBullet : IEntityValue
    {
        /// <summary>
        /// 拖尾长度。
        /// </summary>
        public float tail;

        public ValueBullet(float tail)
        {
            this.tail = tail;
        }
    }


    public struct ValueHealth : IEntityValue
    {
        public float point;
        public Value limit;

        public ValueHealth(float point, Value limit)
        {
            this.point = point;
            this.limit = limit;
        }
    }
}