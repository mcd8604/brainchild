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

		public Vector3 ExternalPosition = Util.Zero;
		public Vector3 ExternalVelocity = Util.Zero;

		public Vector3 CurrentPosition = Util.Zero;
		internal Vector3 CurrentVelocity = Util.Zero;

		public Vector3 PotentialPosition = Util.Zero;
		internal Vector3 PotentialVelocity = Util.Zero;

		public Vector3 NextPosition = Util.Zero;
		public Vector3 NextVelocity = Util.Zero;

		/// <summary>
		/// Null, or the last collidable this point hit
		/// </summary>
		public Collidable LastCollision = null;

		public Vector3 ForceThisFrame = Util.Zero;
		public Vector3 ForceNextFrame = Util.Zero;

		public Vector3 AccelerationThisFrame = Util.Zero;

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
			if (NextPosition == CurrentPosition)
			{
				NextPosition = PotentialPosition;
				NextVelocity = PotentialVelocity;
			}

			CurrentPosition = NextPosition;
			CurrentVelocity = NextVelocity;

			ExternalPosition = CurrentPosition;
			ExternalVelocity = CurrentVelocity;

			ForceThisFrame = ForceNextFrame;

			ForceNextFrame.X = 0;
			ForceNextFrame.Y = 0;
			ForceNextFrame.Z = 0;

			AccelerationThisFrame.X = 0;
			AccelerationThisFrame.Y = 0;
			AccelerationThisFrame.Z = 0;
		}

	}
}
