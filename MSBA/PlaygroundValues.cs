using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBA
{
    public class PlaygroundValues
    {
        private static PlaygroundValues _instance = new();


        public static PlaygroundValues Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public string GameName { get; set; } = "";

        public char[][] PlaygroundTable { get; set; }
        // Table cells content marking:
        // N - none
        // B - bomb
        // 0-8 - bombs count
        public char[][] CellsStatus { get; set; }
        // C - closed
        // M - marked
        // O - opened

        public int BombsCount { get; set; } = 0;
        public string ResultTime { get; set; } = "";
        public string PresetName { get; set; } = "";

        public bool CreatePlaygroundTable(int Cells, int Bombs)
        {
            // Creating a empty playground table
            if(Math.Sqrt(Cells) % 1 == 0)
            {
                PlaygroundTable = new char[(int)Math.Sqrt(Cells)][];
                CellsStatus = new char[(int)Math.Sqrt(Cells)][];
                for(int i = 0; i < PlaygroundTable.Length; i++)
                {
                    PlaygroundTable[i] = new char[(int)Math.Sqrt(Cells)];
                    CellsStatus[i] = new char[(int)Math.Sqrt(Cells)];

                }

            }
            else
            {
                int x = 1;
                int y = Cells;
                int sub = int.MaxValue;
                for(int i = (int)Math.Sqrt(Cells); i < Cells; i++)
                {
                    for(int j = (int)Math.Sqrt(Cells); j > 0; j--)
                    {
                        if(i * j == Cells)
                        {
                            if (Math.Abs(i - j) < sub)
                            {
                                y = i;
                                x = j;
                                sub = Math.Abs(i - j);
                            }
                            break;
                        }
                    }
                }
                if(x <= 3 || y <= 3)
                {
                    return false;
                }
                PlaygroundTable = new char[Math.Min(x, y)][];
                CellsStatus = new char[Math.Min(x, y)][];
                for (int i = 0; i < Math.Min(x,y); i++)
                {
                    PlaygroundTable[i] = new char[Math.Max(x, y)];
                    CellsStatus[i] = new char[Math.Max(x, y)];

                }
            }

            // Put in every cell of playground table '0'
            for (int i = 0; i < PlaygroundTable.Length; i++)
            {
                for (int j = 0; j < PlaygroundTable[i].Length; j++)
                {
                    PlaygroundTable[i][j] = '0';
                    CellsStatus[i][j] = 'C';
                }
            }

            if(Bombs < 1)
            {
                Bombs = 1;
            }
            BombsCount = Bombs;

            // Adding mines to playground table
            Random rand = new Random(DateTime.UtcNow.Millisecond);
            while (Bombs > 0)
            {
                for (int i = 0; i < PlaygroundTable.Length; i++)
                {
                    for (int j = 0; j < PlaygroundTable[i].Length; j++)
                    {
                        if (PlaygroundTable[i][j] != 'B' && rand.Next(1000) < 70)
                        {
                            PlaygroundTable[i][j] = 'B';
                            Bombs--;
                            if (Bombs == 0)
                                break;
                        }
                    }
                    if (Bombs == 0)
                        break;
                }
            }

            // Adding numbers to playground table
            for(int i = 0; i < PlaygroundTable.Length; i++)
            {
                for (int j = 0; j < PlaygroundTable[i].Length; j++)
                {
                    if (PlaygroundTable[i][j] == 'B')
                    {
                        for (int i1 = i - 1; i1 <= i + 1; i1++)
                        {
                            for (int j1 = j - 1; j1 <= j + 1; j1++)
                            {
                                if (i1 < 0 || j1 < 0 || i1 > PlaygroundTable.Length - 1 || j1 > PlaygroundTable[0].Length - 1)
                                {
                                    continue;
                                }
                                if (i1 == i && j1 == j)
                                {
                                    continue;
                                }
                                if (PlaygroundTable[i1][j1] == 'B')
                                {
                                    continue;
                                }
                                PlaygroundTable[i1][j1] = (char)(Convert.ToInt32(_instance.PlaygroundTable[i1][j1]) + 1);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }

    public class SerializablePlaygroundValues
    {
        public SerializablePlaygroundValues()
        {
            GameName = PlaygroundValues.Instance.GameName;
            PlaygroundTable = PlaygroundValues.Instance.PlaygroundTable;
            CellsStatus = PlaygroundValues.Instance.CellsStatus;
            BombsCount = PlaygroundValues.Instance.BombsCount;
            ResultTime = PlaygroundValues.Instance.ResultTime;
            PresetName = PlaygroundValues.Instance.PresetName;
        }

        public void SetPlaygroundValues()
        {
            PlaygroundValues.Instance = new();
            PlaygroundValues.Instance.GameName = GameName;
            PlaygroundValues.Instance.PlaygroundTable = PlaygroundTable;
            PlaygroundValues.Instance.CellsStatus = CellsStatus;
            PlaygroundValues.Instance.BombsCount = BombsCount;
            PlaygroundValues.Instance.ResultTime = ResultTime;
            PlaygroundValues.Instance.PresetName = PresetName;
        }

        public string GameName { get; set; }
        public char[][] PlaygroundTable { get; set; }
        public char[][] CellsStatus { get; set; }
        public int BombsCount { get; set; }
        public string ResultTime { get; set; }
        public string PresetName { get; set; }
    }
}