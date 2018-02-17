// készítette: Kányádi Zoltán
// 2013.október emelt szintű "Informatikai ismeretek" 3. feladata (Amőba)
// <<<Szöveges fájlban adott input adatsoron dolgozó program elkészítése>>> : pl. allas1.txt (input fájl)
// a programozási feladat 5 pontból áll:  a)-e) pontok
using System;
using System.IO;



namespace Amoebae_2013oktoberProg3_fa   // Amőba - Amoebae
{
    // Amőba játékosok :  "O" és "X"
    //
    // az Amőba játék egyes lépéseit tárolni képes 
    // struktúra egy 10x10-es táblán egy mező (ezért elég a byte típus)
    // lineValue: a lépés sor (1..10-ig) koordinátája
    // columnValue: a lépés oszlop (1..10-ig) koordinátája
    struct Move
    {
        public byte lineValue;
        public byte columnValue;
        public bool SameMoves(Move other)
        {
            bool isTheseEquals = false;
            if (lineValue == other.lineValue
                &&
                columnValue == other.columnValue)
                isTheseEquals = true;
            return isTheseEquals;
        }
    }

    class Amoba
    {
        const byte SizeOfTable = 10;
        const byte maxMoves = 10 * 10 ;  // = 100
        // "O" lépései (legfeljeb 50 db (10x10)/2)
        static public Move[] movesOplayer = new Move[maxMoves/2];
        // "X" lépései (legfeljeb 50 db (10x10)/2)
        static public Move[] movesXplayer = new Move[maxMoves/2];
        // az összes lépés ("O" és "X" lépései egyaránt)
        static public Move[] allMovesAllPlayers = new Move[maxMoves];

        // az "allas1.txt"-ből kiolvasott lépések száma ("O", "X" és összes)
        static public int movesNumberOplayer = 0, movesNumberXplayer = 0, allMovesNumber = 0;
        // tárolja az első lépést tevőt ("O" vagy "X")
        static public char firstMove = 'U';  // U - undefined, azaz "firstMove" értéke még határozatlan
        static public char movesOn = 'N';
        // N: nobody -- or  
        // O: one  player or 
        // X: other player

        // az "allas1.txt" vagy egyéb bemeneti fájl sorainak száma és sorai
        static public int linesNumber = 0;
        static public string[] lines = new string[maxMoves+2];
        
        // annak ellenőrzésére, hogy a beolvasott fájl tartalmazza mindkét játékos lépéseit
        static public bool foundBoth = false;

        // ... egy célszerűen megválasztott adatszerkezet segítségével ...
        // a) ponthoz
        static char[,] table10x10 = new char[10, 10];
        static char actualPlayer = 'U'; // "undefined" - a lépésen lévő játékos ("O" vagy "X")

        // az "e)" ponthoz
        static int actualBeginIndex = -1, actualEndIndex = -1 /*, actualLengthStringPlayer = 0*/;
        static byte longestSerial = 0; // az e) pont kéri a soron lévő játékos leghosszabb sorozatának hosszát

