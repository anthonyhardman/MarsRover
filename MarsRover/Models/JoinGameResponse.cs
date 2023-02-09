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
        public int StartingRow { get; set; }
        public int StartingColumn { get; set; }
        public int TargetRow { get; set; }
        public int TargetColumn { get; set; }
        public Cell[] Neighbors { get; set; }
        public LowResolutionMap[] LowResolutionMap { get; set; }
        public Orientation Orientation { get; set; }
    }
}
