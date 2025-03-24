using System;
using System.Drawing;

internal class Element : BaseElement
{
    public bool Movement { get; private set; }

    public Element(string output, Color forecolor, Color backcolor, bool isPassable = true)
        : base(output, forecolor, backcolor, isPassable)
    {
        this.Movement = isPassable;
    }

    public override void Print()
    {
        Console.WriteLine(Output);
    }
}
