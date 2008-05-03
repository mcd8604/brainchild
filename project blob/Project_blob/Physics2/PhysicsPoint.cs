using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class PhysicsPoint
	{

		private Body parent = null;
		public Body ParentBody
		{
			get
			{
				return parent;
			}
			set
			{
				if (parent != null && parent != value)
				{
					throw new Exception("Point Can't be in two bodys");
				}
				parent = value;
			}
		}

		public bool isStatic = false;

		internal bool relativelyStatic = false;

		public Vector3 ExternalPosition = Vector3.Zero;
		public Vector3 ExternalVelocity = Vector3.Zero;

		internal Vector3 CurrentPosition = Vector3.Zero;
		internal Vector3 CurrentVelocity = Vector3.Zero;

		public Vector3 PotentialPosition = Vector3.Zero;
		internal Vector3 PotentialVelocity = Vector3.Zero;

		public Vector3 NextPosition = Vector3.Zero;
		public Vector3 NextVelocity = Vector3.Zero;

		public Collidable LastCollision = null;

		public Vector3 ForceThisFrame = Vector3.Zero;
		public Vector3 ForceNextFrame = Vector3.Zero;

		public Vector3 AccelerationThisFrame = Vector3.Zero;

		public float Mass = 1f;

		public PhysicsPoint(Vector3 startPosition, Body ParentBody)
		{
			ExternalPosition = startPosition;
			CurrentPosition = startPosition;
			PotentialPosition = startPosition;
			NextPosition = startPosition;
			parent = ParentBody;
		}

		public void updatePosition()
		{
			CurrentPosition = NextPosition;
			CurrentVelocity = NextVelocity;

			ExternalPosition = CurrentPosition;
			ExternalVelocity = CurrentVelocity;

			ForceThisFrame = ForceNextFrame;
			ForceNextFrame = Vector3.Zero;
		}

	}
}
