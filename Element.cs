using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовий_проект
{
    internal class Element
    {
        public string Output;
        public Color Forecolor;
        public Color Backcolor;
        public Boolean Movement;
        public void Print() 
        {
            Console.WriteLine(Output);
        }
        public Element(string output, Color forecolor, Color backcolor) 
        {
            this.Output = output;
            this.Forecolor = forecolor;
            this.Backcolor = backcolor;
        }
    }
}
