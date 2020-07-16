using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Systems;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.Items
{
    public abstract class BaseItem : Entity
    {
        string name;
        int id = 1;
        private Texture2D _texture;
        private string spritePath;

        public BaseItem(string name, int id, string spritePath) : base(name)
        {
            this.name = name;
            this.id = id;
            this.spritePath = spritePath;
        }

        public void loadContent(NezContentManager content)
        {
            _texture = content.Load<Texture2D>(this.spritePath);
            var spriteRenderer = new SpriteRenderer(_texture);
            spriteRenderer.SetRenderLayer(-1);
            this.AddComponent(spriteRenderer);
        }
    }
}
