using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Point
	{

		public bool isStatic;

		public Vector3 CurrentPosition;

		public Vector3 CurrentVelocity;

        public Vector3 CurrentAcceleration;

		public Vector3 PotentialPosition;

        public Vector3 PotentialAcceleration;

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
