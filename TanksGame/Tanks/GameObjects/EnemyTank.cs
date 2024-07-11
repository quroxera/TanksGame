using System.Runtime.CompilerServices;
using TanksGame.Map;
using TanksGame.Tanks.GameObjects;
using TanksGame.Tanks.Levels;
using static TanksGame.Tanks.TanksGameplayState;

namespace TanksGame.Tanks.Units
{
    internal class EnemyTank : Tank
    {
        public override TankType Type => TankType.Enemy;
        public override byte TankColor => 4;
        private Random _random;

        private float shootInterval = 1/10f;
        private float timeSinceLastShot = 0.0f;
        private GameObjectsSystem _gameObjectsSystem;
        private GameMap _gameMap;
        public EnemyTank(Cell startPosition, GameMap gameMap, GameObjectsSystem gameObjectsSystem, LevelSystem levelSystem)
                    : base(startPosition, gameMap, gameObjectsSystem, levelSystem, 2)
        {
            _gameMap = gameMap;
            _gameObjectsSystem = gameObjectsSystem;
        }

        public override void Update(float deltaTime)
        {
            timeSinceLastShot += deltaTime;

            if (CanSeePlayer())
            {
                if (timeSinceLastShot >= shootInterval)
                {
                    Shoot();
                    timeSinceLastShot = 0.0f;
                }
            }
            else
            {
                Cell newPosition = ShiftTo(Position, Direction);
                if (_gameMap.IsWalkable(newPosition.X, newPosition.Y, _gameObjectsSystem.GetObjects(), this))
                {
                    Move();

                }
                else
                {
                    ChangeDirection();
                    Move();
                }
            }
        }

        private void ChangeDirection()
        {
            _random = new Random();
            var directions = Enum.GetValues(typeof(TankDir)).Cast<TankDir>().ToList();
            directions.Remove(Direction);
            Direction = directions[_random.Next(directions.Count)];
        }

        private bool CanSeePlayer()
        {
            var direction = Direction;
            var position = Position;

            while (true)
            {

                position = ShiftTo(position, direction);

                if (position.X < 0 || position.X >= _gameMap.GetWidth() || position.Y < 0 || position.Y >= _gameMap.GetHeight())
                {
                    return false;
                }

                var obstacle = _gameMap.GetCellType(position.X, position.Y);

                if (obstacle == CellType.Wall || obstacle == CellType.DamagedWall)
                {
                    return false;
                }

                if (obstacle == CellType.Tank)
                {
                    foreach (var entity in _gameObjectsSystem.GetObjects())
                    {
                        if (entity is Tank playerTank && playerTank.Type == TankType.Player)
                        {
                            if (playerTank.Position.X == position.X && playerTank.Position.Y == position.Y)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
    }
}
