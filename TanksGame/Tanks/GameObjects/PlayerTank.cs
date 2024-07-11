using System.Reflection.Metadata.Ecma335;
using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.GameObjects;
using TanksGame.Tanks.Levels;
using static TanksGame.Tanks.TanksGameplayState;

namespace TanksGame.Tanks.Units
{
    internal class PlayerTank : Tank
    {
        public override TankType Type => TankType.Player;
        public override byte TankColor => 2;

        public PlayerTank(Cell startPosition, GameMap gameMap, GameObjectsSystem gameObjectsSystem, LevelSystem levelSystem)
            : base(startPosition, gameMap, gameObjectsSystem, levelSystem, 3) { }

        public void ResetHealth()
        {
            Health = 3;
        }
    }
}
