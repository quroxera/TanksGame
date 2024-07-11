namespace TanksGame.Base
{
    internal class ConsoleInput
    {
        public interface IArrowListener
        {
            abstract void OnArrowUp();
            abstract void OnArrowDown();
            abstract void OnArrowLeft();
            abstract void OnArrowRight();
            abstract void OnShoot();
        }

        private List<IArrowListener> _arrowListenersList = new();

        public void Subscribe(IArrowListener arrowListener)
        {
            _arrowListenersList.Add(arrowListener);
        }

        public void Update()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        foreach (IArrowListener arrowListener in _arrowListenersList)
                            arrowListener.OnArrowUp();
                        break;
                    case ConsoleKey.DownArrow:
                        foreach (IArrowListener arrowListener in _arrowListenersList)
                            arrowListener.OnArrowDown();
                        break;
                    case ConsoleKey.LeftArrow:
                        foreach (IArrowListener arrowListener in _arrowListenersList)
                            arrowListener.OnArrowLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        foreach (IArrowListener arrowListener in _arrowListenersList)
                            arrowListener.OnArrowRight();
                        break;
                    case ConsoleKey.Enter:
                        foreach (IArrowListener arrowListener in _arrowListenersList)
                            arrowListener.OnShoot();
                        break;
                }
            }
        }
    }
}
