using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryGame.IsometricMap
{
    public class TilesetTile
    {
		public int Id;
		int _animationCurrentFrame;
		public List<AnimationFrame> AnimationFrames;
		float _animationElapsedTime;
		public int currentAnimationFrameGid => AnimationFrames[_animationCurrentFrame].Gid;

		public void UpdateAnimatedTiles()
		{
			if (AnimationFrames.Count == 0)
				return;

			_animationElapsedTime += Time.DeltaTime;

			if (_animationElapsedTime > AnimationFrames[_animationCurrentFrame].Duration)
			{
				_animationCurrentFrame = Mathf.IncrementWithWrap(_animationCurrentFrame, AnimationFrames.Count);
				_animationElapsedTime = 0;
			}
		}
	}

	public class AnimationFrame
	{
		public int Gid;
		public float Duration;
	}
}
