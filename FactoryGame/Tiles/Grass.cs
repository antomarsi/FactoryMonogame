using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.Tiles
{
    public class Grass : BaseTile
    {
        string spritename = "Sprites/tilemap";

        public Grass(int x, int y) : base(x, y) { }
    }
}
