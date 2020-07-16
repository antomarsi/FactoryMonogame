using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.IsometricMap
{
    public class ExempleMap : Map
    {
        public ExempleMap(ContentManager content)
        {
            this.Height = 5;
            this.Width = 5;
            this.TileHeight = 32;
            this.TileWidth = 64;

            Tileset tileset = new Tileset();
            tileset.FirstGid = 1;
            Texture2D texture = content.Load<Texture2D>("Sprites/tileset");

            TileImage tileImage = new TileImage();
            tileImage.Texture = texture;
            tileImage.Height = texture.Height;
            tileImage.Width = texture.Width;
            tileset.Image = tileImage;

            Tile tileSand = new Tile();
            tileSand.Gid = 3;
            tileSand.Tileset = tileset;

            Tile tileGrass1 = new Tile();
            tileSand.Gid = 1;
            tileSand.Tileset = tileset;

            Tile tileGrass2 = new Tile();
            tileSand.Gid = 2;
            tileSand.Tileset = tileset;

            TilesetTile tilesetGrass = new TilesetTile();
            tilesetGrass.AnimationFrames = new List<AnimationFrame>() {
                new AnimationFrame{
                Gid = 1,
                Duration = 100
            },new AnimationFrame
            {
                Gid = 2,
                Duration = 100
            } };

            Vector2 ts = new Vector2(64, 42);

            tileset.TileRegions = new Dictionary<int, Nez.RectangleF> {
                {1, new Nez.RectangleF(tileGrass1.Position, ts) },
                {2, new Nez.RectangleF(tileGrass2.Position, ts) },
                {3, new Nez.RectangleF(tileSand.Position, ts) }
            };

            tileset.Tiles = new Dictionary<int, TilesetTile>() {
                { 1, tilesetGrass }
            };

            this.Tilesets = new List<Tileset>() { tileset };

            Layer layer = new Layer();

            layer.Map = this;

            layer.Visible = true;
            layer.Name = "teste";
            layer.Width = this.Width;
            layer.Height = this.Height;
            layer.Tiles = new Tile[layer.Width * layer.Height];
            layer.Opacity = 1f;
            layer.OffsetY = 9;

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    Tile tile = new Tile() { Gid = 1, X = x, Y = y };
                    layer.SetTile(tile);
                }
            }

            this.Layers = new List<Layer> { layer };
        }
    }
}
