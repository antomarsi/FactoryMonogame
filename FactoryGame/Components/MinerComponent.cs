using FactoryGame.Items;
using FactoryGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace FactoryGame.Components
{
    class MinerComponent : SpriteRenderer, IUpdatable
    {

        float cooldown = 1f;
        float cooldownTimer;

        public MinerComponent() : base()
        {
            cooldownTimer = cooldown;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            var texture = Entity.Scene.Content.Load<Texture2D>("Sprites/Miner");
            SetSprite(new Sprite(texture));
        }
        public void setAcceptor(ItemAcceptorComponent itemAcceptor)
        {
            Entity.GetComponent<ItemEjectorComponent>().setAcceptor(itemAcceptor);
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
            var itemEjector = Entity.GetComponent<ItemEjectorComponent>();
            Entity item = Entity.Scene.AddEntity((Entity.Scene as BasicScene).items[0].Clone(Entity.Position + new Vector2(16,0)));
            itemEjector.ejectItem(item as BaseItem);
        }
    }
}
