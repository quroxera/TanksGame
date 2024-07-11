using System.Text;
using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.Units;
using static TanksGame.Tanks.TanksGameplayState;

namespace TanksGame.Tanks.GameObjects
{
    internal class Projectile : IGameObject
    {
        public Cell Position { get; private set; }
        public TankDir Direction { get; private set; }
        public Tank Owner { get; private set; }
        private string _bulletView = "•";

        private GameMap _gameMap;
        private List<IGameObject> _objects;

        private GameObjectsSystem _gameObjectsSystem;

        public Projectile(Tank owner, Cell startPosition, TankDir direction, GameMap map, List<IGameObject> objects, GameObjectsSystem gameObjectsSystem)
        {
            Owner = owner;
            Position = startPosition;
            Direction = direction;
            _gameMap = map;
            _objects = objects;
            _gameObjectsSystem = gameObjectsSystem;
        }

        public void Update(float deltaTime)
        {
            Move();
        }

        public void Draw(ConsoleRenderer renderer)
        {
            if (Position.X >= 0 && Position.X < renderer.width && Position.Y >= 0 && Position.Y < renderer.height)
            {
                Console.OutputEncoding = Encoding.Unicode;
                renderer.SetPixel(Position.X * Cell.Width + 1, Position.Y * Cell.Height+1, _bulletView, Owner.TankColor);
            }
        }

        public void Move()
        {
            if (Position.X >= _gameMap.GetWidth() || Position.Y >= _gameMap.GetHeight()
                || Position.X < 0 || Position.Y < 0)
            {
                _gameObjectsSystem.RemoveObject(this);
            }
            Cell newPosition = ShiftTo(Position, Direction);
            if (newPosition.X < _gameMap.GetWidth() && newPosition.Y < _gameMap.GetHeight()
                && newPosition.X >= 0 && newPosition.Y >= 0)
            {
                var obstacle = _gameMap.GetCellType(newPosition.X, newPosition.Y);

                if (obstacle == CellType.Wall || obstacle == CellType.DamagedWall)
                {
                    _gameMap.DamageWall(newPosition.X, newPosition.Y);
                    _gameObjectsSystem.RemoveObject(this);
                    return;
                }


                else if (obstacle == CellType.Tank)
                {
                    _gameObjectsSystem.RemoveObject(this);

                    foreach (var entity in _objects)
                    {
                        if (entity is Tank tank && tank.Position.X == newPosition.X && tank.Position.Y == newPosition.Y)
                        {
                            if (tank != Owner)  // Проверка на то, что танк не является владельцем снаряда
                            {
                                tank.TakeDamage(1);
                            }
                            _gameObjectsSystem.RemoveObject(this);
                            break;
                        }
                    }
                    return;
                }
            }
            else
            {
                _gameObjectsSystem.RemoveObject(this);
            }

            Position = newPosition;
        }
    }
}
