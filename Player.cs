using System.Drawing;

internal class Player : BaseElement
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Player(int x, int y) : base("P", Color.Green, Color.Black, true)
    {
        X = x;
        Y = y;
    }

    public void Move(int dx, int dy, int width, int height, BaseElement[,] field)
    {
        int newX = X + dx;
        int newY = Y + dy;

        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
        {
            BaseElement nextElement = field[newY, newX];

            if (!nextElement.IsPassable)
            {
                Console.WriteLine("Ви врізалися у стіну!");
                return;
            }

            if (nextElement.Output == "E")
            {
                Console.WriteLine("Ви натрапили на ворога!");
            }

            field[Y, X] = new Element("#", Color.White, Color.Black, true);
            X = newX;
            Y = newY;
            field[Y, X] = this;
        }
    }

    public override void Print()
    {
        Console.Write(Output);
    }
}
