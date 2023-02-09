namespace MarsRover.Models
{
    public class Cell
    {
        public long Row { get; set; }
        public long Column { get; set; }
        public int Difficulty { get; set; }
        public long Hash => (Row << 32 | Column);
        public double ColorTemp => Difficulty / 300.0;

        public Cell(long row, long column, int difficulty)
        {
            Row = row;
            Column = column;
            Difficulty = difficulty;
        }
    }
}
