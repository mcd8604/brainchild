using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Point
	{

		public bool isStatic;

		public Vector3 CurrentPosition;

		public Vector3 CurrentVelocity;

		public Vector3 potentialPosition;

		public Collidable LastCollision;

		public Vector3 ForceThisFrame;

		public Vector3 AccelerationThisFrame;

		public void updatePosition()
		{

		}

	}
}
