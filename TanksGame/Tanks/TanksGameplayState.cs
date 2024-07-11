using System.Drawing;
using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.GameObjects;
using TanksGame.Tanks.Levels;
using TanksGame.Tanks.Units;
using static System.Net.Mime.MediaTypeNames;

namespace TanksGame.Tanks
{
    internal class TanksGameplayState : BaseGameState
    {
        public enum TankDir
        {
            Up,
            Down,
            Right,
            Left
        }

        private const float MoveInterval = 1 / 4f;

        private float _timeToMove = 0f;
        private GameObjectsSystem _gameObjectsSystem;
        private LevelSystem _levelsSystem;
        public GameMap _gameMap;
        private TankDir _playerStartDirection = TankDir.Up;
        private Level _currentLevel;

        public PlayerTank _playerTank;
        public int fieldWidth { get; set; }
        public int fieldHeight { get; set; }
        public bool gameOver { get; private set; }
        public bool hasWon { get; private set; }

        public TanksGameplayState(GameObjectsSystem gameObjectsSystem, LevelSystem levelSystem, GameMap gameMap)
        {
            _gameObjectsSystem = gameObjectsSystem;
            _levelsSystem = levelSystem;
            _gameMap = gameMap;
        }

        public void SetDirection(TankDir direction)
        {
            if (_playerTank != null)
            {
                _playerTank.SetDirection(direction);
            }
        }

        public override void Reset()
        {
            gameOver = false;
            hasWon = false;

            _currentLevel = _levelsSystem.GetCurrentLevel();

            _gameObjectsSystem.Clear();
            if (_playerTank == null)
                _playerTank = new PlayerTank(_levelsSystem.GetPlayerStartPosition(), _gameMap, _gameObjectsSystem, _levelsSystem);

            _playerTank.SetDirection(_playerStartDirection);
            _gameObjectsSystem.AddObject(_playerTank);

            foreach (Cell enemyPosition in _levelsSystem.GetEnemiesStartPositions())
            {
                var enemyTank = new EnemyTank(enemyPosition, _gameMap, _gameObjectsSystem, _levelsSystem);
                _gameObjectsSystem.AddObject(enemyTank);
            }

            _timeToMove = 0f;
        }

        public override void Draw(ConsoleRenderer renderer)
        {
            _gameMap.Draw(renderer);

            renderer.DrawString($"Level: {_levelsSystem.GetCurrentLevel().LevelNumber}", _gameMap.GetWidth() * Cell.Width + 1, 0, ConsoleColor.White);
            renderer.DrawString($"HP: {_playerTank.Health}", _gameMap.GetWidth() * Cell.Width + 1, 2, ConsoleColor.White);
            renderer.DrawString($"Enemies: {_gameObjectsSystem.GetObjects().OfType<EnemyTank>().Count()}", _gameMap.GetWidth() * Cell.Width + 1, 3, ConsoleColor.White);

            foreach (var obj in _gameObjectsSystem.GetObjects())
            {
                obj.Draw(renderer);
            }
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;

            if (_timeToMove > 0f || gameOver)
                return;

            _timeToMove = MoveInterval;

            _gameObjectsSystem.UpdateObjects(deltaTime);
            _gameObjectsSystem.ProcessObjectsChanges();

            if (!_gameObjectsSystem.GetObjects().Contains(_playerTank))
            {
                gameOver = true;
                _levelsSystem.ResetLevel();
                _playerTank.ResetHealth();
            }

            if (_gameObjectsSystem.GetObjects().OfType<EnemyTank>().Count() <= 0)
            {
                hasWon = true;
                if (_levelsSystem.IsLastLevel())
                {
                    _levelsSystem.ResetLevel();
                }
                else
                {
                    _levelsSystem.NextLevel();
                    _playerTank.Position = _levelsSystem.GetPlayerStartPosition();
                }

            }
        }

        public static Cell ShiftTo(Cell from, TankDir dir)
        {
            int newX = from.X;
            int newY = from.Y;
            switch (dir)
            {
                case TankDir.Up:
                    newY -= 1;
                    break;
                case TankDir.Down:
                    newY += 1;
                    break;
                case TankDir.Right:
                    newX += 1;
                    break;
                case TankDir.Left:
                    newX -= 1;
                    break;
            }

            return new Cell(newX, newY, from.Type);
        }
        public override bool IsDone()
        {
            return gameOver || hasWon;
        }

    }
}
