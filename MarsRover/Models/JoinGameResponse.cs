using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Models
{
    public class JoinGameResponse
    {
        public string Token { get; set; }
        public int StartingX { get; set; }
        public int StartingY { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public Cell[] Neighbors { get; set; }
        public LowResolutionMap[] LowResolutionMap { get; set; }
        public Orientation Orientation { get; set; }
    }
}
