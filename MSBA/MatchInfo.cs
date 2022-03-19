using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBA
{
    class MatchInfo
    {
        public string? Preset { get; private set; }
        public string? Time { get; private set; }
        public string? MinesCount { get; private set; }
        public string? CellsCount { get; private set; }

        public MatchInfo()
        {

        }
        public MatchInfo(string preset, string time, string minesCount, string cellsCount)
        {
            Preset = preset;
            Time = time;
            MinesCount = minesCount;
            CellsCount = cellsCount;
        }

        public static string GetMatchInfo(MatchInfo Info)
        {
            return JsonSerializer.Serialize<MatchInfo>(Info, new() { WriteIndented = true });
        }
    }
}
