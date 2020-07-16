using FactoryGame.Items;
using Microsoft.Xna.Framework;
using Nez;
using System;

namespace FactoryGame.Components
{
    class ItemEjectorComponent : Component, IUpdatable
    {
        ItemAcceptorComponent _itemAcceptor;
        BaseItem _item;
        public Action ejectItemHandler;

        public void setAcceptor(ItemAcceptorComponent itemAcceptor)
        {
            _itemAcceptor = itemAcceptor;
        }

        public bool ejectItem(BaseItem item)
        {
            if (_item != null)
            {
                return false;
            }
            _item = item;
            return true;
        }

        public bool canEjectItem()
        {
            return _item == null;
        }

        public void tryEjectItem()
        {
            if (_item != null && _itemAcceptor != null)
            {
                if (_itemAcceptor.AddItem(_item)) { 
                    _item = null;
                    if (ejectItemHandler != null)
                    {
                        ejectItemHandler();
                    }
                }
            }
        }

        public void Update()
        {
            tryEjectItem();
        }
    }
}
