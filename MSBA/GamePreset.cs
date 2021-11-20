using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBA
{
    public class GamePreset
    {
        public string? Name { get; private set; }
        public string? CellsCount { get; set; }
        public string? BombsCount { get; set; }
        public PlaygroundSettings.BombCountType BombsType { get; set; }

        public GamePreset(string name, string cellscount, string bombscount, PlaygroundSettings.BombCountType bombstype)
        {
            Name = name;
            CellsCount = cellscount;
            BombsCount = bombscount;
            BombsType = bombstype;
        }
    }
}
