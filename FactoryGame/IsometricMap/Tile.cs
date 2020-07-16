using Microsoft.Xna.Framework;

namespace FactoryGame.IsometricMap
{
    public class Tile
    {
		public Tileset Tileset;
		public int Gid;
		public int X;
		public int Y;
		public Vector2 Position => new Vector2(X, Y);
		public bool HorizontalFlip = false;
		public bool VerticalFlip = false;
		public bool DiagonalFlip = false;

		int? _tilesetTileIndex;

		public TilesetTile TilesetTile
		{
			get
			{
				if (!_tilesetTileIndex.HasValue)
                {
					_tilesetTileIndex = this.Gid;
                }

				return Tileset.Tiles[_tilesetTileIndex.Value];
			}
		}
	}
}
