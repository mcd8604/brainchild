using Microsoft.Xna.Framework;

namespace Physics
{
	public class Point
	{
		public Vector3 Position = Vector3.Zero;
		public Vector3 Velocity = Vector3.Zero;
		public Vector3 Acceleration = Vector3.Zero;
		public Vector3 Force = Vector3.Zero;
        public bool Static; 

		internal Vector3 NextPosition = Vector3.Zero;

		public float mass = 1;

		public Point(Vector3 startPosition, bool staticness)
		{
			Position = startPosition;
			NextPosition = Position;
            Static = staticness;
		}

		public Vector3 getCurrentPosition()
		{
			return Position;
		}

		internal void updatePosition()
		{
			Position = NextPosition;
		}

	}
}
