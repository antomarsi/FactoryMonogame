using FactoryGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using Nez.Textures;

namespace FactoryGame.Entities
{
    class Belt : Nez.Entity
    {
        public Belt() : base("belt")
        {
            setupComponents();
        }

        public void setupComponents()
        {
            this.AddComponent(new Components.BeltComponent());
        }

        public void onAddedToMap()
        {

        }
    }
}