        static void Main(string[] args)
        {
            // ***********************************************************************************
            // a) pont leprogramozása                        (6 pont)
            /*
             * A program olvassa be egy, a felsorolt szabályoknak megfelelő szöveges állomány tartalmát
             * és tárolja el a játékállást egy célszerűen megválasztott adatszerkezet segítségével, a
             * későbbi feldolgozás céljából!            
            */
            Console.WriteLine("***********************************************************");
            Console.WriteLine("Az a) pontnak megfelelő kimenet.");
            Console.WriteLine("***********************************************************");
            Console.Write("\nKérem adja meg a játék aktuális pozicióját/mentését tartalmazó fájl nevét: ");
            string positionGameFile = Console.ReadLine();  // pl." allas2.txt"
            // tesztelés érdekében, majd törölhető (2 sor)
            if (positionGameFile == "")
                positionGameFile = "allas1.txt";
            if (!IsOKReadingFromFileAmoebae(positionGameFile) && foundBoth)
            {
                Console.WriteLine("A fájl beolvasása után kiderült, hogy a két fél lépéseinek száma jelentősen különbözik !!!");
                Console.WriteLine("(--- kettő (2) vagy annál nagyobb; egy az elfogadott) --- ");
                Console.WriteLine("VAGY");
                Console.WriteLine("Nem találtam a fájlban csupán az egyik fél lépéseit");
            }
            Console.Write("A beolvasás megtörtént.Folytatás billentyűlenyomásra ...");
            Console.ReadKey(); Console.Clear();


            // ***********************************************************************************
            // b) pont leprogramozása                       (3 pont)
            /*
             * Az amőba program még nem tökéletes, ezért néha előfordul, 
             * hogy az állást úgy menti el, mintha ugyanarra a mezőre 
             * többször is léptek volna! Ellenőrizze, hogy a beolvasott
             * játékállás tartalmaz-e ilyen jellegű hibát!
             * Ha igen, írja ki az első, ilyen szempontból hibás lépést! 
             */

            Console.WriteLine("***********************************************************");
            Console.WriteLine("A b) pontnak megfelelő kimenet.");
            Console.WriteLine("***********************************************************");
            int hibaIndexe = ControlDuplicateMistake();
            if (hibaIndexe != -1)
            {
                Console.WriteLine("A játékállás sajnos tartalmaz -- duplikációs -- hibát!");
                Console.WriteLine("A hiba a {0}. lépésben történik meg," +
                                  "a lépés a következő: #{1} {2}#",
                                  hibaIndexe + 1,
                                  allMovesAllPlayers[hibaIndexe].lineValue,
                                  allMovesAllPlayers[hibaIndexe].columnValue);
            }
            else
            {
                
                Console.WriteLine("\nNincs ismétlődő lépés a megadott állományban.\n");
                Console.Write("Folytatás billentyűlenyomásra ...");
                Console.ReadKey();

                // c) pont leprogramozása                   (4 pont)
                /*
                 * Hibátlan állás esetén jelenítse meg a képernyőn a játékállást úgy, ahogy azt a játékosok
                 * látják!
                 * Elegendő, ha egyszerű karakteres megjelenítést alkalmaz, pl. X és O karakterekkel.
                 * Az áttekinthetőség érdekében az üres mezőket is jelölje, pl. egy-egy pont karakterrel.
                 * A négyzethálót, sorszámokat nem kell megjeleníteni.
                 * Példa az egyszerű megjelenítésre az allas1.txt fájl alapján
                 * 
                 * */
                Console.WriteLine("***********************************************************");
                Console.WriteLine("A c) pontnak megfelelő kimenet.");
                Console.WriteLine("***********************************************************");
                FillInTable10x10StandardDot();
                FillInTable10x10FromMoves();
                DisplayTable10x10();
                /*
                 * Console.WriteLine("\n");
                FillInTable10x10Standard();
                FillInTable10x10FromMoves();
                DisplaySecondTable10x10();
                */
            }

            // ***********************************************************************************
            // d) pont leprogramozása                       (3 pont)
            /*
                * Írja ki, hogy ki kezdte a játékot, és hogy melyik játékos következik!
                * 
                */

            // áthelyezve az "Amoba" osztályba tag(ja)ként
            // char actualPlayer = 'U'; // "undefined"

            Console.WriteLine("***********************************************************");
            Console.WriteLine("A d) pontnak megfelelő kimenet.");
            Console.WriteLine("***********************************************************");
            if (firstMove == 'O')
            {
                Console.WriteLine("A játékot az < O > jelű játékos kezdte!");
                if (movesNumberOplayer > movesNumberXplayer)
                {
                    // "actualPlayer" : a soron (lépésen) következő játékos ("O" vagy "X")
                    actualPlayer = 'X';
                    Console.WriteLine("Az < X > jelű játékos van lépésen!");
                }
                else
                {
                    actualPlayer = 'O';
                    Console.WriteLine("Az < O > jelű játékos van lépésen!");
                }
            }
            else
            {
                Console.WriteLine("A játékot az < X > jelű játékos kezdte!");
                if (movesNumberOplayer < movesNumberXplayer)
                {
                    actualPlayer = 'O';
                    Console.WriteLine("Az < O > jelű játékos van lépésen!");
                }
                else
                {
                    actualPlayer = 'X';
                    Console.WriteLine("Az < X > jelű játékos van lépésen!");
                }
            }
            // ***********************************************************************************
            // e) pont leprogramozása                       (6 pont)
            /* Készítsen összesítő táblázatot az alábbi minta szerint arról, hogy a soron 
             * következő játékosnak vízszintes irányban milyen hosszúságú sorozatai vannak, 
             * ezek hol kezdődnek el, és hol végződnek! 
             * (A minta az allas1.txt fájl alapján készült.) 
             * Sor      Kezdete         Vége        Hossz
                2.         4             4            1
                3.         5             6            2
                4.         4             4            1
                5.         3             3            1
                7.         7             8            2
                8.         7             7            1
                9.         3             5            3

             * Állapítsa meg, hogy vízszintes irányban hány jelből áll a leghosszabb sorozata
             * a soron következő játékosnak!
            */

            // az alábbi sor áthelyezve láthatósági okokból
            // byte aBeginIndex = 0, aEndIndex = 10 /*, aLengthStringPlayer = 0*/;
            Console.Clear();
            Console.WriteLine("e) pont:  STATISZTIKA a soron következő játékos ({0})"
                    + "vízszintes sorozatairól!\n", actualPlayer);            
            Console.WriteLine("A TÁBLÁZAT (10X10 -es)!");
            DisplayTable10x10();
            Statistics();

            //Console.Clear();
            Console.WriteLine("A soron következő játékos leghosszabb sorozata vízszintes irányban: {0}", longestSerial);

            Console.WriteLine("Program befejezése egy billentyű lenyomására ... ");
            Console.ReadKey();

        }

