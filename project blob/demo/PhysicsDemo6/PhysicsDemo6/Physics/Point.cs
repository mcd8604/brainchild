using Microsoft.Xna.Framework;

namespace Physics
{
	public class Point : Actor
	{
		private Vector3 position = Vector3.Zero;
		public Vector3 Position
		{
			get
			{
				return position;
			}
		}

		private Vector3 velocity = Vector3.Zero;
		public Vector3 Velocity
		{
			get
			{
				return velocity;
			}
		}

		public Vector3 CurrentAcceleration = Vector3.Zero;
		public Vector3 CurrentForce = Vector3.Zero;

		internal Vector3 NextPosition = Vector3.Zero;
		internal Vector3 NextVelocity = Vector3.Zero;

		public float mass = 1;

		public Point(Vector3 startPosition)
		{
			position = startPosition;
			NextPosition = Position;
		}

		public Vector3 getCurrentPosition()
		{
			return Position;
		}

		internal void updatePosition()
		{
			position = NextPosition;
			velocity = NextVelocity;
			CurrentAcceleration = Vector3.Zero;
			CurrentForce = Vector3.Zero;
		}

	}
}
