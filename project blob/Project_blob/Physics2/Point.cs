using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Point
	{

		public bool isStatic;

		public bool relativelyStatic;

		private Vector3 currentPosition = Vector3.Zero;
		public Vector3 CurrentPosition
		{
			get { return currentPosition; }
			internal set { currentPosition = value; }
		}

		public Vector3 CurrentVelocity;

		public Vector3 PotentialPosition;

		public Vector3 PotentialVelocity;

		public Collidable LastCollision;

		public Vector3 ForceThisFrame;

		public Vector3 AccelerationThisFrame;

		public Vector3 Mass;

		public void updatePosition()
		{

		}

	}
}
