using FactoryGame.Components;

namespace FactoryGame.Entities
{
    class Belt : Nez.Entity
    {
        public Belt() : base("belt")
        {
            setupComponents();
        }

        public void setupComponents()
        {
            this.AddComponent(new ItemAcceptorComponent());
            this.AddComponent(new ItemEjectorComponent());
            this.AddComponent(new BeltComponent());
        }

        public void onAddedToMap()
        {

        }
    }
}
