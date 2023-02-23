namespace Frame.Common
{
    /// <summary>
    /// 枪支数据。
    /// </summary>
    public class GunsData : IData
    {
        public int Id { get; set; }
        
        public string name;

        public Value interval;

        public Value range;

        public Value clipSize;

        public Value reloadTime;

        public Value bulletSpeed;

        public float caliber;

        public Value damage;

        public void OnParse(string[] line)
        {
            name = line[1];
            interval.basic = float.Parse(line[2]);
            range.basic = float.Parse(line[3]) * Constants.unitMeter;
            clipSize.basic = float.Parse(line[4]);
            reloadTime.basic = float.Parse(line[5]);
            bulletSpeed.basic = float.Parse(line[6]);
            caliber = float.Parse(line[7]) * Constants.unitMeter / 100f;
            damage.basic = float.Parse(line[8]);
        }

    }
}