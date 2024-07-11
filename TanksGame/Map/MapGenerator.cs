using TanksGame.Base;
using TanksGame.Map.Maze;
using TanksGame;

internal class MapGenerator
{
    public static MazeGenerator? _mazeGenerator;
    public Cell[,]? map { get; private set; }
    private const int _waterChance = 30;
    public static Random _rndm = new Random();
    public int mazeWidth = 10;
    public int mazeHeight = 10;

    public void GenerateMaze()
    {
        _mazeGenerator = new MazeGenerator(new MazeAlgorithm());
        var boolMap = _mazeGenerator?.Generate(mazeWidth, mazeHeight);

        map = new Cell[mazeWidth, mazeHeight];

        for (int y = 0; y < mazeHeight; y++)
        {
            for (int x = 0; x < mazeWidth; x++)
            {
                CellType type;

                if (IsBorderCell(x, y, mazeWidth, mazeHeight))
                {
                    type = CellType.Wall;
                }
                else if (boolMap != null && boolMap[x, y])
                {
                    type = CellType.Road;
                }
                else
                {
                    type = (_rndm.Next(100) <= _waterChance) ? CellType.Water : CellType.Wall;
                }

                map[x, y] = new Cell(x, y, type);
            }
        }
    }

    public byte GetCellColor(CellType type)
    {
        switch (type)
        {
            case CellType.Road:
                return 3;
            case CellType.Wall:
                return 0;
            case CellType.DamagedWall:
                return 0;
            case CellType.Water:
                return 1;
            case CellType.Tank:
                return 3;
            default:
                return 0;
        }
    }

    private static bool IsBorderCell(int x, int y, int width, int height)
    {
        return x == 0 || y == 0 || x == width - 1 || y == height - 1;
    }

    public void DamageWall(int x, int y)
    {
        if (map == null) return;

        if (map[x, y].Type == CellType.Wall)
        {
            map[x, y].Type = CellType.DamagedWall;
        }
        else if (map[x, y].Type == CellType.DamagedWall)
        {
            map[x, y].Type = CellType.Road;
        }
    }
}
