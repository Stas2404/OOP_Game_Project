using System;
using System.Drawing;

public class GambleElement : BaseElement
{
    public GambleElement() : base("G", Color.Magenta, Color.Black, true)
    {
    }

    public override void Print()
    {
        Console.Write(Output);
    }
    public override void Interact(Player player)
    {
        bool success = new Random().Next(2) == 0;
        player.Level += success ? 1 : -1;
        if (player.Level < 1) player.Level = 1;

        Console.WriteLine(success ? "🎲 Lucky! You gained 1 level." : "🎲 Unlucky! You lost 1 level.");
        Thread.Sleep(1000);
    }

}
