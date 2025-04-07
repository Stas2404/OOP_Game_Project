internal class Program
{
    static void Main(string[] args)
    {
        Map map = new Map();
        map.Init();
        map.Print();

        List<int> playerLevelsHistory = new List<int>(); // Initialize the list to store player levels

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.Clear();

            switch (key.Key)
            {
                case ConsoleKey.W:
                    map.MovePlayer(0, -1);
                    break;
                case ConsoleKey.S:
                    map.MovePlayer(0, 1);
                    break;
                case ConsoleKey.A:
                    map.MovePlayer(-1, 0);
                    break;
                case ConsoleKey.D:
                    map.MovePlayer(1, 0);
                    break;
                case ConsoleKey.Escape:
                    return;
            }

            map.Print();
        }
    }
}
