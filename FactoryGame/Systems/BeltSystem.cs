using FactoryGame.Components;
using Nez;
using System.Collections.Generic;

namespace FactoryGame.Systems
{
    public class BeltSystem : EntitySystem
    {
        public BeltSystem(Matcher matcher) : base(matcher) { }
        float secondsPerFrame = 0.1f;


        protected override void Process(List<Entity> entities)
        {
            if (entities.Count == 0)
            {
                return;
            }
            var time = Time.TimeSinceSceneLoad;
            var iterationDuration = this.secondsPerFrame * entities.LastItem().GetComponent<BeltComponent>().sprites.Length;
            var currentElapsed = time % iterationDuration;
            var desiredFrame = Mathf.FloorToInt(currentElapsed / secondsPerFrame);

            foreach (var entity in entities)
            {
                entity.GetComponent<BeltComponent>().setSpriteIndex(desiredFrame);
            }
        }
    }
}
