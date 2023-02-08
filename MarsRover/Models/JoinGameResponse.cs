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
        public string Orientation { get; set; }
    }

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

    public class LowResolutionMap
    {
        public int LowerLeftRow { get; set; }
        public int LowerLeftColumn { get; set; }
        public int UpperRightRow { get; set; }
        public int UpperRightColumn { get; set; }
        public int AverageDifficulty { get; set; }

        public double ColorTemp => AverageDifficulty / 300.0;
    }
}
