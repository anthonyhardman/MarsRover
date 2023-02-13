namespace MarsRover.Models
{
    public class Cell
    {
        public long X { get; set; }
        public long Y { get; set; }
        public int Difficulty { get; set; }
        public long Hash => (X << 32 | Y);
        public double ColorTemp => Difficulty / 300.0;

        public Cell(long x, long y, int difficulty)
        {
            X = x;
            Y = y;
            Difficulty = difficulty;
        }
    }
}
