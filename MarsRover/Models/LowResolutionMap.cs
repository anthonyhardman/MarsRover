namespace MarsRover.Models
{
    public class LowResolutionMap
    {
        public int LowerLeftRow { get; set; }
        public int LowerLeftColumn { get; set; }
        public int UpperRightRow { get; set; }
        public int UpperRightColumn { get; set; }
        public int AverageDifficulty { get; set; }

        public double ColorTemp => AverageDifficulty / 300.0;
    }
}
