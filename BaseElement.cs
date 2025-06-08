using System.Drawing;

public abstract class BaseElement
{
    public string Output { get; set; }
    public Color Forecolor { get; protected set; }
    public Color Backcolor { get; protected set; }
    public bool IsPassable { get; protected set; }
    public bool IsEnemy { get; protected set; }

    protected BaseElement(string output, Color forecolor, Color backcolor, bool isPassable, bool isEnemy = false)
    {
        Output = output;
        Forecolor = forecolor;
        Backcolor = backcolor;
        IsPassable = isPassable;
        IsEnemy = isEnemy;
    }

    public abstract void Print();
    public abstract void Interact(Player player);

}
