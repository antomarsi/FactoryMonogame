using FactoryGame.Scenes;
using Microsoft.Xna.Framework;
using Nez;

namespace FactoryGame
{
	class FactoryGame : Core
	{
		GraphicsDeviceManager graphics;

        protected override void Initialize()
        {
            base.Initialize();

			Window.AllowUserResizing = true;
			Scene = new BasicScene();
        }
	}
}
