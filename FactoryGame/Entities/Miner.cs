using FactoryGame.Components;
using Microsoft.Xna.Framework;
using Nez;

namespace FactoryGame.Entities
{
    public class Miner : Entity
    {
        public Miner() : base("miner") {
            SetupComponents();
        }

        public void SetupComponents()
        {
            AddComponent(new MinerComponent());
            AddComponent(new ItemEjectorComponent());
        }
    }
}
