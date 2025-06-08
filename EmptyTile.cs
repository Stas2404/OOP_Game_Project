using System.Drawing;

public class EmptyTile : BaseElement
{
    public EmptyTile()
        : base(".", Color.White, Color.Black, true)
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
