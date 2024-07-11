namespace TanksGame.Map.Maze
{
    public class MazeGenerator
    {
        private IMazeAlgorithm _generationAlgorithm = new MazeAlgorithm();

        public MazeGenerator(IMazeAlgorithm algorithm)
        {
            _generationAlgorithm = algorithm;
        }

        public bool[,] Generate(int width, int height)
        {
            bool[,] maze = _generationAlgorithm.Generate(width, height);

            return maze;
        }
    }
}