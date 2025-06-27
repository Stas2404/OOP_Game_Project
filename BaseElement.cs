using System.Drawing;

public abstract class BaseElement
{
    public string Output { get; set; }
    public Color Forecolor { get; protected set; }
    public Color Backcolor { get; protected set; }
    public bool IsPassable { get; protected set; }
    public bool IsEnemy { get; protected set; }
    public static event Action<string>? OnMessage;

    protected BaseElement(string output, Color forecolor, Color backcolor, bool isPassable, bool isEnemy = false)
    {
        Output = output;
        Forecolor = forecolor;
        Backcolor = backcolor;
        IsPassable = isPassable;
        IsEnemy = isEnemy;
    }
    protected void RaiseMessage(string message)
    {
        OnMessage?.Invoke(message);
    }

    public abstract void Interact(Player player);

}
