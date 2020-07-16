using FactoryGame.IsometricMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Tiled;
using System;
using System.Collections.Generic;

namespace FactoryGame.IsometricMap
{
    public class Map : IDisposable
    {
        public int Width;
        public int Height;
        public int WorldWidth => Width * TileWidth;
        public int WorldHeight => Height * TileHeight;
        public int TileWidth;
        public int TileHeight;
		public Vector2 TileSize => new Vector2(TileWidth, TileHeight);

		public bool RequiresLargeTileCulling => MaxTileWidth > TileWidth || MaxTileHeight > TileHeight;

		/// <summary>
		/// when we have an image tileset, tiles can be any size so we record the max size for culling
		/// </summary>
		public int MaxTileWidth;

		/// <summary>
		/// when we have an image tileset, tiles can be any size so we record the max size for culling
		/// </summary>
		public int MaxTileHeight;

		public List<Tileset> Tilesets;

		public List<Layer> Layers;

		public void Update()
        {
            foreach (var tileset in Tilesets)
                tileset.Update();
        }

        #region IDisposable Support

        bool _isDisposed;

        void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    foreach (var tileset in Tilesets)
                        tileset.Image?.Dispose();
                }

                _isDisposed = true;
            }
        }

        void IDisposable.Dispose() => Dispose(true);
		#endregion


		public void RenderTile(Tile tile, Batcher batcher, Vector2 position, Vector2 scale, float tileWidth, float tileHeight, Color color, float layerDepth)
		{
			var gid = tile.Gid;

			// animated tiles (and tiles from image tilesets) will be inside the Tileset itself, in separate TmxTilesetTile
			// objects, not to be confused with TmxLayerTiles which we are dealing with in this loop
			var tilesetTile = tile.TilesetTile;
			if (tilesetTile != null && tilesetTile.AnimationFrames.Count > 0)
				gid = tilesetTile.currentAnimationFrameGid;

			var sourceRect = tile.Tileset.TileRegions[gid];

			// for the y position, we need to take into account if the tile is larger than the tileHeight and shift. Tiled uses
			// a bottom-left coordinate system and MonoGame a top-left
			var tileworldpos = isometricTileToWorldPosition(tile.X, tile.Y);
			var tx = tileworldpos.X;
			var ty = tileworldpos.Y;
			var rotation = 0f;

			var spriteEffects = SpriteEffects.None;
			if (tile.HorizontalFlip)
				spriteEffects |= SpriteEffects.FlipHorizontally;
			if (tile.VerticalFlip)
				spriteEffects |= SpriteEffects.FlipVertically;
			if (tile.DiagonalFlip)
			{
				if (tile.HorizontalFlip && tile.VerticalFlip)
				{
					spriteEffects ^= SpriteEffects.FlipVertically;
					rotation = MathHelper.PiOver2;
					tx += tileHeight + (sourceRect.Height * scale.Y - tileHeight);
					ty -= (sourceRect.Width * scale.X - tileWidth);
				}
				else if (tile.HorizontalFlip)
				{
					spriteEffects ^= SpriteEffects.FlipVertically;
					rotation = -MathHelper.PiOver2;
					ty += tileHeight;
				}
				else if (tile.VerticalFlip)
				{
					spriteEffects ^= SpriteEffects.FlipHorizontally;
					rotation = MathHelper.PiOver2;
					tx += tileWidth + (sourceRect.Height * scale.Y - tileHeight);
					ty += (tileWidth - sourceRect.Width * scale.X);
				}
				else
				{
					spriteEffects ^= SpriteEffects.FlipHorizontally;
					rotation = -MathHelper.PiOver2;
					ty += tileHeight;
				}
			}

			// if we had no rotations (diagonal flipping) shift our y-coord to account for any non map.tileSize tiles due to
			// Tiled being bottom-left origin
			if (rotation == 0)
				ty += (tileHeight - sourceRect.Height * scale.Y);

			var pos = new Vector2(tx, ty) + position;

			batcher.Draw(tile.Tileset.Image.Texture, pos, sourceRect, color, rotation, Vector2.Zero, scale, spriteEffects, layerDepth);
		}

		public void RenderLayer(Layer layer, Batcher batcher, Vector2 position, Vector2 scale, float layerDepth, RectangleF cameraClipBounds)
		{
			if (!layer.Visible)
				return;

			position += layer.Offset;

			// offset it by the entity position since the tilemap will always expect positions in its own coordinate space
			cameraClipBounds.Location -= position;

			var tileWidth = layer.Map.TileWidth * scale.X;
			var tileHeight = layer.Map.TileHeight * scale.Y;

			int minX, minY, maxX, maxY;
			if (layer.Map.RequiresLargeTileCulling)
			{
				// we expand our cameraClipBounds by the excess tile width/height of the largest tiles to ensure we include tiles whose
				// origin might be outside of the cameraClipBounds
				minX = layer.Map.WorldToTilePositionX(cameraClipBounds.Left - (layer.Map.MaxTileWidth * scale.X - tileWidth));
				minY = layer.Map.WorldToTilePositionY(cameraClipBounds.Top - (layer.Map.MaxTileHeight * scale.Y - tileHeight));
				maxX = layer.Map.WorldToTilePositionX(cameraClipBounds.Right + (layer.Map.MaxTileWidth * scale.X - tileWidth));
				maxY = layer.Map.WorldToTilePositionY(cameraClipBounds.Bottom + (layer.Map.MaxTileHeight * scale.Y - tileHeight));
			}
			else
			{
				minX = layer.Map.WorldToTilePositionX(cameraClipBounds.Left);
				minY = layer.Map.WorldToTilePositionY(cameraClipBounds.Top);
				maxX = layer.Map.WorldToTilePositionX(cameraClipBounds.Right);
				maxY = layer.Map.WorldToTilePositionY(cameraClipBounds.Bottom);
			}

			var color = Color.White;
			color.A = (byte)(layer.Opacity * 255);

			// loop through and draw all the non-culled tiles
			for (var y = minY; y <= maxY; y++)
			{
				for (var x = minX; x <= maxX; x++)
				{
					var tile = layer.GetTile(x, y);
					if (tile != null)
						RenderTile(tile, batcher, position, scale, tileWidth, tileHeight, color, layerDepth);
				}
			}
		}

		#region world/local conversion

		/// <summary>
		/// converts from world to tile position clamping to the tilemap bounds
		/// </summary>
		/// <returns>The to tile position.</returns>
		/// <param name="pos">Position.</param>
		public Point WorldToTilePosition(Vector2 pos, bool clampToTilemapBounds = true)
		{
			return new Point(WorldToTilePositionX(pos.X, clampToTilemapBounds), WorldToTilePositionY(pos.Y, clampToTilemapBounds));
		}

		/// <summary>
		/// converts from world to tile position clamping to the tilemap bounds
		/// </summary>
		/// <returns>The to tile position x.</returns>
		/// <param name="x">The x coordinate.</param>
		public int WorldToTilePositionX(float x, bool clampToTilemapBounds = true)
		{
			var tileX = Mathf.FastFloorToInt(x / TileWidth);
			if (!clampToTilemapBounds)
				return tileX;
			return Mathf.Clamp(tileX, 0, Width - 1);
		}

		/// <summary>
		/// converts from world to tile position clamping to the tilemap bounds
		/// </summary>
		/// <returns>The to tile position y.</returns>
		/// <param name="y">The y coordinate.</param>
		public int WorldToTilePositionY(float y, bool clampToTilemapBounds = true)
		{
			var tileY = Mathf.FloorToInt(y / TileHeight);
			if (!clampToTilemapBounds)
				return tileY;
			return Mathf.Clamp(tileY, 0, Height - 1);
		}

		/// <summary>
		/// converts from tile to world position
		/// </summary>
		/// <returns>The to world position.</returns>
		/// <param name="pos">Position.</param>
		public Vector2 TileToWorldPosition(Vector2 pos) => new Vector2(TileToWorldPositionX((int)pos.X), TileToWorldPositionY((int)pos.Y));

		/// <summary>
		/// converts from tile to world position
		/// </summary>
		/// <returns>The to world position x.</returns>
		/// <param name="x">The x coordinate.</param>
		public int TileToWorldPositionX(int x) => x * TileWidth;

		/// <summary>
		/// converts from tile to world position
		/// </summary>
		/// <returns>The to world position y.</returns>
		/// <param name="y">The y coordinate.</param>
		public int TileToWorldPositionY(int y) => y * TileHeight;

		public Vector2 isometricTileToWorldPosition(Point pos)
		{
			return isometricTileToWorldPosition(pos.X, pos.Y);
		}

		/// <summary>
		/// converts from world to tile position for isometric map clamping to the tilemap bounds
		/// </summary>
		/// <returns>The to tile position.</returns>
		/// <param name="pos">Position.</param>
		public Point isometricWorldToTilePosition(Vector2 pos, bool clampToTilemapBounds = true)
		{
			return isometricWorldToTilePosition(pos.X, pos.Y, clampToTilemapBounds);
		}

		/// <summary>
		/// converts from world to tile position for isometric map clamping to the tilemap bounds
		/// </summary>
		/// <returns>The to tile position.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Point isometricWorldToTilePosition(float x, float y, bool clampToTilemapBounds = true)
		{
			x -= (Height - 1) * TileWidth/ 2;
			var tileX = Mathf.FastFloorToInt((y / TileHeight) + (x / TileWidth));
			var tileY = Mathf.FastFloorToInt((-x / TileWidth) + (y / TileHeight));
			if (!clampToTilemapBounds) { 
				return new Point(tileX, tileY);
			}
			return new Point(Mathf.Clamp(tileX, 0, Width - 1), Mathf.Clamp(tileY, 0, Height - 1));
		}

		/// <summary>
		/// converts from isometric tile to world position
		/// </summary>
		/// <returns>The to world position.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Vector2 isometricTileToWorldPosition(int x, int y)
		{
			var worldX = x * TileWidth / 2 - y * TileWidth / 2 + (Height - 1) * TileWidth / 2;
			var worldY = y * TileHeight / 2 + x * TileHeight / 2;
			return new Vector2(worldX, worldY);
		}

		#endregion

		#region Tileset and Layer getters

		/// <summary>
		/// gets the TiledTileset for the given tileId
		/// </summary>
		/// <returns>The tileset for tile identifier.</returns>
		/// <param name="gid">Identifier.</param>
		public Tileset GetTilesetForTileGid(int gid)
		{
			if (gid == 0)
				return null;
				
			for (var i = Tilesets.Count - 1; i >= 0; i--)
			{
				if (Tilesets[i].FirstGid <= gid)
					return Tilesets[i];
			}

			throw new Exception(string.Format("tile gid {0} was not found in any tileset", gid));
		}

		/// <summary>
		/// returns the TmxTilesetTile for the given id or null if none exists. TmxTilesetTiles exist only for animated tiles
		/// and tiles with properties set.
		/// </summary>
		/// <returns>The tileset tile.</returns>
		/// <param name="gid">Identifier.</param>
		public TilesetTile GetTilesetTile(int gid)
		{
			for (var i = Tilesets.Count - 1; i >= 0; i--)
			{
				if (Tilesets[i].FirstGid <= gid)
				{
					if (Tilesets[i].Tiles.TryGetValue(gid - Tilesets[i].FirstGid, out var tilesetTile))
						return tilesetTile;
				}
			}

			return null;
		}

		/// <summary>
		/// gets the ITmxLayer by index
		/// </summary>
		/// <returns>The layer.</returns>
		/// <param name="index">Index.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetLayer<T>(int index) where T : Layer => (T)Layers[index];

		#endregion
	}
}
