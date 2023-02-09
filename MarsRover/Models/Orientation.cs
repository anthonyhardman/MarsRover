using System.Text.Json.Serialization;

namespace MarsRover.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Orientation
    {
        North,
        East,
        South,
        West
    }
}
