using System;
using System.Collections.Generic;
using System.Drawing;

internal static class MazeGenerator
{
    public static BaseElement[,] GenerateMaze(int width, int height, Player player, GameLevel gameLevel)
    {
        BaseElement[,] maze = new BaseElement[height, width];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                maze[y, x] = new Wall();

        Random rnd = new Random();

        int startX = 1;
        int startY = 1;
        maze[startY, startX] = new EmptyTile();

        List<(int x, int y)> walls = new List<(int, int)>();
        AddWalls(startX, startY, maze, walls, width, height);

        while (walls.Count > 0)
        {
            var index = rnd.Next(walls.Count);
            var (wallx, wally) = walls[index];
            walls.RemoveAt(index);

            var directions = new (int dx, int dy)[]
            {
                (0, -1), (0, 1), (-1, 0), (1, 0)
            };

            int passableNeighbors = 0;
            int px = 0, py = 0;

            foreach (var (dx, dy) in directions)
            {
                int nx = wallx + dx;
                int ny = wally + dy;

                if (nx >= 0 && ny >= 0 && nx < width && ny < height &&
                    maze[ny, nx].IsPassable)
                {
                    passableNeighbors++;
                    px = wallx - dx;
                    py = wally - dy;
                }
            }

            if (passableNeighbors == 1 &&
                px >= 0 && py >= 0 && px < width && py < height &&
                !maze[py, px].IsPassable)
            {
                maze[wally, wallx] = new EmptyTile();
                maze[py, px] = new EmptyTile();
                AddWalls(px, py, maze, walls, width, height);
            }
        }

        player.SetPosition(startX, startY);
        maze[startY, startX] = player;

        var (minLvl, maxLvl) = gameLevel.GetEnemyLevelRange();
        int totalEnemies = rnd.Next(4, 7);
        int placed = 0;

        while (placed < totalEnemies)
        {
            int enemyx = rnd.Next(width);
            int enemyy = rnd.Next(height);

            if (maze[enemyy, enemyx].IsPassable &&
                (Math.Abs(enemyx - startX) + Math.Abs(enemyy - startY) > 3))
            {
                string symbol = rnd.NextDouble() < 0.3 ? "T" : "E";
                bool isTank = symbol == "T";
                var stats = new EnemyStats(rnd.Next(minLvl, maxLvl + 1), isTank);
                maze[enemyy, enemyx] = new Enemy(symbol, Color.Red, Color.Black, stats);
                placed++;
            }
        }

        return maze;
    }

    private static void AddWalls(int x, int y, BaseElement[,] maze, List<(int, int)> walls, int width, int height)
    {
        foreach (var (dx, dy) in new (int dx, int dy)[] { (0, -2), (0, 2), (-2, 0), (2, 0) })
        {
            int nx = x + dx;
            int ny = y + dy;
            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 &&
                !maze[ny, nx].IsPassable)
            {
                int wallx = x + dx / 2;
                int wally = y + dy / 2;
                if (!walls.Contains((wallx, wally)))
                    walls.Add((wallx, wally));
            }
        }
    }
}
