using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSBA
{
    public static class PlaygroundValues
    {
        public static string GameName = "";

        public static char[,] PlaygroundTable;
        // Table cells content marking:
        // N - none
        // B - bomb
        // 0-8 - bombs count

        public static int BombsCount = 0;
        public static string ResultTime = "";
        public static string PresetName = "";

        public static bool CreatePlaygroundTable(int Cells, int Bombs)
        {
            // Creating a empty playground table
            if(Math.Sqrt(Cells) % 1 == 0)
            {
                PlaygroundTable = new char[(int)Math.Sqrt(Cells), (int)Math.Sqrt(Cells)];
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
                PlaygroundTable = new char[Math.Min(x, y), Math.Max(x, y)];
            }

            // Put in every cell of playground table '0'
            for (int i = 0; i < PlaygroundTable.GetLength(0); i++)
            {
                for (int j = 0; j < PlaygroundTable.GetLength(1); j++)
                {
                    PlaygroundTable[i, j] = '0';
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
                for (int i = 0; i < PlaygroundTable.GetLength(0); i++)
                {
                    for (int j = 0; j < PlaygroundTable.GetLength(1); j++)
                    {
                        if (PlaygroundTable[i, j] != 'B' && rand.Next(1000) < 70)
                        {
                            PlaygroundTable[i, j] = 'B';
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
            for(int i = 0; i < PlaygroundTable.GetLength(0); i++)
            {
                for (int j = 0; j < PlaygroundTable.GetLength(1); j++)
                {
                    if (PlaygroundTable[i, j] == 'B')
                    {
                        for (int i1 = i - 1; i1 <= i + 1; i1++)
                        {
                            for (int j1 = j - 1; j1 <= j + 1; j1++)
                            {
                                if (i1 < 0 || j1 < 0 || i1 > PlaygroundTable.GetLength(0) - 1 || j1 > PlaygroundTable.GetLength(1) - 1)
                                {
                                    continue;
                                }
                                if (i1 == i && j1 == j)
                                {
                                    continue;
                                }
                                if (PlaygroundTable[i1, j1] == 'B')
                                {
                                    continue;
                                }
                                PlaygroundTable[i1, j1] = (char)(Convert.ToInt32(PlaygroundTable[i1, j1]) + 1);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}