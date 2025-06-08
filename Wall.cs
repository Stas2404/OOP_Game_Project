using System.Drawing;

public class Wall : BaseElement
{
    public Wall()
        : base("W", Color.Blue, Color.Black, false)
    {
    }

    public override void Print()
    {
        Console.Write(Output);
    }
    public override void Interact(Player player)
    {
    }
}
