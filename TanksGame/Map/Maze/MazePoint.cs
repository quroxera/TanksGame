namespace TanksGame.Map.Maze
{
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Point p = (Point)obj;
            return X == p.X && Y == p.Y;
        }

        public override int GetHashCode()
        {
            return X + Y;
        }
    }
}