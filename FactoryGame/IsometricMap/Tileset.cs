using Nez;
using System.Collections.Generic;

namespace FactoryGame.IsometricMap
{
    public class Tileset
    {
        public int FirstGid;
        public TileImage Image;
        public Dictionary<int, TilesetTile> Tiles;
        public Dictionary<int, RectangleF> TileRegions;
        public void Update()
        {
            foreach (var kvPair in Tiles)
                kvPair.Value.UpdateAnimatedTiles();
        }
    }
}
