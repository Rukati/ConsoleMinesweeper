using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sapper_C_
{

    internal class Program
    {
        static string bomb = "[*]";

        static void ascii()
        {
            Console.WriteLine(" _____    ___    _____   _____   _____   _____     ___   _____     _  _     ___ ");
            Console.WriteLine("/  ___|  / _ \'  | ___ \' | ___ \' |  ___| | ___ \'   |  _| /  __ \'  _| || |_  |_  |");
            Console.WriteLine("\' `--.  / /_\' \' | |_/ / | |_/ / | |__   | |_/ /   | |   | /  \'/ |_  __  _|   | |");
            Console.WriteLine(" `--. \' |  _  | |  __/  |  __/  |  __|  |    /    | |   | |      _| || |_    | |");
            Console.WriteLine("/\'__/ / | | | | | |     | |     | |___  | |\' \'    | |_  | \'__/\' |_  __  _|  _| |");
            Console.WriteLine("\'____/  \'_| |_/ \'_|     \'_|     \'____/  \'_| \'_|   |___|  \'____/   |_||_|   |___|");

        }

        static bool TheEndGame(ref List<List<bool>> matrix)
        {
            for(int i = 0; i < matrix.Count;i++)
            {
                if (matrix[i].Contains(false))
                    return false;
            }
            return true;
        }

        static void WinOrLose(ref List<List<string>> map,ref List<List<string>> matrix)
        {
            bool win = true ;

            for(int i = 0; i < map.Count;i++)
            {
                for(int j = 0; j < map[i].Count;j++)
                {
                    if (map[i][j] == "[⚑]")
                        if (matrix[i][j] != bomb)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(matrix[i][j]);
                            Console.ResetColor();
                            win = false;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(bomb);
                            Console.ResetColor();
                        }
                    else
                        Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }

            if (win)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Всё правильно!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ты не правильно поставил флажок.");
                Console.ResetColor();
            }
        }

        static void Game()
        {
            int MatrixW = new int();
            int MatrixH = new int();

            while (true)
            {
                Console.Write("Выберите поле:\n\t1. [5x5]\n\t2. [10x10]\n\t3. [15x15]\nВвод: ");
                string input = Console.ReadLine();

                int choice = new int();
                if (string.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ошибка! Ввод не может быть пустым.");
                    Console.ResetColor();
                    continue;
                }
                else
                    choice = Convert.ToInt32(input);


                switch (choice)
                {
                    case 1:
                        {
                            MatrixW = 5;
                            MatrixH = 5;
                            break;
                        }
                    case 2:
                        {
                            MatrixW = 10;
                            MatrixH = 10;
                            break;
                        }
                    case 3:
                        {
                            MatrixW = 15;
                            MatrixH = 15;
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.Write("Такого выбора нет. Чудик.\n");
                            break;
                        }
                }
                if (MatrixW != 0)
                {
                    Console.Clear();
                    break;
                }
            }

            List<List<string>> map = new List<List<string>>();
            List<List<bool>> mapTF = new List<List<bool>>();
            List<List<string>> matrix = new List<List<string>>();


            int countMine = new int();
            GenMap(MatrixH, MatrixW, ref matrix, ref map, ref mapTF,ref countMine);

            bool flag = false;

            while (true)
            {

                PrintMap(ref map);
                Console.Write($"Количество мин: {countMine}\n");
                if (flag)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Введён не правильный формат");
                    Console.ResetColor();
                    flag = false;
                }
                Console.Write("Введи координаты -> ряд[Y] столбец[X], не забудь пробел!\nВвод: ");

                List<int> n = new List<int>();

                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    flag = true;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ошибка! Ввод не может быть пустым.");
                    Console.ResetColor();
                }
                else
                    n = input.Split(' ').Select(e => Convert.ToInt32(e)).ToList();

                if (n.Count == 2)
                {
                    if (n[0] > 0 & n[0] <= MatrixH & n[1] > 0 & n[1] <= MatrixW)
                    {
                        Console.Write("Открываем(1) или флажок(0)?\nВвод: ");
                        string InputChoice = Console.ReadLine();

                        int choice = new int();
                        if (string.IsNullOrEmpty(InputChoice))
                        {
                            flag = true;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Ошибка! Ввод не может быть пустым.");
                            Console.ResetColor();
                        }
                        else
                        {
                            choice = Convert.ToInt32(InputChoice);

                            if (choice == 1)
                                if (matrix[n[0] - 1][n[1] - 1] == bomb)
                                    break;
                                else
                                {
                                    if (matrix[n[0] - 1][n[1] - 1] == "[0]")
                                        ChekCell(ref map, ref mapTF, matrix, n[0] - 1, n[1] - 1);
                                    else
                                    {
                                        map[n[0] - 1][n[1] - 1] = matrix[n[0] - 1][n[1] - 1];
                                        mapTF[n[0] - 1][n[1] - 1] = true;
                                    }
                                }
                            else if (choice == 0)
                            {
                                map[n[0] - 1][n[1] - 1] = "[⚑]";
                                mapTF[n[0] - 1][n[1] - 1] = true;
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                    else
                        flag = true;
                }
                else
                {
                    flag = true;
                }

                Console.Clear();
                if(TheEndGame(ref mapTF))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Проверяем правильность флажков...");
                    
                    Console.ResetColor();
                    WinOrLose(ref map, ref matrix);
                    return;
                }
            }
            Console.Clear();
            PrintMap(ref matrix);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ты взорвался\n");
            Console.ResetColor();

            return;
        }

        static void ChekCell(ref List<List<string>> map, ref List<List<bool>> mapTF , List<List<string>> matrix, int i, int j)
        {
            Console.WriteLine($"{i + ","+j}");
            string zero = "[0]";
            map[i][j] = matrix[i][j];
            mapTF[i][j] = true;

            if (j + 1 < matrix[i].Count)
            {
                if (matrix[i][j + 1] == zero & map[i][j + 1] == "[ ]")
                    ChekCell(ref map, ref mapTF, matrix, i, j + 1);
                else
                {
                    map[i][j + 1] = matrix[i][j + 1];
                    mapTF[i][j + 1] = true;
                }
                if (i + 1 < matrix.Count)
                {
                    if (matrix[i + 1][j + 1] == zero & map[i + 1][j + 1] == "[ ]")
                        ChekCell(ref map, ref mapTF, matrix, i + 1, j + 1);
                    else
                    {
                        map[i + 1][j + 1] = matrix[i + 1][j + 1];
                        mapTF[i + 1][j + 1] = true;
                    }
                }
                if (i - 1 > -1)
                    if (matrix[i - 1][j + 1] == zero & map[i - 1][j + 1] == "[ ]")
                        ChekCell(ref map, ref mapTF, matrix, i - 1, j + 1);
                    else
                    {
                        map[i - 1][j + 1] = matrix[i - 1][j + 1];
                        mapTF[i - 1][j + 1] = true;
                    }
            }

            if (j - 1 > -1)
            {
                if (matrix[i][j - 1] == zero & map[i][j - 1] == "[ ]")
                    ChekCell(ref map, ref mapTF, matrix, i, j - 1);
                else
                {
                    map[i][j - 1] = matrix[i][j - 1];
                    mapTF[i][j - 1] = true;
                }
                if (i - 1 > -1)
                {
                    if (matrix[i - 1][j - 1] == zero & map[i - 1][j - 1] == "[ ]")
                        ChekCell(ref map, ref mapTF, matrix, i - 1, j - 1);
                    else
                    {
                        map[i - 1][j - 1] = matrix[i - 1][j - 1];
                        mapTF[i - 1][j - 1] = true;
                    }
                }

                if (j - 1 > -1 && i + 1 < matrix.Count)
                    if (matrix[i + 1][j - 1] == zero & map[i + 1][j - 1] == "[ ]")
                        ChekCell(ref map, ref mapTF, matrix, i + 1, j - 1);
                    else
                    {
                        map[i + 1][j - 1] = matrix[i + 1][j - 1];
                        mapTF[i + 1][j - 1] = true;
                    }

            }

            if (i - 1 > -1)
                if (matrix[i - 1][j] == zero & map[i - 1][j] == "[ ]")
                    ChekCell(ref map, ref mapTF, matrix, i - 1, j);
                else {
                    map[i - 1][j] = matrix[i - 1][j];
                    mapTF[i - 1][j] = true;
                }
            if (i + 1 < matrix.Count)
                if (matrix[i + 1][j] == zero & map[i + 1][j] == "[ ]")
                    ChekCell(ref map, ref mapTF, matrix, i + 1, j);
                else
                {
                    map[i + 1][j] = matrix[i + 1][j];
                    mapTF[i + 1][j] = true;
                }
        }

        static void ChekBomb(ref List<List<string>> matrix,ref int countMine)
        {
            
            for (int i = 0; i < matrix.Count; i++)
            {

                for (int j = 0; j < matrix[i].Count; j++)
                {
                    int countMin = 0;
                    if (matrix[i][j] != bomb)
                    {
                        if (j + 1 < matrix[i].Count)
                        {
                            if (matrix[i][j + 1] == bomb)
                                ++countMin;
                            if (i + 1 < matrix.Count)
                            {
                                if (matrix[i + 1][j + 1] == bomb)
                                    ++countMin;
                            }
                            if (i - 1 > -1)
                                if (matrix[i - 1][j + 1] == bomb)
                                    ++countMin;
                        }

                        if (j - 1 > -1)
                        {
                            if (matrix[i][j - 1] == bomb)
                                ++countMin;
                            if (i - 1 > -1)
                            {
                                if (matrix[i - 1][j - 1] == bomb)
                                    ++countMin;
                            }

                            if (j - 1 > -1 && i + 1 < matrix.Count)
                                if (matrix[i + 1][j - 1] == bomb)
                                    ++countMin;

                        }

                        if (i - 1 > -1)
                            if (matrix[i - 1][j] == bomb)
                                ++countMin;
                        if (i + 1 < matrix.Count)
                            if (matrix[i + 1][j] == bomb)
                                ++countMin;

                    }

                    if (matrix[i][j] != bomb)
                        matrix[i][j] = ($"[{countMin}]");

                    if (matrix[i][j] == bomb)
                        countMine++;
                }

            }
        }

        static void GenMap(int MatrixH, int MatrixW, ref List<List<string>> matrix, ref List<List<string>> map, ref List<List<bool>> mapTF, ref int countMine)
        {
            Random random = new Random();

            for (int i = 0; i < MatrixH; i++)
            {
                matrix.Add(new List<string>());
                map.Add(new List<string>());
                mapTF.Add(new List<bool> ());

                for (int j = 0; j < MatrixW; j++)
                {

                    int rnd = random.Next(0, 9);
                    if (rnd == 1)
                        matrix[i].Add(bomb);
                    else if (rnd == 3)
                        matrix[i].Add(bomb);
                    else if (rnd == 5)
                        matrix[i].Add(bomb);

                    else
                        matrix[i].Add($"[{i + ";" + j}]");

                    map[i].Add("[ ]");
                    mapTF[i].Add(false);

                }

            }
            ChekBomb(ref matrix,ref countMine);
        }

        static void PrintMap(ref List<List<string>> map)
        {
            for (int i = 0; i < map.Count; i++)
            {
                if (i == 0)
                {
                    Console.Write("   ");
                    for (int j = 0; j < map[i].Count; j++)
                    {
                        Console.Write($" {j + 1} ");
                    }
                    Console.WriteLine();
                }
                Console.Write($" {i + 1} ");
                for (int j = 0; j < map[i].Count; j++)
                {
                    string sym = map[i][j];
                    switch (sym)
                    {
                        case "[*]":
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[1]":
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[2]":
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[3]":
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[4]":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[5]":
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[6]":
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[7]":
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        case "[8]":
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(sym);
                            Console.ResetColor();
                            break;
                        default:
                            Console.Write(sym);
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            ascii();
            Console.WriteLine();
            while (true)
            {
                Game();
            }
        }
    }
}
