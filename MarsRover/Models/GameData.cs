using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Models
{
    public class GameData
    {
        public string Token { get; set; }  
        public string Name { get; set; } 
        public string Orientation { get; set; }
        public Coordinate Target { get; set; }
        public Coordinate Position { get; set; }   
        public int Battery { get; set; }
        public LowResolutionMap[] LowResolutionMap { get; set; }
        public Dictionary<long, Cell> HighResolutionMap { get; set; }
    }
}
