using FactoryGame.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tweens;
using System;

namespace FactoryGame.Components
{
    class BeltComponent : SpriteRenderer, IUpdatable
    {
        public Sprite[] sprites;
        float secondsPerFrame = 0.1f;
        BaseItem _item;
        float beltSpeed = 0.5f;

        ItemAcceptorComponent itemAcceptor;
        ItemEjectorComponent itemEjector;
        bool isHoldingItem = false;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            var texture = Entity.Scene.Content.Load<Texture2D>("Sprites/Belt");
            sprites = Sprite.SpritesFromAtlas(texture, 32, 32).ToArray();
            SetSprite(sprites[0]);
            itemAcceptor = Entity.GetComponent<ItemAcceptorComponent>();
            itemEjector = Entity.GetComponent<ItemEjectorComponent>();

            itemAcceptor.acceptItemHandler = onItemAccepted;
        }
        public bool onItemAccepted(BaseItem item)
        {
            if (_item != null)
            {
                return false;
            }
            _item = item;
            _item.TweenPositionTo(Entity.Position, beltSpeed / 2)
                .SetFrom(_item.Position)
                .SetRecycleTween(false)
                .SetEaseType(EaseType.Linear)
                .SetCompletionHandler(delegate (ITween<Vector2> position) {
                    isHoldingItem = true;
                }).Start();
            return true;
        }

        public void UpdateSprite()
        {
            var time = Time.TimeSinceSceneLoad;
            var iterationDuration = this.secondsPerFrame * sprites.Length * beltSpeed;
            var currentElapsed = time % iterationDuration;
            var desiredFrame = Mathf.FloorToInt(currentElapsed / secondsPerFrame);
            SetSprite(sprites[desiredFrame]);
        }

        public void Update()
        {
            UpdateSprite();
            if (isHoldingItem && itemEjector.canEjectItem())
            {
                isHoldingItem = false;
                Vector2 endPosition = Entity.Position + new Vector2(16, 0);
                
                _item.TweenPositionTo(exitPosition(), beltSpeed/2)
                          .SetFrom(_item.Position)
                          .SetRecycleTween(false)
                          .SetEaseType(EaseType.Linear)
                          .SetCompletionHandler(delegate (ITween<Vector2> position2) {
                              itemEjector.ejectItem(_item);
                _item = null;
                          })
                          .Start();
            }
        }

        Vector2 exitPosition()
        {
            var angle = Entity.RotationDegrees;
            float xDiff = 16;
            float yDiff = 0;

            //Rotate the vector
            float x = (float)((Math.Cos(angle) * xDiff) - (Math.Sin(angle) * yDiff) + Entity.Position.X);
            float y = (float)((Math.Sin(angle) * xDiff) + (Math.Cos(angle) * yDiff) + Entity.Position.Y);

            return new Vector2(x, y);
        }
    }
}
