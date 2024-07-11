using TanksGame.Base;
using TanksGame.Map;
using TanksGame.Tanks.GameObjects;
using TanksGame.Tanks.Levels;
using TanksGame.Tanks;
public class EnterPoint
{
    public const float targetFrameTime = 1f / 60f;

    public static void Main()
    {
        GameObjectsSystem gameObjectsSystem = new GameObjectsSystem();
        GameMap gameMap = new GameMap();
        LevelSystem levelSystem = new LevelSystem(gameMap);

        TanksGameLogic tanksGameLogic = new TanksGameLogic(gameObjectsSystem, levelSystem, gameMap);
        var palette = tanksGameLogic.CreatePalette();

        ConsoleRenderer renderer0 = new ConsoleRenderer(palette);
        ConsoleRenderer renderer1 = new ConsoleRenderer(palette);

        ConsoleInput input = new ConsoleInput();
        tanksGameLogic.InitializeInput(input);

        ConsoleRenderer prevRenderer = renderer0;
        ConsoleRenderer currRenderer = renderer1;
        DateTime lastFrameTime = DateTime.Now;

        while (true)
        {
            DateTime frameStartTime = DateTime.Now;
            float deltaTime = (float)(frameStartTime - lastFrameTime).TotalSeconds;
            input.Update();
            tanksGameLogic.DrawNewState(deltaTime, currRenderer);
            lastFrameTime = frameStartTime;

            if (!currRenderer.Equals(prevRenderer))
            {
                currRenderer.Render();
            }
            ConsoleRenderer temp = prevRenderer;
            prevRenderer = currRenderer;
            currRenderer = temp;
            currRenderer.Clear();


            var nextFrameTime = frameStartTime + TimeSpan.FromSeconds(targetFrameTime);
            var endFrameTime = DateTime.Now;

            if (nextFrameTime > endFrameTime)
            {
                Thread.Sleep((int)(nextFrameTime - endFrameTime).TotalMilliseconds);
            }
        }
    }
}
