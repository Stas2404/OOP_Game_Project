using System;
using System.Drawing;

public class LevelUpElement : BaseElement
{
    public LevelUpElement() : base("L", Color.Yellow, Color.Black, true)
    {
    }

    public override void Print()
    {
        Console.Write(Output);
    }
    public override void Interact(Player player)
    {
        player.Level++;
        Console.WriteLine("✨ You stepped on a Level-Up tile! Level increased to " + player.Level);
        Thread.Sleep(1000);
    }

}
