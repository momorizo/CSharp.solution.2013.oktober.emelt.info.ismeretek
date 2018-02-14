// készítette: Kányádi Zoltán
// 2017.október emelt szintű "Informatikai alapismeretek" 3. feladata (Amőba)
using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
//using System.Threading.Tasks;


// a programozási feladat 5 pontból áll:  a)-e) pontok
namespace Amoba._2017.oktober.prog._3.fa
{
    // Amőba játékosok :  "O" és "X"
    //
    // az Amőba játék egyes lépéseit tárolni képes 
    // struktúra egy 10x10-es táblán (ezért elég a byte típus)
    // lineValue: a lépés sor (1..10-ig) koordinátája
    // columnValue: a lépés oszlop (1..10-ig) koordinátája
    struct Move
    {
        public byte lineValue;
        public byte columnValue;
    }

    class Amoba
    {
        const byte SizeOfTable = 10;
        const byte maxMoves = 10 * 10 / 2;  // = 50
        // "O" lépései (legfeljeb 50 db (10x10)/2)
        static public Move[] movesOplayer = new Move[maxMoves];
        // "X" lépései (legfeljeb 50 db (10x10)/2)
        static public Move[] movesXplayer = new Move[maxMoves];

        // az "allas1.txt"-ből kiolvasott lépések száma ("O" és "X")
        static public int movesNumberOplayer = 0, movesNumberXplayer = 0;
        static public char movesOn = 'N';
        // N: nobody -- or  
        // O: one or 
        // X: other       
        static public int linesNumber = 0;         // az "allas1.txt" fájl sorainak száma
        static public string[] lines = new string[50];
        static public bool findBoth = false;

        static char[,] table10x10 = new char[10, 10];

        static void Main(string[] args)
        {
            // a) pont leprogramozása
            if (!ReadingFromFileAmoba() && findBoth)
            {
                Console.WriteLine("A fájl beolvasása után kiderült, hogy a két fél lépéseinek száma jelentősen különbözik !!!");
                Console.WriteLine("(--- kettő (2) vagy annál nagyobb; egy az elfogadott) --- ");
                Console.WriteLine("VAGY");
                Console.WriteLine("Nem találtam a fájlban csupán az egyik fél lépéseit");
            }
            Fill_InTable10x10Standard();
            Console.WriteLine("\n");
            Fill_InTable10x10FromMoves();
            ListTable10x10();

            Console.ReadKey();
        }

        // beolvassa a két fél lépéseinek az adatait 
        // [ ezek sor- és oszlop koordinátákat jelentenek a 10x10-es táblán ]
        // és eltárolja külön-külön egyik és másik összes lépését
        // (ld. Amoba osztaly "moves O/X player" struktúra tömbjeit)
        // valamint a lépések számát is külön-külön (ld. "movesNumber O/X player" tagok)
        static bool ReadingFromFileAmoba()
        {
            using (StreamReader sr = new StreamReader("allas1.txt"))
            {
                // char movesOn = 'N'; -- lásd az Amoba osztály mezőjeként
                // N: nobody -- or  
                // O: one or 
                // X: other
                // int linesNumber = 0;         // az "allas1.txt" fájl sorainak száma
                // bool findBoth = false;      // TRUE, ha mindket fél (O és X) lépéseit megtalálta az állományban
                string line = "";
                // string [] lines = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 1)
                    {
                        if (movesOn != 'N')
                            findBoth = true;
                        movesOn = line[0];
                    }
                    else
                    {
                        lines[linesNumber] = line;
                        linesNumber++;
                        string[] coordinates = line.Split(' ');
                        if (movesOn == 'O')
                        {
                            movesOplayer[movesNumberOplayer].lineValue = byte.Parse(coordinates[0]);
                            //movesOplayer[movesNumberOplayer++].columnValue = byte.Parse(coordinates[1]);
                            movesOplayer[movesNumberOplayer].columnValue = byte.Parse(coordinates[1]);
                            movesNumberOplayer++;
                        }
                        else
                        {
                            movesXplayer[movesNumberXplayer].lineValue = byte.Parse(coordinates[0]);
                            //movesXplayer[movesNumberXplayer++].columnValue = byte.Parse(coordinates[1]);
                            movesXplayer[movesNumberXplayer].columnValue = byte.Parse(coordinates[1]);
                            movesNumberXplayer++;
                        }
                    }
                }
            }
            // akkor van rendben a beolvasás, ha a két fél lépésszáma között nincs 1-nél nagyobb különbség 
            // (mgj. az amőba játék szabályai miatt)
            return (Math.Abs(movesNumberOplayer - movesNumberXplayer) < 2);
        }

        // feltölti standard módon - azaz '_' (aláhúzás karakterekkel) - a 
        // 10x10-es játéktáblát
        static void Fill_InTable10x10Standard()
        {
            for (int i = 0; i < SizeOfTable; i++)
                for (int j = 0; j < SizeOfTable; j++)
                {
                    table10x10[i, j] = '_';
                }

        }

        // feltölti a beolvasott lépésadatokból megfelelő módon - (azaz 'O' ill. 'X' karakterekkel) - a 
        // 10x10-es játéktáblát
        static void Fill_InTable10x10FromMoves()
        {
            for (int i = 0; i < movesNumberOplayer; i++)
            {
                table10x10[movesOplayer[i].lineValue, movesOplayer[i].columnValue] = 'O';
            }
            for (int j = 0; j < movesNumberXplayer; j++)
            {
                table10x10[movesOplayer[j].lineValue, movesOplayer[j].columnValue] = 'X';
            }
        }

        static void ListTable10x10()
        {
            for (int i = 0; i < SizeOfTable; i++)
            {
                for (int j = 0; j < SizeOfTable; j++)
                    Console.Write(table10x10[i, j]);
                Console.WriteLine();
            }
        }
    }
}
