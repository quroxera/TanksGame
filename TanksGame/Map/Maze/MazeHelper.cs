namespace TanksGame.Map.Maze
{
    public static class MazeHelper
    {
        private static Random _random = new Random();

        public static int[,] directions = new int[,]
        {
        {-1, 0 },
        { 1, 0 },
        { 0,-1 },
        { 0, 1 }
        };

        public static bool IsRoad(bool[,] maze, int x, int y)
        {
            return IsPointInMaze(maze, x, y) && maze[y, x] == true;
        }

        public static bool IsPointInMaze(bool[,] maze, int x, int y)
        {
            int height = maze.GetLength(0);
            int width = maze.GetLength(1);
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public static void Shuffle<T>(T[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                int bufIndex = _random.Next(0, data.GetLength(0));
                if (bufIndex == i)
                    continue;

                T bufX = data[bufIndex, 0];
                T bufY = data[bufIndex, 1];

                data[bufIndex, 0] = data[i, 0];
                data[bufIndex, 1] = data[i, 1];

                data[i, 0] = bufX;
                data[i, 1] = bufY;
            }
        }
    }
}
