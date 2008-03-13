using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysicsDemo2
{
	internal enum CameraMode { Fixed, Follow };

	class Camera
	{
		public CameraMode mode = CameraMode.Fixed;

		private Vector3 position = Vector3.Zero;
		private Vector3 target = Vector3.Zero;
		
		private double angle = 0;
		private double angleStep = MathHelper.Pi / 36; // (5 degrees)

		private Vector3 offsetPoint = new Vector3(20, 5, 20); //temporary default
		private Vector3 relativeChasePoint = Vector3.Zero;

		public Camera()
		{
		}

		public Camera(Vector3 aPosition, Vector3 aTarget)
		{
			position = aPosition;
			target = aTarget;
		}

		public Vector3 getPosition()
		{
			return position;
		}

		public Matrix getViewMatrix() {
			return Matrix.CreateLookAt(position, target, Vector3.Up);
		}

		public void incrementAngle()
		{
			angle += angleStep;
			setRelativeChasePoint();
		}

		public void decrementAngle()
		{
			angle -= angleStep;
			setRelativeChasePoint();
		}

		public void setAngle(double anAngle)
		{
			angle = anAngle;
			setRelativeChasePoint();
		}

		private void setRelativeChasePoint()
		{			
			relativeChasePoint = new Vector3((float)Math.Cos(angle) * offsetPoint.X, offsetPoint.Y, (float)Math.Sin(angle) * offsetPoint.Z);
		}

		public void setTarget(Vector3 aTarget)
		{
			target = aTarget;
		}

		public void update()
		{
			Vector3 chasePoint = Vector3.Add(target, relativeChasePoint);
			position = Vector3.Lerp(position, chasePoint, 0.5f);
		}

		public void update(Vector3 aTarget)
		{
			if (!target.Equals(aTarget))
			{
				target = aTarget;
				Vector3 chasePoint = Vector3.Add(aTarget, relativeChasePoint);
				position = Vector3.Lerp(position, chasePoint, 0.5f);
			}
		}

	}
}
