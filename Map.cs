using System.Drawing;

internal class Map
{
    const int Width = 10;
    const int Height = 10;
    BaseElement[,] field = new BaseElement[Width, Height];
    Player player;

    public void Init()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                field[i, j] = new Element("#", Color.White, Color.Black, true);
            }
        }

        player = new Player(0, 0);
        field[0, 0] = player;

        CreateElement(5, "W", Color.Blue, Color.Black, false);
        CreateElement(10, "E", Color.Red, Color.Black, true);
    }

    public void CreateElement(int count, string output, Color foreground, Color background, bool isPassable)
    {
        Random rnd = new Random();
        for (int i = 0; i < count; i++)
        {
            int x, y;
            do
            {
                x = rnd.Next(0, Width);
                y = rnd.Next(0, Height);
            } while (!(field[y, x] is Element) || field[y, x].Output != "#" || (x == player.X && y == player.Y));

            field[y, x] = new Element(output, foreground, background, isPassable);
        }
    }


    public void MovePlayer(int dx, int dy)
    {
        player.Move(dx, dy, Width, Height, field);
    }

    public void Print()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Console.Write(field[i, j].Output + " ");
            }
            Console.WriteLine();
        }
    }
}
