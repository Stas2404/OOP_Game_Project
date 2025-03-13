using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовий_проект
{
    internal class Map
    {
        const int Width = 10;
        const int Height = 10;
        Element[,] field = new Element[Width, Height];

        public void Init() 
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    field[i, j] = new Element("#", Color.White, Color.Black);
                }
            }
            field[0, 0] = new Element("P", Color.Green, Color.Black);
            CreateElement(5, "W", Color.Blue, Color.Black);
            CreateElement(10, "E", Color.Red, Color.Black);

        }
        public void CreateElement(int count, string output, Color foreground, Color background)
        {
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = rnd.Next(0, Width);
                    y = rnd.Next(0, Height);
                } while (field[y, x].Output != "#"); 

                field[y, x] = new Element(output, foreground, background);
            }
        }
        public void Print()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(field[i, j].Output + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
