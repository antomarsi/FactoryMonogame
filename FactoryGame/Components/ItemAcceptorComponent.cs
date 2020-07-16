using FactoryGame.Items;
using Microsoft.Xna.Framework;
using Nez;
using System;

namespace FactoryGame.Components
{
    public class ItemAcceptorComponent : Component
    {

        public Func<BaseItem, bool> acceptItemHandler;

        public bool AddItem(BaseItem item)
        {
            return acceptItemHandler(item);
        }
    }
}
