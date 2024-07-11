using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.GameObjects;
using TanksGame.Tanks.Levels;
using static TanksGame.Tanks.TanksGameplayState;

namespace TanksGame.Tanks.Units
{
    internal class Tank : IGameObject
    {
        public enum TankType
        {
            Player,
            Enemy
        }

        private GameMap _gameMap;
        private GameObjectsSystem _gameObjectsSystem;

        public virtual TankType Type { get; private set; }
        public int Health;
        public virtual byte TankColor { get; protected set; }
        public Cell Position { get; set; }
        public TankDir Direction { get; set; }

        public virtual void Update(float deltaTime) { }

        public Tank(Cell startPosition, GameMap gameMap, GameObjectsSystem gameObjectsSystem, LevelSystem levelSystem, int startHealth)
        {
            Position = startPosition;
            _gameMap = gameMap;
            _gameObjectsSystem = gameObjectsSystem;
            Health = startHealth;
        }

        public void Draw(ConsoleRenderer renderer)
        {
            string[] tankShape = GetTankView();

            for (int i = 0; i < tankShape.Length; i++)
            {
                for (int j = 0; j < tankShape[i].Length; j++)
                {
                    int x = Position.X * Cell.Width + j;
                    int y = Position.Y * Cell.Height + i;
                    if (x >= 0 && x < renderer.width && y >= 0 && y < renderer.height)
                    {
                        renderer.SetPixel(x, y, tankShape[i][j].ToString(), TankColor);
                    }
                }
            }
        }

        public virtual void SetDirection(TankDir direction)
        {
            Direction = direction;
        }

        public void Shoot()
        {
            Thread.Sleep(50);
            var projectilePosition = Position;
            var projectile = new Projectile(this, projectilePosition, Direction, _gameMap, _gameObjectsSystem.GetObjects(), _gameObjectsSystem);
            _gameObjectsSystem.AddObject(projectile);
        }

        public void Move()
        {
            var newPosition = ShiftTo(Position, Direction);

            if (newPosition.X >= 0 && newPosition.X < _gameMap.GetWidth() && newPosition.Y >= 0 && newPosition.Y < _gameMap.GetHeight())
            {
                if (_gameMap.IsWalkable(newPosition.X, newPosition.Y, _gameObjectsSystem.GetObjects(), this))
                {
                    _gameMap.UpdateTankPosition(Position, newPosition);
                    Position = new Cell(newPosition.X, newPosition.Y, CellType.Tank);
                }
            }
        }

        private string[] GetTankView()
        {
            switch (Direction)
            {
                case TankDir.Up:
                    return TankView.Up;
                case TankDir.Down:
                    return TankView.Down;
                case TankDir.Left:
                    return TankView.Left;
                case TankDir.Right:
                    return TankView.Right;
                default:
                    return TankView.Up;
            }
        }
        public void TakeDamage(int damage)  
        {
            Health -= damage;
            if (Health <= 0)
            {
                Destroy();

            }
        }
        private void Destroy()
        {
            var map = _gameMap.GetMap();
            map[Position.X, Position.Y].Type = CellType.Road;
            _gameObjectsSystem.RemoveObject(this);
        }
    }
}
