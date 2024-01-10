﻿namespace ServerCore
{
    internal class Program
    {

        static void Main(string[] args)
        {
            int[,] arr = new int[10000, 10000];

            {
                long start = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[y, x] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y, x)걸린 시간 {end - start}");
            }
            {
                long start = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[x, y] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x, y)걸린 시간 {end - start}");
            }
        }
    }
}