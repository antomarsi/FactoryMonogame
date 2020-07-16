using FactoryGame.Scenes;
using Microsoft.Xna.Framework;
using Nez;

namespace FactoryGame
{
	public class FactoryGame : Core
	{
        protected override void Initialize()
        {
            base.Initialize();

#if DEBUG
            DebugRenderEnabled = true;
#else
            DebugRenderEnabled = false;
#endif
            Window.AllowUserResizing = true;
			Scene = new BasicScene();
        }
	}
}
