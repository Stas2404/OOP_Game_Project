public class GameConsoleUI
{
    private readonly IGameUI ui;

    public GameConsoleUI(IGameUI ui)
    {
        this.ui = ui;
    }

    public int ShowMainMenu()
    {
        while (true)
        {
            ui.Clear();
            ui.WriteLine("Goblins Dungeon");
            ui.WriteLine("1. Play");
            ui.WriteLine("2. Custom Level");
            ui.WriteLine("3. Exit");
            ui.Write("\nSelect an option (1-3): ");

            ConsoleKey key = ui.ReadKey();

            switch (key)
            {
                case ConsoleKey.D1: return 1;
                case ConsoleKey.D2: return 2;
                case ConsoleKey.D3: return 3;
                default:
                    ui.WriteLine("\nInvalid input. Please press 1, 2, or 3.");
                    ui.WaitForKey();
                    break;
            }
        }
    }

    public void ShowMessage(string message)
    {
        ui.WriteLine(message);
        ui.WaitForKey();
    }
}