        static void DisplayHeader(char actPlayer)
        {
            Console.WriteLine("\nA soron következő játékos (\"{0}\") statisztikai lépésadatai:\n", actPlayer);
            Console.WriteLine("Sor\tKezdete\t\tVége\t\tHossz");
            Console.WriteLine("========================================================\n");
        }

        // e) ponthoz
        // elég a sor számát paraméterként megadni
        // static void DisplayActualPlayerStatisticsLineByLine(int lineNumber, int begin, int end /*, byte length*/)
        static void DisplayActualPlayerStatisticsLineByLine(int lineNumber, int begin, int end /*, byte length*/)
        {
            // one line only    ---   mindig csak egy sort
            // Console.WriteLine("{0,3}. {1,15} {2,20} {3,12}", begin, end /*, length/*/, end - begin + 1);
            Console.WriteLine("{0,2}. {1,7} {2,15} {3,15}", lineNumber, begin, end, end - begin + 1);
        }



        // ***********************************************************************************
        // e) ponthoz
        static void StatisticsOnLine(char actPlayer  /* "O", "X" */, byte linNum /*1..10*/,
                                out int begin, out int end /*, out byte length*/)
        {

            bool found = false /*, interior = false*/;
            byte index = 0;

            // visszaállítjuk a következő sor vizsgálatának kezdetekor
            begin = -1;
            // visszaállítjuk a következő sor vizsgálatának kezdetekor
            end = -1;

            while (index < SizeOfTable)
            {
                if (table10x10[linNum, index] == actPlayer)
                {
                    if (!found)
                    {
                        begin = index;
                        end = index;
                        found = true;
                    }
                    else
                    {
                        end = index;
                    }
                }
                else
                {
                    if (found && (begin <= end))
                    {
                        DisplayActualPlayerStatisticsLineByLine(linNum + 1, begin + 1, end + 1 /*, length*/);
                        found = false;
                    }
                }
                index++;
            }
            if (found) // or (end == SizeOfTable - 1)
                DisplayActualPlayerStatisticsLineByLine(linNum + 1, begin + 1, end + 1);
            // ha hosszabb mint a korábbi sorozat(ok)
            if (longestSerial < end - begin + 1)
                longestSerial = (byte)(end - begin + 1);  // itt a konverzió (int-->byte) kötelező: "cast"-olás
        }

        // ***********************************************************************************
        // e) ponthoz
        // a statisztikát legyártó és képernyőre kiírató "Statistics" metódus
        // használja a
        //      StatisticsOnLine(...)
        //      DisplayActualPlayerStatisticsLineByLine(...)
        //      DisplayHeader(...)
        // metódusokat ( ld. fentebb )

        static void Statistics()
        {
            
            DisplayHeader(actualPlayer);
            for (byte i = 0; i < SizeOfTable; i++)
            {
                StatisticsOnLine(actualPlayer, i, out actualBeginIndex, out actualEndIndex /*, out aLength*/);                
                /*
                 * áthelyezve a StatisticsOnLine metódusba
                
                // visszaállítjuk a következő sor elejére
                aBeginIndex = -1;
                //aEndIndex = SizeOfTable - 1;
                // visszaállítjuk a következő sor elejére
                aEndIndex = -1;
                */                
            }
        }

        // ***********************************************************************************
        // a) ponthoz
        // beolvassa a két fél lépéseinek az adatait 
        // [ ezek sor- és oszlop koordinátákat jelentenek a 10x10-es táblán ]
        // és eltárolja külön-külön egyik és másik összes lépését
        // (ld. Amoba osztaly "moves O/X player" struktúra tömbjeit)
        // valamint a lépések számát is külön-külön (ld. "movesNumber O/X player" tagok)
        static bool IsOKReadingFromFileAmoebae(string fileName) // amőba --> angolul: amoebae
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                // char movesOn = 'N'; -- lásd az Amoba osztály mezőjeként
                // N: nobody -- or  
                // O: one player or 
                // X: other player
                // int linesNumber = 0;         // az "allas1.txt" fájl sorainak száma
                // bool findBoth = false;      // TRUE, ha mindket fél (O és X) lépéseit megtalálta az állományban
                string line = "";
                // string [] lines = null;

