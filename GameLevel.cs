public class GameLevel
{
    public int LevelNumber { get; private set; } = 1;

    public void IncreaseLevel()
    {
        LevelNumber++;
    }
    public (int min, int max) GetEnemyLevelRange()
    {
        int min = (LevelNumber - 1) * 2 + 1;
        int max = min + 1;
        return (min, max);
    }
}
