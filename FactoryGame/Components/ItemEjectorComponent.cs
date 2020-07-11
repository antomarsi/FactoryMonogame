using FactoryGame.Items;
using Microsoft.Xna.Framework;
using Nez;

namespace FactoryGame.Components
{
    class ItemEjectorComponent : RenderableComponent
    {
        BaseItem _item;

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    if (_item != null)
                        _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                            Entity.Transform.Scale, Entity.Transform.Rotation, 15, 15);
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public void AddItem(BaseItem item)
        {
            _item = item;
        }

        public bool canAcceptItem()
        {
            return _item == null;
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            if (_item != null)
            {
                _item.Render(batcher, Entity.Transform.Position + LocalOffset);
            }
        }
    }
}
