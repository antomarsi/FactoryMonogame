using Microsoft.Xna.Framework;
using Nez;
using Nez.Tweens;
using System.Collections.Generic;

namespace FactoryGame.Utils
{
    public class PathTween : Vector2Tween, ITweenTarget<Vector2>
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 _current_position;
        int current_index = 0;

        public Vector2 GetTweenedValue()
        {
            return _current_position;
        }

        public void SetTweenedValue(Vector2 value)
        {
            _current_position = value;
            if (_elapsedTime - (_duration * current_index) >= _duration / path.Count)
            {
                current_index++;
            }
        }

        protected override void UpdateValue()
        {
            _target.SetTweenedValue(Lerps.Ease(_easeType, path[current_index], path[current_index+1], _elapsedTime - (_duration * current_index), _duration / path.Count));
        }

        public override void RecycleSelf()
        {
            if (_shouldRecycleTween)
            {
                _target = null;
                _nextTween = null;
                Pool<PathTween>.Free(this);
            }
        }
    }
}
