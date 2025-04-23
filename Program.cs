internal class Program
{
    static void Main(string[] args)
    {
        Map map = new Map();
        map.Init();
        map.Print();

        List<int> playerLevelsHistory = new List<int>();

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

                case ConsoleKey.F5:
                    {
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "save.txt");
                        map.SaveGame(path);
                        Console.WriteLine($"Гру збережено у файл: {path}");
                        Thread.Sleep(1000);
                        break;
                    }

                case ConsoleKey.F6:
                    {
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "save.txt");
                        map.LoadGame(path);
                        Console.WriteLine($"Гру завантажено з файлу: {path}");
                        Thread.Sleep(1000);
                        break;
                    }

                case ConsoleKey.Escape:
                    return;
            }

            map.Print();
        }
    }
}
