using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.IsometricMap
{
    public class Layer
	{
		public Map Map;
		public string Name { get; set; }
		public float Opacity { get; set; }
		public bool Visible { get; set; }
		public float OffsetX { get; set; }
		public float OffsetY { get; set; }
		public Vector2 Offset => new Vector2(OffsetX, OffsetY);
		public Dictionary<string, string> Properties { get; set; }

		/// <summary>
		/// width in tiles for this layer. Always the same as the map width for fixed-size maps.
		/// </summary>
		public int Width;

		/// <summary>
		/// height in tiles for this layer. Always the same as the map height for fixed-size maps.
		/// </summary>
		public int Height;
		public Tile[] Tiles;

		/// <summary>
		/// returns the TmxLayerTile with gid. This is a slow lookup so cache it!
		/// </summary>
		/// <param name="gid"></param>
		/// <returns></returns>
		public Tile GetTileWithGid(int gid)
		{
			for (var i = 0; i < Tiles.Length; i++)
			{
				if (Tiles[i] != null && Tiles[i].Gid == gid)
					return Tiles[i];
			}
			return null;
		}

		/// <summary>
		/// gets the TmxLayerTile at the x/y coordinates. Note that these are tile coordinates not world coordinates!
		/// </summary>
		/// <returns>The tile.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Tile GetTile(int x, int y) => Tiles[x + y * Width];

		/// <summary>
		/// gets the TmxLayerTile at the given world position
		/// </summary>
		public Tile GetTileAtWorldPosition(Vector2 pos)
		{
			var worldPoint = Map.WorldToTilePosition(pos);
			return GetTile(worldPoint.X, worldPoint.Y);
		}

		/// <summary>
		/// Returns a list of rectangles in tile space, where any non-null tile is combined into bounding regions
		/// </summary>
		public List<Rectangle> GetCollisionRectangles()
		{
			var checkedIndexes = new bool?[Tiles.Length];
			var rectangles = new List<Rectangle>();
			var startCol = -1;
			var index = -1;

			for (var y = 0; y < Map.Height; y++)
			{
				for (var x = 0; x < Map.Width; x++)
				{
					index = y * Map.Width + x;
					var tile = GetTile(x, y);

					if (tile != null && (checkedIndexes[index] == false || checkedIndexes[index] == null))
					{
						if (startCol < 0)
							startCol = x;

						checkedIndexes[index] = true;
					}
					else if (tile == null || checkedIndexes[index] == true)
					{
						if (startCol >= 0)
						{
							rectangles.Add(FindBoundsRect(startCol, x, y, checkedIndexes));
							startCol = -1;
						}
					}
				} // end for x

				if (startCol >= 0)
				{
					rectangles.Add(FindBoundsRect(startCol, Map.Width, y, checkedIndexes));
					startCol = -1;
				}
			}

            return rectangles;
		}

		/// <summary>
		/// Finds the largest bounding rect around tiles between startX and endX, starting at startY and going
		/// down as far as possible
		/// </summary>
		public Rectangle FindBoundsRect(int startX, int endX, int startY, bool?[] checkedIndexes)
		{
			var index = -1;

			for (var y = startY + 1; y < Map.Height; y++)
			{
				for (var x = startX; x < endX; x++)
				{
					index = y * Map.Width + x;
					var tile = GetTile(x, y);

					if (tile == null || checkedIndexes[index] == true)
					{
						// Set everything we've visited so far in this row to false again because it won't be included in the rectangle and should be checked again
						for (var _x = startX; _x < x; _x++)
						{
							index = y * Map.Width + _x;
							checkedIndexes[index] = false;
						}

						return new Rectangle(startX * Map.TileWidth, startY * Map.TileHeight,
							(endX - startX) * Map.TileWidth, (y - startY) * Map.TileHeight);
					}

					checkedIndexes[index] = true;
				}
			}

			return new Rectangle(startX * Map.TileWidth, startY * Map.TileHeight,
				(endX - startX) * Map.TileWidth, (Map.Height - startY) * Map.TileHeight);
		}

		/// <summary>
		/// gets a List of all the TiledTiles that intersect the passed in Rectangle. The returned List can be put back in the pool via ListPool.free.
		/// </summary>
		public List<Tile> GetTilesIntersectingBounds(Rectangle bounds)
		{
			var topLeft = Map.isometricWorldToTilePosition(bounds.X, bounds.Y);
			var bottomLeft = Map.isometricWorldToTilePosition(bounds.X, bounds.Bottom);
			var topRight = Map.isometricWorldToTilePosition(bounds.Right, bounds.Y);
			var bottomRight = Map.isometricWorldToTilePosition(bounds.Right, bounds.Bottom);

			var tilelist = ListPool<Tile>.Obtain();

			for (var x = topLeft.X; x <= bottomRight.X; x++)
			{
				for (var y = topRight.Y; y <= bottomLeft.Y; y++)
				{
					var tile = GetTile(x, y);
					if (tile != null && bounds.Contains(Map.isometricTileToWorldPosition(x, y)))
						tilelist.Add(tile);
				}
			}

			return tilelist;
		}

		/// <summary>
		/// sets the tile and updates its tileset. If you change a tiles gid to one in a different Tileset you must
		/// call this method or update the TmxLayerTile.tileset manually!
		/// </summary>
		/// <returns>The tile.</returns>
		/// <param name="tile">Tile.</param>
		public Tile SetTile(Tile tile)
		{
			Tiles[tile.X + tile.Y * Width] = tile;
			tile.Tileset = Map.GetTilesetForTileGid(tile.Gid);

			return tile;
		}

		/// <summary>
		/// nulls out the tile at the x/y coordinates
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public void RemoveTile(int x, int y) => Tiles[x + y * Width] = null;
	}
}
