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

		private bool useRelativePoints = false;
		public bool UseRelativePoints
		{
			get
			{
				return useRelativePoints;
			}
			set
			{
				useRelativePoints = value;
			}
		}

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

		public bool run = true;

		//Bodies cannot share this task because 
		//currentIndex and currentTime will be 
		//shared and updated by each body
		private int currentIndex = 0;
		private float currentTime = 0;
		private bool forward = true;

		public TaskKeyFrameMovement() { }

		/// <summary>
		/// Copy Constructor
		/// </summary>
		/// <param name="t"></param>
		public TaskKeyFrameMovement(TaskKeyFrameMovement t)
		{
			active = t.Active;
			useRelativePoints = t.UseRelativePoints;
			foreach (KeyFrame k in t.Frames)
			{
				frames.Add(new KeyFrame(k));
			}
			mode = t.Mode;			
		}

		public override void update(Body b, float time)
		{
			// optimize this later..
			if (!active)
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
					targetFrame = frames[currentIndex + 1];
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

			if (targetFrame != null)
			{

				float timeDiff = Math.Abs(targetFrame.Time - currentFrame.Time);

				Vector3 newPosition;

				if (useRelativePoints)
				{
					if (forward)
					{
						newPosition = Vector3.Lerp(currentFrame.Position, targetFrame.Position, MathHelper.Clamp(time / timeDiff, 0, 1)) + b.getCenter();
					}
					else
					{
						newPosition = b.getCenter() - Vector3.Lerp(targetFrame.Position, currentFrame.Position, MathHelper.Clamp(time / timeDiff, 0, 1));
					}
				}
				else
				{
					newPosition = Vector3.Lerp(currentFrame.Position, targetFrame.Position, MathHelper.Clamp(currentTime / timeDiff, 0, 1));
				}

				if (currentTime > timeDiff)
				{
					switch (mode)
					{
						case Modes.Once:
							++currentIndex;
							if (frames.Count - currentIndex <= 1)
							{
								active = false;
								currentIndex = 0;
							}
							break;
						case Modes.Loop:
							++currentIndex;
							if (frames.Count - currentIndex <= 1)
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

				Vector3 delta;

				if (active)
				{
					delta = (newPosition - b.getCenter()) / time;
				}
				else
				{
					delta = Vector3.Zero;
				}

				foreach (PhysicsPoint p in b.points)
				{
					p.PotentialVelocity = delta;
				}

				b.setCenter(newPosition);
			}
		}
	}
}
