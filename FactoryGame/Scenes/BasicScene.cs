using Microsoft.Xna.Framework;
using FactoryGame.Components;
using Nez;
using Nez.Sprites;
using System.Collections.Generic;
using FactoryGame.Entities;
using FactoryGame.Items;
using Microsoft.Xna.Framework.Graphics;

namespace FactoryGame.Scenes
{
    public class BasicScene : Scene
    {
        int mapWidth = 5;
        int mapHeight = 5;
        public List<BaseItem> items;
        public SpriteFont font;
        public override void Initialize()
        {
            base.Initialize();

            SetDesignResolution(1280, 720, SceneResolutionPolicy.BestFit);
            Screen.SetSize(1280, 720);
            InitializeItems();
            InitializeMap();
            font = Content.Load<SpriteFont>("Fonts/Roboto");
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

        public void InitializeMap()
        {
            var map = CreateEntity("map", new Vector2(200, 200));
            map.AddComponent(new MapTesteComponent());

            //setupTest();
        }

        public void setupTest()
        {
            var miner = new Miner();
            miner.Position = new Vector2(3 * 32, 4 * 32);
            AddEntity(miner);

            Belt oldBelt = null;
            for (int i = 0; i < 8; i++)
            {
                Belt belt = new Belt();
                belt.Position = new Vector2((4 + i) * 32, 4 * 32);
                if (oldBelt != null)
                {
                    oldBelt.GetComponent<ItemEjectorComponent>().setAcceptor(belt.GetComponent<ItemAcceptorComponent>());
                }
                if (i == 0)
                {
                    miner.GetComponent<ItemEjectorComponent>().setAcceptor(belt.GetComponent<ItemAcceptorComponent>());
                }
                oldBelt = AddEntity(belt);
            }

            var miner2 = new Miner();
            miner2.Position = new Vector2(11 * 32, 3 * 32);
            AddEntity(miner2);
            Belt belt2 = new Belt();
            belt2.Position = new Vector2(12 * 32, 3 * 32);
            belt2.SetRotationDegrees(90);
            AddEntity(belt2);
            miner2.GetComponent<ItemEjectorComponent>().setAcceptor(belt2.GetComponent<ItemAcceptorComponent>());

            for (int j = 0; j < 14; j++)
            {
                Belt belt = new Belt();
                belt.Position = new Vector2(12 * 32, (4 + j) * 32);
                if (oldBelt != null)
                {
                    oldBelt.GetComponent<ItemEjectorComponent>().setAcceptor(belt.GetComponent<ItemAcceptorComponent>());
                }
                if (j == 0)
                {
                    belt2.GetComponent<ItemEjectorComponent>().setAcceptor(belt.GetComponent<ItemAcceptorComponent>());
                }
                belt.SetRotationDegrees(90);
                oldBelt = AddEntity(belt);
            }
        }
    }
}
