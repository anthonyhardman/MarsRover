namespace MarsRover.Models
{
    public class MoveResponse
    {
        public int row { get; set; }
        public int column { get; set; }
        public int batteryLevel { get; set; }
        public Cell[] neighbors { get; set; }
        public string message { get; set; }
        public Orientation orientation { get; set; }
    }

}
