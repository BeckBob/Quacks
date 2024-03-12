using pointSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Assembly_CSharp.boardPlacement
{

    public class Program
    {

        private Chips[] boardPlacement;

        public Program() { 
        static void Main(string[] args)
            {
                Chips[] boardPlacement = new Chips[55];

                Chips chip1 = new Chips();
                chip1.Coins = 1;
                chip1.VictoryPoints = 0;
                chip1.Ruby = false;

                Chips chip2 = new Chips();
                chip2.Coins = 2;
                chip2.VictoryPoints = 0;
                chip2.Ruby = false;

                Chips chip3 = new Chips();
                chip3.Coins = 3;
                chip3.VictoryPoints = 0;
                chip3.Ruby = false;

                Chips chip4 = new Chips();
                chip4.Coins = 4;
                chip4.VictoryPoints = 0;
                chip4.Ruby = false;

                Chips chip5 = new Chips();
                chip5.Coins = 5;
                chip5.VictoryPoints = 0;
                chip5.Ruby = true;

                Chips chip6 = new Chips();
                chip6.Coins = 6;
                chip6.VictoryPoints = 1;
                chip6.Ruby = false;

                Chips chip7 = new Chips();
                chip7.Coins = 7;
                chip7.VictoryPoints = 1;
                chip7.Ruby = false;

                Chips chip8 = new Chips();
                chip8.Coins = 8;
                chip8.VictoryPoints = 1;
                chip8.Ruby = false;

                Chips chip9 = new Chips();
                chip9.Coins = 9;
                chip9.VictoryPoints = 1;
                chip9.Ruby = true;

                Chips chip10 = new Chips();
                chip10.Coins = 10;
                chip10.VictoryPoints = 2;
                chip10.Ruby = false;

                Chips chip11 = new Chips();
                chip11.Coins = 11;
                chip11.VictoryPoints = 2;
                chip11.Ruby = false;

                Chips chip12 = new Chips();
                chip12.Coins = 12;
                chip12.VictoryPoints = 2;
                chip12.Ruby = false;

                Chips chip13 = new Chips();
                chip13.Coins = 13;
                chip13.VictoryPoints = 2;
                chip13.Ruby = true;

                Chips chip14 = new Chips();
                chip14.Coins = 14;
                chip14.VictoryPoints = 3;
                chip14.Ruby = false;

                Chips chip15s = new Chips();
                chip15s.Coins = 15;
                chip15s.VictoryPoints = 3;
                chip15s.Ruby = false;

                Chips chip15b = new Chips();
                chip15b.Coins = 15;
                chip15b.VictoryPoints = 3;
                chip15b.Ruby = true;

                Chips chip16s = new Chips();
                chip16s.Coins = 16;
                chip16s.VictoryPoints = 3;
                chip16s.Ruby = false;

                Chips chip16b = new Chips();
                chip16b.Coins = 16;
                chip16b.VictoryPoints = 4;
                chip16b.Ruby = false;

                Chips chip17s = new Chips();
                chip17s.Coins = 17;
                chip17s.VictoryPoints = 4;
                chip17s.Ruby = false;

                Chips chip17b = new Chips();
                chip17b.Coins = 17;
                chip17b.VictoryPoints = 4;
                chip17b.Ruby = true;

                Chips chip18s = new Chips();
                chip18s.Coins = 18;
                chip18s.VictoryPoints = 4;
                chip18s.Ruby = false;

                Chips chip18b = new Chips();
                chip18b.Coins = 18;
                chip18b.VictoryPoints = 5;
                chip18b.Ruby = false;

                Chips chip19s = new Chips();
                chip19s.Coins = 19;
                chip19s.VictoryPoints = 5;
                chip19s.Ruby = false;

                Chips chip19b = new Chips();
                chip19b.Coins = 19;
                chip19b.VictoryPoints = 5;
                chip19b.Ruby = true;

                Chips chip20s = new Chips();
                chip20s.Coins = 20;
                chip20s.VictoryPoints = 5;
                chip20s.Ruby = false;

                Chips chip20b = new Chips();
                chip20b.Coins = 20;
                chip20b.VictoryPoints = 6;
                chip20b.Ruby = false;

                Chips chip21s = new Chips();
                chip21s.Coins = 21;
                chip21s.VictoryPoints = 6;
                chip21s.Ruby = false;

                Chips chip21b = new Chips();
                chip21b.Coins = 21;
                chip21b.VictoryPoints = 6;
                chip21b.Ruby = true;

                Chips chip22s = new Chips();
                chip22s.Coins = 22;
                chip22s.VictoryPoints = 7;
                chip22s.Ruby = false;

                Chips chip22b = new Chips();
                chip22b.Coins = 22;
                chip22b.VictoryPoints = 7;
                chip22b.Ruby = true;

                Chips chip23s = new Chips();
                chip23s.Coins = 23;
                chip23s.VictoryPoints = 7;
                chip23s.Ruby = false;

                Chips chip23b = new Chips();
                chip23b.Coins = 23;
                chip23b.VictoryPoints = 8;
                chip23b.Ruby = false;

                Chips chip24s = new Chips();
                chip24s.Coins = 24;
                chip24s.VictoryPoints = 8;
                chip24s.Ruby = false;

                Chips chip24b = new Chips();
                chip24b.Coins = 24;
                chip24b.VictoryPoints = 8;
                chip24b.Ruby = true;

                Chips chip25s = new Chips();
                chip25s.Coins = 25;
                chip25s.VictoryPoints = 9;
                chip25s.Ruby = false;

                Chips chip25b = new Chips();
                chip25b.Coins = 25;
                chip25b.VictoryPoints = 9;
                chip25b.Ruby = true;

                Chips chip26s = new Chips();
                chip26s.Coins = 26;
                chip26s.VictoryPoints = 9;
                chip26s.Ruby = false;

                Chips chip26b = new Chips();
                chip26b.Coins = 26;
                chip26b.VictoryPoints = 10;
                chip26b.Ruby = false;

                Chips chip27s = new Chips();
                chip27s.Coins = 27;
                chip27s.VictoryPoints = 10;
                chip27s.Ruby = false;

                Chips chip27b = new Chips();
                chip27b.Coins = 27;
                chip27b.VictoryPoints = 10;
                chip27b.Ruby = true;

                Chips chip28s = new Chips();
                chip28s.Coins = 28;
                chip28s.VictoryPoints = 11;
                chip28s.Ruby = false;

                Chips chip28b = new Chips();
                chip28b.Coins = 28;
                chip28b.VictoryPoints = 11;
                chip28b.Ruby = true;

                Chips chip29s = new Chips();
                chip29s.Coins = 29;
                chip29s.VictoryPoints = 11;
                chip29s.Ruby = false;

                Chips chip29b = new Chips();
                chip29b.Coins = 29;
                chip29b.VictoryPoints = 12;
                chip29b.Ruby = false;

                Chips chip30s = new Chips();
                chip30s.Coins = 30;
                chip30s.VictoryPoints = 12;
                chip30s.Ruby = false;

                Chips chip30b = new Chips();
                chip30b.Coins = 30;
                chip30b.VictoryPoints = 12;
                chip30b.Ruby = true;

                Chips chip31s = new Chips();
                chip31s.Coins = 31;
                chip31s.VictoryPoints = 12;
                chip31s.Ruby = false;

                Chips chip31b = new Chips();
                chip31b.Coins = 31;
                chip31b.VictoryPoints = 13;
                chip31b.Ruby = false;

                Chips chip32s = new Chips();
                chip32s.Coins = 32;
                chip32s.VictoryPoints = 13;
                chip32s.Ruby = false;

                Chips chip32b = new Chips();
                chip32b.Coins = 32;
                chip32b.VictoryPoints = 13;
                chip32b.Ruby = true;

                Chips chip33s = new Chips();
                chip33s.Coins = 33;
                chip33s.VictoryPoints = 14;
                chip33s.Ruby = false;

                Chips chip33b = new Chips();
                chip33b.Coins = 33;
                chip33b.VictoryPoints = 14;
                chip33b.Ruby = true;

                Chips chip35 = new Chips();
                chip35.Coins = 35;
                chip35.VictoryPoints = 15;
                chip35.Ruby = false;

                boardPlacement[0] = chip1;
                boardPlacement[1] = chip2;
                boardPlacement[2] = chip3;
                boardPlacement[3] = chip4;
                boardPlacement[4] = chip5;
                boardPlacement[5] = chip6;
                boardPlacement[6] = chip7;
                boardPlacement[7] = chip8;
                boardPlacement[8] = chip9;
                boardPlacement[9] = chip10;
                boardPlacement[10] = chip11;
                boardPlacement[11] = chip12;
                boardPlacement[12] = chip13;
                boardPlacement[13] = chip14;
                boardPlacement[14] = chip15s;
                boardPlacement[15] = chip15b;
                boardPlacement[16] = chip16s;
                boardPlacement[17] = chip16b;
                boardPlacement[18] = chip17s;
                boardPlacement[19] = chip17b;
                boardPlacement[20] = chip18s;
                boardPlacement[21] = chip18b;
                boardPlacement[22] = chip19s;
                boardPlacement[23] = chip19b;
                boardPlacement[24] = chip20s;
                boardPlacement[25] = chip20b;
                boardPlacement[26] = chip21s;
                boardPlacement[27] = chip21b;
                boardPlacement[28] = chip22s;
                boardPlacement[29] = chip22b;
                boardPlacement[30] = chip23s;
                boardPlacement[31] = chip23b;
                boardPlacement[32] = chip24s;
                boardPlacement[33] = chip25s;
                boardPlacement[34] = chip25b;
                boardPlacement[35] = chip26s;
                boardPlacement[36] = chip26b;
                boardPlacement[37] = chip27s;
                boardPlacement[38] = chip27b;
                boardPlacement[39] = chip28s;
                boardPlacement[40] = chip28b;
                boardPlacement[41] = chip29s;
                boardPlacement[42] = chip29b;
                boardPlacement[43] = chip30s;
                boardPlacement[44] = chip30b;
                boardPlacement[45] = chip31s;
                boardPlacement[46] = chip31b;
                boardPlacement[47] = chip32s;
                boardPlacement[48] = chip32b;
                boardPlacement[49] = chip33s;
                boardPlacement[50] = chip33b;
                boardPlacement[51] = chip35;

                Console.WriteLine(boardPlacement[5].Coins);
            }
        }
        public Chips[] GetBoardPlacement()
        {
            return boardPlacement;
        }

    }

}
