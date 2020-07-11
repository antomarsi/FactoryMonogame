using Nez;

namespace FactoryGame.Scenes
{
    class BasicScene : Scene
    {

        public override void Initialize()
        {
            base.Initialize();

            SetDesignResolution(1280, 720, SceneResolutionPolicy.None);
            Screen.SetSize(1280, 720);
        }
    }
}
