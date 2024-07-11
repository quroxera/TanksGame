namespace TanksGame.Map.Maze
{
    public interface IMazeAlgorithm
    {
        bool[,] Generate(int width, int height);
    }
}
