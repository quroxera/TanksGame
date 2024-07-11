using TanksGame.Tanks.Units;

namespace TanksGame.Tanks.GameObjects
{
    internal class GameObjectsSystem
    {
        private List<IGameObject> _objects;
        private List<IGameObject> _objectsToAdd;
        private List<IGameObject> _objectsToRemove;

        public GameObjectsSystem()
        {
            _objects = new List<IGameObject>();
            _objectsToAdd = new List<IGameObject>();
            _objectsToRemove = new List<IGameObject>();
        }

        public void UpdateObjects(float deltaTime)
        {
            foreach (var obj in _objects)
            {
                obj.Update(deltaTime);
            }
        }

        public void ProcessObjectsChanges()
        {
            if (_objectsToAdd.Count > 0 || _objectsToRemove.Count > 0)
            {
                foreach (var obj in _objectsToAdd)
                {
                    _objects.Add(obj);
                }
                _objectsToAdd.Clear();

                foreach (var obj in _objectsToRemove)
                {
                    _objects.Remove(obj);

                }
                _objectsToRemove.Clear();
            }
        }

        public void AddObject(IGameObject obj)
        {
            _objectsToAdd.Add(obj);
        }

        public void RemoveObject(IGameObject obj)
        {
            _objectsToRemove.Add(obj);

        }

        public List<IGameObject> GetObjects()
        {
            return _objects;
        }

        public void Clear()
        {
            _objects.Clear();
            _objectsToAdd.Clear();
            _objectsToRemove.Clear();
        }
    }
}
