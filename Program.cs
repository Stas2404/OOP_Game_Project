using System;

namespace Курсовий_проект
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map();
            map.Init();
            map.Print();

            Console.ReadLine();
        }
    }
}
