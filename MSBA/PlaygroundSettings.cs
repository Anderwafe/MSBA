using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBA
{
    public static class PlaygroundSettings
    {
        static int CellCount;
        static int BombCount;
        static BombCountType BombType;
        public enum BombCountType
        {
            Count,
            Percent
        }

        static public bool isInputCorrect(int Cells, int Bombs, BombCountType Type)
        {
            if (Cells < 16 || Cells > Int32.MaxValue)
                return false;
            switch(Type)
            {
                case BombCountType.Count:
                    if (Bombs / (float)Cells <= 0.8 && Bombs / (float)Cells >= 0.05)
                    {
                        CellCount = Cells;
                        BombCount = Bombs;
                        BombType = Type;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case BombCountType.Percent:
                    if(Bombs < 5 || Bombs > 80)
                    {
                        return false;
                    }
                    else
                    {
                        CellCount = Cells;
                        BombCount = Bombs/100;
                        BombType = Type;
                        return true;
                    }
                default: return false;
            }
        }
    }
}
