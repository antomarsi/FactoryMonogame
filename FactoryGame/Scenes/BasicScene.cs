using Microsoft.Xna.Framework;
using FactoryGame.Components;
using Nez;
using Nez.Sprites;
using System.Collections.Generic;
using FactoryGame.Entities;
using FactoryGame.Systems;
using FactoryGame.Items;

namespace FactoryGame.Scenes
{
    public class BasicScene : Scene
    {
        int mapWidth = 40;
        int mapHeight = 23;
        public List<BaseItem> items;
        public override void Initialize()
        {
            base.Initialize();

            SetDesignResolution(1280, 720, SceneResolutionPolicy.None);
            Screen.SetSize(1280, 720);
            InitializeItems();
            InitializeMap();
        }

        public void InitializeItems()
        {
            items = new List<BaseItem>();
            items.Add(new Crystal());

            foreach(BaseItem item in items)
            {
                item.loadContent(Content);
            }
        }

        public override void Unload()
        {
            foreach(BaseItem item in items)
            {
                item.unloadContent();
            }
        }

        public void InitializeMap()
        {
            var map = CreateEntity("map", new Vector2(0, 0));
            map.AddComponent(new MapComponent(mapWidth, mapHeight));
            var belt = new Belt();

            belt.Position = new Vector2(4 * 32, 4 * 32);
            AddEntity(belt);
            belt.onAddedToMap();

            belt = new Belt();

            belt.Position = new Vector2(5 * 32, 4 * 32);
            AddEntity(belt);

            var miner = new Miner();
            miner.Position = new Vector2(3 * 32, 4 * 32);
            AddEntity(miner);

            AddEntityProcessor(new BeltSystem(new Matcher().All(typeof(BeltComponent))));
        }
    }
}
