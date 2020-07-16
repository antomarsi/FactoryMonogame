using FactoryGame.IsometricMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace FactoryGame.Components
{
    public class MapTesteComponent : RenderableComponent, IUpdatable
    {
        ExempleMap map;

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    if (map != null)
                    {
                        _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                            Entity.Transform.Scale, Entity.Transform.Rotation, map.WorldWidth, map.WorldHeight);
                    }
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public MapTesteComponent()
        {

        }
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            map = new ExempleMap(Entity.Scene.Content);
           
        }


        public override void Render(Batcher batcher, Camera camera)
        {
            for (var i = 0; i < map.Layers.Count; i++)
            {
                map.RenderLayer(map.Layers[i], batcher, Entity.Transform.Position + _localOffset, Transform.Scale, LayerDepth, camera.Bounds);
            }
        }

        public override void DebugRender(Batcher batcher)
        {
            base.DebugRender(batcher);

            for (var i = 0; i < map.Layers.Count; i++)
            {
                foreach (Tile tile in map.Layers[i].Tiles)
                {
                    Vector2 pos = map.isometricTileToWorldPosition(tile.X, tile.Y);
                    Debug.DrawText(Graphics.Instance.BitmapFont, string.Format("{0},{1}", tile.X, tile.Y), pos + Entity.Position + (map.TileSize/2), Color.Black);
                    foreach (Rectangle rect in map.Layers[i].GetCollisionRectangles())
                    {
                        Rectangle rec = rect.Clone();
                        rec.X += (int)(Entity.Position.X);
                        rec.Y += (int)(Entity.Position.Y);
                        Debug.DrawHollowRect(rec, Color.Red);
                    }
                }
            }
        }

        public void Update()
        {
            map.Update();
            var pos = Entity.Scene.Camera.MouseToWorldPoint();
            Debug.DrawText(Graphics.Instance.BitmapFont, string.Format("{0}", map.isometricWorldToTilePosition(pos - Entity.Position)), pos, Color.Black);
        }
    }
}
