using FactoryGame.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;

namespace FactoryGame.Tiles
{
    public abstract class BaseTile : IComparable<BaseTile>
    {
        string spritename = "Sprites/tilemap";

        public int _coordX;
        public int _coordY;
        private Vector2 _localOffset = new Vector2(0, 5);

        public Vector2 Center
        {
            get
            {
                return localPosition + sprite.Origin;
            }
            private set { }
        }
        public int coordX
        {
            set
            {
                _coordX = value;
                calculatePosition();
            }
            get
            {
                return _coordX;
            }
        }
        public int coordY
        {
            set
            {
                _coordY = value;
                calculatePosition();
            }
            get
            {
                return _coordY;
            }
        }
        public Sprite sprite { get; set; }

        private Vector2 _localPosition;

        public Vector2 localPosition
        {
            set
            {
                _localPosition = value;
            }
            get
            {
                return _localPosition + parent.Transform.Position - new Vector2(parent.Bounds.Width / 2, 0);
            }
        }

        private RenderableComponent _parent;
        public RenderableComponent parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                calculatePosition();
            }
        }

        public BaseTile(int x, int y)
        {
            coordX = x;
            coordY = y;
        }

        public void loadContent(ContentManager content)
        {
            var texture = content.Load<Texture2D>(spritename);
            sprite = new Sprite(texture);

            sprite.Origin = sprite.Origin - _localOffset;
            calculatePosition();
        }

        private void calculatePosition()
        {
            if (sprite != null)
            {
                _localPosition = new Vector2(
                    (coordY * sprite.Origin.X) + (coordX * sprite.Origin.X),
                    (coordX * sprite.Origin.Y) - (coordY * sprite.Origin.Y)) - new Vector2(0, sprite.Origin.Y);
            }
        }

        public int CompareTo(BaseTile obj)
        {
            int objCoord = obj.coordX - obj.coordY;
            int coord = coordX - coordY;
            if (objCoord > coord)
            {
                return -1;
            }
            else if (objCoord < coord)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
