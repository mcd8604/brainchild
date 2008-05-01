using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Point
	{

		public bool isStatic = false;

		internal bool relativelyStatic = false;

		public Vector3 ExternalPosition = Vector3.Zero;
		public Vector3 ExternalVelocity = Vector3.Zero;

		internal Vector3 CurrentPosition = Vector3.Zero;
		internal Vector3 CurrentVelocity = Vector3.Zero;

		internal Vector3 PotentialPosition = Vector3.Zero;
		internal Vector3 PotentialVelocity = Vector3.Zero;

		internal Vector3 NextPosition = Vector3.Zero;
		internal Vector3 NextVelocity = Vector3.Zero;

		public Collidable LastCollision = null;

		public Vector3 ForceThisFrame = Vector3.Zero;
		public Vector3 AccelerationThisFrame = Vector3.Zero;

		public float Mass = 1f;

		public void updatePosition()
		{

		}

	}
}
