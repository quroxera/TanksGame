using System.Drawing;

namespace TanksGame.Map.Maze
{
    public class MazeAlgorithm : IMazeAlgorithm
    {
        public bool[,] Generate(int width, int height)
        {
            bool[,] maze = new bool[height, width];

            Random random = new Random();

            int x = random.Next(0, width / 2) * 2;
            int y = random.Next(0, height / 2) * 2;

            HashSet<Point> needConnectPoints = new HashSet<Point> { new Point(x, y) }; 

            while (needConnectPoints.Count > 0)
            {
                var list = needConnectPoints.ToList();
                int randomIndex = random.Next(list.Count);
                var point = list[randomIndex];
                needConnectPoints.Remove(point);

                x = point.X;
                y = point.Y;

                maze[y, x] = true;

                Connect(maze, x, y);
                AddVisitPoints(maze, needConnectPoints, x, y);
            }
            return maze;
        }

        private void Connect(bool[,] maze, int x, int y)
        {
            int[,] directions = MazeHelper.directions;

            MazeHelper.Shuffle(directions);

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int neighbourX = x + directions[i, 0] * 2;
                int neighbourY = y + directions[i, 1] * 2;

                if (MazeHelper.IsRoad(maze, neighbourX, neighbourY))
                {
                    int connectorX = x + directions[i, 0];
                    int connectorY = y + directions[i, 1];
                    maze[connectorY, connectorX] = true;
                    return;
                }
            }
        }

        private void AddVisitPoints(bool[,] maze, HashSet<Point> points, int x, int y)
        {
            if (MazeHelper.IsPointInMaze(maze, x - 2, y) && MazeHelper.IsRoad(maze, x - 2, y) == false)
                points.Add(new Point(x - 2, y));

            if (MazeHelper.IsPointInMaze(maze, x + 2, y) && MazeHelper.IsRoad(maze, x + 2, y) == false)
                points.Add(new Point(x + 2, y));

            if (MazeHelper.IsPointInMaze(maze, x, y - 2) && MazeHelper.IsRoad(maze, x, y - 2) == false)
                points.Add(new Point(x, y - 2));

            if (MazeHelper.IsPointInMaze(maze, x, y + 2) && MazeHelper.IsRoad(maze, x, y + 2) == false)
                points.Add(new Point(x, y + 2));

        }
    }
}
