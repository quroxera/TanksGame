using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanksGame.Base;

namespace TanksGame.Tanks.Units
{
    internal interface IGameObject
    {
        void Update(float deltaTime);
        void Draw(ConsoleRenderer renderer);
    }
}