                /*
                line = sr.ReadLine();
                if (line.Length == 1 && (line[0] == 'O' || line[0] == 'X')
                */

            while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length == 1 && (line[0] == 'O' || line[0] == 'X'))
                    {
                        if (movesOn == 'N')
                        {
                            if (firstMove == 'U')
                                    firstMove = line[0];                            
                        }
                        else foundBoth = true;
                        movesOn = line[0];
                    }
                    else
                    {
                        lines[linesNumber] = line;
                        linesNumber++;
                        string[] coordinates = line.Split(' ');
                        allMovesAllPlayers[allMovesNumber].lineValue = byte.Parse(coordinates[0]);
                        allMovesAllPlayers[allMovesNumber++].columnValue = byte.Parse(coordinates[1]);
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

        // a) és c) ponthoz
        // feltölti standard módon, azaz '-' (kötőjeles karakterekkel) 
        // a 10x10-es játéktáblát
        static void FillInTable10x10Standard()
        {
            for (int i = 0; i < SizeOfTable; i++)
                for (int j = 0; j < SizeOfTable; j++)
                {
                    table10x10[i, j] = '-';
                }

        }

        // a) és c) ponthoz
        // feltölti "pontozott" standard módon, azaz '.' (pont karakterekkel)
        // a 10x10-es játéktáblát
        static void FillInTable10x10StandardDot()
        {
            for (int i = 0; i < SizeOfTable; i++)
                for (int j = 0; j < SizeOfTable; j++)
                {
                    table10x10[i, j] = '.';
                }

        }

        // a) ponthoz
        // feltölti a beolvasott lépésadatokból megfelelő módon - (azaz 'O' ill. 'X' karakterekkel) - a 
        // 10x10-es játéktáblát
        static void FillInTable10x10FromMoves()
        {
            for (int i = 0; i < movesNumberOplayer; i++)
            {
                // fontos, hogy a beolvasott és tárolt adatok 1..10 közöttiek; ezért 1 -et levonunk, mert
                // nekünk a "table10x10" indexei 0..9 közöttiek
                table10x10[movesOplayer[i].lineValue - 1, movesOplayer[i].columnValue - 1] = 'O';
            }
            for (int j = 0; j < movesNumberXplayer; j++)
            {
                // fontos, hogy a beolvasott és tárolt adatok 1..10 közöttiek; ezért 1 -et levonunk, mert
                // nekünk a "table10x10" indexei 0..9 közöttiek
                table10x10[movesXplayer[j].lineValue - 1, movesXplayer[j].columnValue - 1] = 'X';
            }
        }

        // ez "pontozott" megjelenítést tesz lehetővé
        // (az üres helyek helyett, azaz ahová még nem léptek)

        // c) ponthoz
        // ez "kötőjeles" megjelenítést tesz lehetővé
        // (az üres helyek helyett, azaz ahová még nem léptek)
        static void DisplayTable10x10()
        {
            Console.WriteLine();
            for (int i = 0; i < SizeOfTable;  i++)
            {
                for (int j = 0; j < SizeOfTable; j++)
                {
                    if (j == 0)
                        Console.Write("{0,2}. sor:\t\t", i + 1);
                    Console.Write(table10x10[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // c) ponthoz
        // ez is "kötőjeles" megjelenítést tesz lehetővé
        // (az üres helyek helyett, azaz ahová még nem léptek)
        // különbség az előzőhöz képest a sorok megjelenítésében van (felcserélve a sorrend)
        static void DisplaySecondTable10x10()
        {
            Console.WriteLine();
            for (int i = SizeOfTable - 1; i >= 0; i--)
            {
                for (int j = 0; j < SizeOfTable; j++)
                {
                    if (j == 0)
                        Console.Write("{0,2}. sor:\t\t", i + 1);
                    Console.Write(table10x10[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // b) ponthoz
        // megvizsgálja, hogy a megadott állományból "átvett" lépések
        // között van-e ismétlődő 
        //      -   ha van azzal az értékkel tér vissza, ahányadik a lépés,
        //          attól függetlenül, hogy ki lépte
        //      -   ha nincs (-1)-el tér vissza 
        static int ControlDuplicateMistake()
        {
            int index = -1;
            //bool found = false;
            // movesOplayer: vizsgáljuk "O" lépéseit --- van-e duplikátum 
            for (int i = 0; i < allMovesNumber-1; i++)
                for (int j = i+1; j < allMovesNumber; j++)
                {
                    if (allMovesAllPlayers[i].SameMoves(allMovesAllPlayers[j]))
                        index = j;
                }
            return index;
        }
    }
}
