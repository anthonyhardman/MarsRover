namespace MarsRover.Models
{
    public class LowResolutionMap
    {
        public int LowerLeftX { get; set; }
        public int LowerLeftY { get; set; }
        public int UpperRightX { get; set; }
        public int UpperRightY { get; set; }
        public int AverageDifficulty { get; set; }

        public double ColorTemp => AverageDifficulty / 300.0;
    }
}
