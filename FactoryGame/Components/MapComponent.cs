using FactoryGame.Scenes;
using FactoryGame.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Tiled;
using System.Collections.Generic;

namespace FactoryGame.Components
{
    public class MapComponent : RenderableComponent, IUpdatable
    {
        int _mapWidth;
        int _mapHeight;
        Entity[][] mapEntities;
        List<BaseTile> tiles = new List<BaseTile>();
        int TileWidth = 64;
        int TileHeight = 32;

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    if (tiles.Count > 0)
                    {
                        float width = _mapWidth * 64;
                        float height = _mapHeight * 32;
                        _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, new Vector2(width / 2, height / 2),
                            Entity.Transform.Scale, Entity.Transform.Rotation, width, height);
                    }
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public MapComponent(int width, int heigth)
        {
            this._mapWidth = width;
            this._mapHeight = heigth;
            initializeMap(this._mapWidth, this._mapHeight);
        }
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            for (int x = 0; x < this._mapWidth; x++)
            {
                for (int y = 0; y < this._mapHeight; y++)
                {
                    
                    Grass tile = new Grass(x, y);
                    tile.loadContent(Entity.Scene.Content);
                    tile.parent = this;
                    tiles.Add(tile);
                }
            }
            tiles.Sort();
        }

        public void initializeMap(int width, int height)
        {
            mapEntities = new Entity[width][];
            for (int x = 0; x < width; x++)
            {
                mapEntities[x] = new Entity[height];
            }
        }

        public override void Render(Batcher batcher, Camera camera)
        {

            foreach ( BaseTile tile in tiles)
            {
                batcher.Draw(tile.sprite, tile.localPosition);
            }
        }

        public Entity GetEntityOnCoordenate(int x, int y)
        {
            return mapEntities[x][y];
        }
        
        public Entity RemoveEntityOnCoordenate(int x, int y)
        {
            Entity entity = mapEntities[x][y];
            mapEntities[x][y] = null;
            return entity;
        }
        
        public override void DebugRender(Batcher batcher)
        {
            base.DebugRender(batcher);

            foreach (BaseTile tile in tiles)
            {
                Debug.DrawText(Graphics.Instance.BitmapFont, string.Format("{0}, {1}", tile.coordX, tile.coordY), tile.Center, Color.Black);
            }
        }

        private Vector2 cartesianToIsometric(int x, int y)
        {
            return new Vector2(x - y, (x + y) / 2);
        }
        private Vector2 isometricToCartesian(Vector2 isoPt)
        {
            return new Vector2((2 * isoPt.Y + isoPt.X) / 2, (2 * isoPt.Y - isoPt.X) / 2);
        }

        public void Update()
        {
            var pos = Entity.Scene.Camera.MouseToWorldPoint();
            Debug.DrawText(Graphics.Instance.BitmapFont, string.Format("{0}", WorldToTilePosition(pos)), pos, Color.Black);
        }

        public Point? WorldToTilePosition(Vector2 pos, bool clampToTilemapBounds = true)
        {
            if (!Bounds.Contains(pos.X, pos.Y))
            {
                return null;
            }
            Vector2 npos = cartesianToIsometric((int)(pos.X), (int)(pos.Y));
            Point point = new Point();
            point.X = Mathf.FastFloorToInt(((npos.Y * 2) - ((Bounds.Height * TileWidth) / 2) + npos.X) / 2) / TileWidth;
            point.Y = Mathf.FastFloorToInt(npos.X - point.X) / TileHeight;
            return point;
        }
    }
}
