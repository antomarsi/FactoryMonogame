using FactoryGame.Items;
using FactoryGame.Scenes;
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
    class MinerComponent : SpriteRenderer, IUpdatable
    {

        float cooldown = 1f;
        float cooldownTimer;
        ItemEjectorComponent itemEjector;

        public MinerComponent() : base()
        {
            cooldownTimer = cooldown;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            var texture = Entity.Scene.Content.Load<Texture2D>("Sprites/crystal");
            SetSprite(new Sprite(texture));
            itemEjector = Entity.AddComponent(new ItemEjectorComponent());
        }

        public void Update()
        {
            cooldownTimer -= Time.DeltaTime;
            if (cooldownTimer <= 0)
            {
                spawnItem();
                cooldownTimer = cooldown;
            }
        }

        public void spawnItem()
        {
            if (itemEjector.canAcceptItem())
            {
                itemEjector.AddItem((Entity.Scene as BasicScene).items[0]);
            }
        }
    }
}
