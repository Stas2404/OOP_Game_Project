internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IGameUI ui = new ConsoleGameUI();
        GameConsoleUI menu = new GameConsoleUI(ui);

        int choice = menu.ShowMainMenu();

        switch (choice)
        {
            case 1:
                var game = new Game(ui);
                game.Init();
                game.Run();
                break;

            case 2:
                var editor = new LevelEditor(ui);
                var editorUI = new LevelEditorConsoleUI(editor, ui);
                editorUI.Run();
                break;

            case 3:
                Environment.Exit(0);
                break;
        }
    }
}
