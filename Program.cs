using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Intrinsics.X86;

namespace ConsoleApp28
{
    internal class Dino
    {
        public ConsoleKey key;
        public string Link;
    }

    internal class json : de_ser
    {
        protected static List<Dino> all = new List<Dino>();
        protected static List<string> links = new List<string>();
        protected static List<ConsoleKey> keys = new List<ConsoleKey>();
        public static void nach()
        {
            int pos = 3;
            Console.Clear();
            Console.WriteLine($"Список того, что можно открыть");
            foreach (Dino a in all)
            {
                Console.SetCursorPosition(3, pos);
                Console.WriteLine(a.key);
                Console.SetCursorPosition(16, pos);
                Console.WriteLine(a.Link);
                pos++;
            }
            Console.WriteLine();
            Console.WriteLine($"Добавить горячую клавишу - 'F1'");
            Console.WriteLine($"Удалить горячую клавишу - 'F2'");
            Console.WriteLine($"Режим открытия - 'F3'");
            ser(all);
        }
        public static void New()
        {
            Dino hg = new Dino();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите клавишу");
                hg.key = Console.ReadKey().Key;
                if (keys.Contains(hg.key))
                {
                    Console.Clear();
                    Console.WriteLine("Горячая клавиша уже забинжена, попробуй другую.");
                    Thread.Sleep(700);
                }
                else
                {
                    keys.Add(hg.key);
                    break;
                }
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ссылка на объект");
                hg.Link = Console.ReadLine();
                if (links.Contains(hg.Link))
                {
                    Console.Clear();
                    Console.WriteLine("Ссылка существует, попробуйте другую");
                    Thread.Sleep(700);
                }
                else
                {
                    links.Add(hg.Link);

                    break;
                }
            }
            all.Add(hg);
        }
        public static void Open()
        {
            Console.SetCursorPosition(0, 9 + all.Count);
            Console.WriteLine("Вводите клавиши, которые доступны. \nЧтобы выйти нажмите '/' ");
            ConsoleKeyInfo n = Console.ReadKey(true);
            int u = 0;
            foreach (ConsoleKey g in keys)
            {
                if (g == n.Key)
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo { FileName = links[u], UseShellExecute = true });
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка");
                    }
                    break;
                }
                else
                {
                    u++;
                }
            }
        }
    }
    class arrow : json
    {
        public static int Del()
        {
            nach();
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("->");
            ConsoleKeyInfo k;
            int pos = 3;
            do
            {
                Console.SetCursorPosition(0, pos);
                k = Console.ReadKey();
                if (k.Key == ConsoleKey.DownArrow)
                {
                    pos++;
                    if (pos > 2 + all.Count)
                    {
                        pos--;
                    }
                }
                if (k.Key == ConsoleKey.UpArrow)
                {
                    pos--;
                    if (pos < 3)
                    {
                        pos = 3;
                    }
                }
                Console.Clear();
                nach();
                Console.SetCursorPosition(0, pos);
                Console.WriteLine("->");
            } while (k.Key != ConsoleKey.Enter);
            return pos - 3;
        }
    }
    class de_ser
    {
        public static List<Dino> deser()
        {
            return JsonConvert.DeserializeObject<List<Dino>>(File.ReadAllText("C:\\Users\\user\\Desktop\\Result.json")) ?? new List<Dino>();
        }
        public static void ser(List<Dino> all)
        {
            File.WriteAllText("C:\\Users\\user\\Desktop\\Result.json", JsonConvert.SerializeObject(all));
        }

    }
    class Program : json
    {
        static void Main()
        {
            all = deser();
            foreach (Dino l in all)
            {
                keys.Add(l.key);
                links.Add(l.Link);
            }
            //do
            //{
            //  Console.Clear();
            //  nach();
            //  key = Console.ReadKey();
            //}  while ((key.Key == ConsoleKey.A));
            ConsoleKeyInfo key;
            while (true)
                {
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.F1)
                    {
                        New();
                    }
                    else if (key.Key == ConsoleKey.F2)
                    {
                        int pos = arrow.Del();
                        all.RemoveAt(pos);
                        links.RemoveAt(pos);
                        keys.RemoveAt(pos);
                    }
                    else if (key.Key == ConsoleKey.F3)
                    {
                        Open();
                    }    
            }
               

        }
    }
}
