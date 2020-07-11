using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.Components
{
    class BeltComponent : SpriteRenderer
    {
        public Sprite[] sprites;
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            var texture = Entity.Scene.Content.Load<Texture2D>("Sprites/Belt");
            sprites = Sprite.SpritesFromAtlas(texture, 32, 32).ToArray();
            SetSprite(sprites[0]);
        }

        public void setSpriteIndex(int i)
        {
            SetSprite(sprites[i]);
        }

        public override void DebugRender(Batcher batcher)
        {
            base.DebugRender(batcher);
            batcher.DrawCircle(Transform.Position, 8, Color.Green);
        }
    }
}
