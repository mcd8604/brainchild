using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
	public class CinematicCamera : Camera
	{
		public bool Running = false;

		private List<CameraFrame> frames = new List<CameraFrame>();
		public List<CameraFrame> Frames
		{
			get { return frames; }
			set { frames = value; }
		}

		private int currentIndex = 0;
		private float currentTime = 0;

		public bool FinishedCinematics = false;

		public override void Update(GameTime gameTime)
		{
			if (!Running)
			{
				return;
			}

			currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

			CameraFrame currentFrame = frames[currentIndex];
			CameraFrame nextFrame = frames[currentIndex + 1];

			float timeDiff = Math.Abs(nextFrame.Time - currentFrame.Time);

			float lerpAmount = MathHelper.Clamp(currentTime / timeDiff, 0, 1);

			//Run cinematics
			Position = Vector3.Lerp(currentFrame.Position, nextFrame.Position, lerpAmount);
			Target = Vector3.Lerp(currentFrame.LookAt, nextFrame.LookAt, lerpAmount);
			Up = Vector3.Lerp(currentFrame.Up, nextFrame.Up, lerpAmount);

			UpdateMatrices();

			if (currentTime > timeDiff)
			{
				++currentIndex;
				currentTime = 0f;
				if ((frames.Count - currentIndex) - 1 == 0)
				{
					currentIndex = 0;
					Running = false;
					FinishedCinematics = true;
				}
			}
		}

		public override void UpdateMatrices()
		{
			//base.UpdateMatrices();

			View = Matrix.CreateLookAt(Position, Target, Up);

			Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlane, FarPlane);

			Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));
		}
	}

	[Serializable]
	public class CameraFrame
	{
		private Vector3 position;
		public Vector3 Position
		{
			get { return position; }
			set { position = value; }
		}

		private Vector3 lookAt;
		public Vector3 LookAt
		{
			get { return lookAt; }
			set { lookAt = value; }
		}

		private Vector3 up;
		public Vector3 Up
		{
			get { return up; }
			set { up = value; }
		}

		private float time;
		public float Time
		{
			get { return time; }
			set { time = value; }
		}
	}
}
