using System.Drawing;

internal abstract class BaseElement
{
    public string Output { get; protected set; }
    public Color Forecolor { get; protected set; }
    public Color Backcolor { get; protected set; }
    public bool IsPassable { get; protected set; }

    protected BaseElement(string output, Color forecolor, Color backcolor, bool isPassable)
    {
        Output = output;
        Forecolor = forecolor;
        Backcolor = backcolor;
        IsPassable = isPassable;
    }

    public abstract void Print();
}
