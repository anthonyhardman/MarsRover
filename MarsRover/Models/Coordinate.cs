using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarsRover.Models
{
    public class Coordinate
    {
        public float X { get; set; }
        public float Y { get; set; }

        [JsonConstructor]
        public Coordinate(float x, float y)
        {
            X = x;
            Y = y;  
        }

        public Coordinate(Coordinate coord)
        {
            X = coord.X;
            Y = coord.Y;
        }

        public override string ToString() 
        {
            return $"{X} {Y}";
        }
    }
}
