namespace MarsRover.Models
{
    public class MoveResponse
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int BatteryLevel { get; set; }
        public Cell[] Neighbors { get; set; }
        public string Message { get; set; }
        public Orientation Orientation { get; set; }
    }

}
