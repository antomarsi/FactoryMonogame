using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.IsometricMap
{
    public class TileImage : IDisposable
    {
        public Texture2D Texture;
        public int Width;
        public int Height;
        public Vector2 offset;

        public void Dispose()
        {
            if (Texture != null)
            {
                Texture.Dispose();
                Texture = null;
            }
        } 
    }
}
