using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Systems;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.Items
{
    public abstract class BaseItem
    {
        string name;
        int id = 1;
        private Texture2D _texture;
        public Sprite sprite;
        private string spritePath;

        public BaseItem(string name, int id, string spritePath)
        {
            this.name = name;
            this.id = id;
            this.spritePath = spritePath;
        }

        public void loadContent(NezContentManager content)
        {
            _texture = content.Load<Texture2D>(this.spritePath);
            sprite = new Sprite(_texture);
        }

        public void unloadContent()
        {
            _texture.Dispose();
        }

        public void Render(Batcher batcher, Vector2 position)
        {
            batcher.Draw(this.sprite, position);
        }
    }
}
