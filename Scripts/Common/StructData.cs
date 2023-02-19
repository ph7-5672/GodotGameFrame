namespace Frame.Common
{
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


        public int intBasic    => (int) basic;
        public int intAddition => (int) addition;
        public int intMultiple => (int) multiple;


        public float final  => (basic) * (multiple + 1f) + addition;
        public int intFinal => (intBasic) * (intMultiple + 1) + intAddition;

        
        public static Value operator + (Value val1, Value val2)
        {
            var basic = val1.basic + val2.basic;
            var addition = val1.addition + val2.addition;
            var multiple = val1.multiple + val2.multiple;
            return new Value(basic, addition, multiple);
        }
        
        public static Value operator - (Value val1, Value val2) 
        {
            var basic = val1.basic - val2.basic;
            var addition = val1.addition - val2.addition;
            var multiple = val1.multiple - val2.multiple;
            return new Value(basic, addition, multiple);
        }


        public static bool operator ==(Value val1, Value val2) => val1.Equals(val2);

        public static bool operator !=(Value val1, Value val2) => !(val1 == val2);

        public static Value Zero => new Value();

    }
}