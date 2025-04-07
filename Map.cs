using System.Drawing;

internal class Map
{
    const int Width = 10;
    const int Height = 10;
    BaseElement[,] field = new BaseElement[Width, Height];
    public Player player;
    private List<(int x, int y, string output, Color fg, Color bg, bool isPassable)> initialEnemies
    = new List<(int, int, string, Color, Color, bool)>();

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

        player.LevelsHistory.Add(player.Level);
    }

    public void CreateElement(int count, string output, Color foreground, Color background, bool isPassable)
    {
        Random rnd = new Random();
        int enemyNumber = rnd.Next(3, 6);

        for (int i = 0; i < enemyNumber; i++)
        {
            int x, y;
            do
            {
                x = rnd.Next(0, Width);
                y = rnd.Next(0, Height);
            } while (!(field[y, x] is Element) || field[y, x].Output != "#" || (x == player.X && y == player.Y));

            field[y, x] = new Element(output, foreground, background, isPassable);

            initialEnemies.Add((x, y, output, foreground, background, isPassable));
        }
    }


    public void MovePlayer(int dx, int dy)
    {
        player.Move(dx, dy, Width, Height, field);
    }

    public void ResetAfterDefeat()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                field[i, j] = new Element("#", Color.White, Color.Black, true);
            }
        }

        player.SetPosition(0, 0);
        field[0, 0] = player;

        foreach (var (x, y, output, fg, bg, isPassable) in initialEnemies)
        {
            field[y, x] = new Element(output, fg, bg, isPassable);
        }
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
