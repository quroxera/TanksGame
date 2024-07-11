using System;
using static TanksGame.Base.ConsoleInput;
using TanksGame.Base;
using TanksGame.Tanks.Levels;
using TanksGame.Map;
using TanksGame.Tanks.GameObjects;
using TanksGame.Tanks.Units;

namespace TanksGame.Tanks
{
    internal class TanksGameLogic : BaseGameLogic, IArrowListener
    {
        private TanksGameplayState _gameplayState;
        private LevelSystem _levelSystem;
        private GameMap _gameMap;
        private PlayerTank _playerTank;
        private ShowTextState _showTextState = new(3f);
        private bool _newGamePending = false;
        private int _currentLevel;

        public TanksGameLogic(GameObjectsSystem gameObjectsSystem, LevelSystem levelSystem, GameMap gameMap)
        {
            _levelSystem = levelSystem;
            _gameMap = gameMap;
            _gameplayState = new TanksGameplayState(gameObjectsSystem, _levelSystem, _gameMap);
        }

        public override void Update(float deltaTime)
        {
            _currentLevel = _levelSystem.GetCurrentLevel().LevelNumber;
            _playerTank = _gameplayState._playerTank;
            if (currentState != null && !currentState.IsDone())
                return;

            if (currentState == null || currentState == _gameplayState && !_gameplayState.gameOver)
                GoToNextLevel();
            else if (currentState == _gameplayState && _gameplayState.gameOver)
                GoToGameOver();
            else if (currentState != _gameplayState && _newGamePending)
                GoToNextLevel();
            else if (currentState != _gameplayState && !_newGamePending)
                GoToGameplay();
        }

        public void GoToGameplay()
        {
            _gameplayState.fieldHeight = screenHeight;
            _gameplayState.fieldWidth = screenWidth;
            ChangeState(_gameplayState);
            _gameplayState.Reset();
        }

        private void GoToGameOver()
        {
            _newGamePending = true;
            _showTextState.text = "GAME OVER";
            ChangeState(_showTextState);
        }
        private void GoToNextLevel()
        {
            _newGamePending = false;
            _showTextState.text = $"LEVEL {_currentLevel}";
            ChangeState(_showTextState);
        }


        public override void OnArrowUp()
        {
            if (IsLoading()) return;
            _gameplayState.SetDirection(TanksGameplayState.TankDir.Up);
            _playerTank.Move();
        }

        public override void OnArrowDown()
        {
            if (IsLoading()) return;
            _gameplayState.SetDirection(TanksGameplayState.TankDir.Down);
            _playerTank.Move();
        }

        public override void OnArrowRight()
        {
            if (IsLoading()) return;
            _gameplayState.SetDirection(TanksGameplayState.TankDir.Right);
            _playerTank.Move();
        }

        public override void OnArrowLeft()
        {
            if (IsLoading()) return;
            _gameplayState.SetDirection(TanksGameplayState.TankDir.Left);
            _playerTank.Move();
        }

        public override void OnShoot()
        {
            if (IsLoading()) return;
            _playerTank.Shoot();
        }

        public override ConsoleColor[] CreatePalette()
        {
            return new ConsoleColor[]
            {
                ConsoleColor.DarkRed,
                ConsoleColor.Blue,
                ConsoleColor.Green,
                ConsoleColor.Black,
                ConsoleColor.White
            };
        }

        private bool IsLoading()
        {
            if (currentState != _gameplayState && _playerTank == null) 
                return true;
            return false;
        }

    }
}