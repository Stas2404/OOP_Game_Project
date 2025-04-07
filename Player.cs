using System.Drawing;

internal class Player : BaseElement
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Level { get; set; } = 1;
    public List<int> LevelsHistory { get; private set; } = new List<int>();

    public Player(int x, int y) : base("P", Color.Green, Color.Black, true)
    {
        X = x;
        Y = y;
    }

    public void SetPosition(int x, int y)
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
                return;
            }

            if (nextElement.Output == "E")
            {
                LevelsHistory.Add(Level);
                Battle battle = new Battle(this, (Element)nextElement, LevelsHistory);

                if (!battle.PlayerWon)
                {
                    return; 
                }
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
