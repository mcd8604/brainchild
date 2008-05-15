using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics2
{
	[Serializable]
	public class TaskKeyFrameMovement : Task
	{

		public enum Modes { Once, Mirror, Loop };

		private List<KeyFrame> frames = new List<KeyFrame>();
		public List<KeyFrame> Frames
		{
			get
			{
				return frames;
			}
			set
			{
				frames = value;
			}
		}

		private Modes mode = Modes.Once;
		public Modes Mode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;
			}
		}

		private int currentIndex = 0;
		private float currentTime = 0;
		private bool forward = true;
		public bool run = true;

		public override void update(Body b, float time)
		{
			// optimize this later..

			if (!run)
			{
				return;
			}

			currentTime += time;

			KeyFrame currentFrame = null;
			KeyFrame targetFrame = null;

			switch (mode)
			{
				case Modes.Loop:
					currentFrame = frames[currentIndex];
					targetFrame = frames[(currentIndex + 1) % frames.Count];
					break;
				case Modes.Once:
					currentFrame = frames[currentIndex];
					targetFrame = frames[(int)MathHelper.Clamp(currentIndex + 1, 0, frames.Count)];
					break;
				case Modes.Mirror:
					if (forward)
					{
						currentFrame = frames[currentIndex];
						targetFrame = frames[currentIndex + 1];
					}
					else
					{
						currentFrame = frames[currentIndex];
						targetFrame = frames[currentIndex - 1];
					}
					break;
			}

			float timeDiff = Math.Abs(targetFrame.Time - currentFrame.Time);

			Vector3 newPosition = Vector3.Lerp(currentFrame.Position, targetFrame.Position, MathHelper.Clamp(currentTime / timeDiff, 0, 1));

			if (currentTime > timeDiff)
			{
				switch (mode)
				{
					case Modes.Once:
						++currentIndex;
						if (frames.Count - currentIndex == 0)
						{
							run = false;
						}
						break;
					case Modes.Loop:
						++currentIndex;
						if (frames.Count - currentIndex == 0)
						{
							currentIndex = 0;
						}
						break;
					case Modes.Mirror:
						if (forward)
						{
							++currentIndex;
							if (frames.Count - currentIndex <= 1)
							{
								forward = false;
							}
						}
						else
						{
							--currentIndex;
							if (currentIndex == 0)
							{
								forward = true;
							}
						}
						break;
				}
				currentTime = 0f;
			}

			Vector3 delta = (newPosition - b.getCenter()) / time;

			foreach (PhysicsPoint p in b.points)
			{
				p.PotentialVelocity = delta;
			}

			b.setCenter(newPosition);
		}

	}
}
