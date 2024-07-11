using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TanksGame.Tanks.GameObjects
{
    internal class TankView
    {
        public static readonly string[] Up =
        {
            " ╥ ",
            "╔═╗",
            "╚═╝"
        };

        public static readonly string[] Down =
        {
            "╔═╗",
            "╚═╝",
            " ╨ "
        };

        public static readonly string[] Left =
        {
            " ╔╗",
            "═ ║",
            " ╚╝"
        };

        public static readonly string[] Right =
        {
            "╔╗ ",
            "║ ═",
            "╚╝ "
        };
    }
}
