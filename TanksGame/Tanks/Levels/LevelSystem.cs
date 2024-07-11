using TanksGame.Map;
using TanksGame.Tanks.GameObjects;

namespace TanksGame.Tanks.Levels
{
    internal class LevelSystem
    {
        private List<Level> _levels;
        private int _levelIndex;

        public GameMap _gameMap { get; private set; }
        private Cell[,] _map;
        private Cell _playerStartPosition;
        private List<Cell> _enemiesStartPositions = new();
        public Cell GetPlayerStartPosition() => _playerStartPosition;
        public List<Cell> GetEnemiesStartPositions() => _enemiesStartPositions;

        public LevelSystem(GameMap gameMap)
        {
            _gameMap = gameMap;
            SetLevelData();
            _levels = new List<Level>
            {
                new Level(1, _playerStartPosition, _enemiesStartPositions),
                new Level(2, _playerStartPosition, _enemiesStartPositions),
                new Level(3, _playerStartPosition, _enemiesStartPositions),
                new Level(4, _playerStartPosition, _enemiesStartPositions),
                new Level(5, _playerStartPosition, _enemiesStartPositions)
            };

        }

        public void SetLevelData()
        {
            _gameMap.NewMap();
            _map = _gameMap.GetMap();
            SetPlayerStartPosition();
            SetEnemiesStartPositions();
        }

        public bool IsLastLevel() => _levelIndex == _levels.Count - 1;

        public Level GetCurrentLevel()
        {
            return _levels[_levelIndex];
        }

        public void NextLevel()
        {
            if (_levelIndex >= _levels.Count - 1)
            {
                ResetLevel();
            }
            else
            {
                _levelIndex++;
                SetLevelData();
            }
        }

        public void ResetLevel()
        {
            _levelIndex = 0;
            SetLevelData();
        }


        public void SetPlayerStartPosition()
        {
            _playerStartPosition = null;
            bool positionSet = false;

            for (int y = _gameMap.GetHeight() - 1; y >= 0 && !positionSet; y--)
            {
                for (int x = _gameMap.GetWidth() - 1; x >= 0 && !positionSet; x--)
                {
                    if (_map[x, y].Type == CellType.Road)
                    {
                        _playerStartPosition = new Cell(x, y, CellType.Tank);
                        positionSet = true;
                    }
                }
            }
            if (!positionSet)
            {
                throw new InvalidOperationException("No suitable start position found.");
            }
        }

        public void SetEnemiesStartPositions()
        {
            _enemiesStartPositions.Clear();
            Random random = new Random();
            for (int i = 0; i <= _levelIndex; i++)
            {
                bool positionSet = false;

                for (int y = random.Next(0, _gameMap.GetHeight() / 2); y < _gameMap.GetHeight() && !positionSet; y++)
                {
                    for (int x = random.Next(0, _gameMap.GetWidth()); x < _gameMap.GetWidth() && !positionSet; x++)
                    {
                        if (_map[x, y].Type == CellType.Road)
                        {
                            _enemiesStartPositions.Add(new Cell(x, y, CellType.Tank));
                            positionSet = true;
                        }
                    }
                }
                if (!positionSet)
                {
                    throw new InvalidOperationException("No suitable start position found.");
                }
            }
        }
    }
}
