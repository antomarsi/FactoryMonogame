using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.Components
{
    public class MapComponent : RenderableComponent
    {
        Texture2D _texture;
        int _mapWidth;
        int _mapHeight;

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    if (_texture != null)
                        _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                            Entity.Transform.Scale, Entity.Transform.Rotation, _texture.Bounds.Width * _mapWidth,
                            _texture.Bounds.Height * _mapHeight);
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public MapComponent(int width, int heigth)
        {
            this._mapWidth = width;
            this._mapHeight = heigth;
        }

        public override void OnAddedToEntity()
        {
            this._texture = Entity.Scene.Content.Load<Texture2D>("Sprites/Map");
            base.OnAddedToEntity();
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            for (int x = 0; x < this._mapWidth; x++)
                for (int y = 0; y < this._mapHeight; y++)
                    RenderTile(batcher, Vector2.Zero, x * 32 - 16, y * 32 - 16);
        }

        public void RenderTile(Batcher batcher, Vector2 position, float tx, float ty)
        {
            var pos = new Vector2(tx, ty) + position;
            batcher.Draw(_texture, pos, Color.White);
        }
    }
}
