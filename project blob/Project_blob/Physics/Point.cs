using Microsoft.Xna.Framework;
using System.Collections;
using System;

namespace Physics
{
	public class Point : Actor
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

		internal Vector3 PhysicsCurrentPosition = Vector3.Zero;
		internal Vector3 PhysicsCurrentVelocity = Vector3.Zero;
		internal Vector3 PhysicsCurrentAcceleration = Vector3.Zero;

		public Vector3 CurrentPosition = Vector3.Zero;
		public Vector3 CurrentVelocity = Vector3.Zero;
		public Vector3 currentAcceleration = Vector3.Zero;
		public Vector3 CurrentAcceleration
		{
			get
			{
				return currentAcceleration;
			}
			set
			{
				currentAcceleration = value;
			}
		}

		public Vector3 forceThisFrame = Vector3.Zero;
		public Vector3 ForceThisFrame
		{
			get
			{
				return forceThisFrame;
			}
			set
			{
				forceThisFrame = value;
			}
		}

		public Vector3 potentialPosition = Vector3.Zero;
		public Vector3 potentialVelocity = Vector3.Zero;
		public Vector3 potentialAcceleration = Vector3.Zero;
		public Vector3 PotentialAcceleration
		{
			get
			{
				return potentialAcceleration;
			}
			set
			{
				potentialAcceleration = value;
			}
		}

		public Vector3 NextPosition = Vector3.Zero;
		public Vector3 NextVelocity = Vector3.Zero;
		public Vector3 nextAcceleration = Vector3.Zero;
		public Vector3 NextAcceleration
		{
			get
			{
				return nextAcceleration;
			}
			set
			{
				nextAcceleration = value;
			}
		}

		public Vector3 ForceNextFrame = Vector3.Zero;

		public float mass = 1;
		public bool isStatic = false;

		public Collidable LastCollision = null;

		public Point(Vector3 startPosition, Body ParentBody)
		{
			PhysicsCurrentPosition = startPosition;
			CurrentPosition = startPosition;
			potentialPosition = CurrentPosition;
			NextPosition = CurrentPosition;
			parent = ParentBody;
		}

		internal void updatePhysicsPosition()
		{
			PhysicsCurrentPosition = NextPosition;
			PhysicsCurrentVelocity = NextVelocity;
			PhysicsCurrentAcceleration = Vector3.Zero;
			nextAcceleration = Vector3.Zero;
			ForceThisFrame = ForceNextFrame;
			ForceNextFrame = Vector3.Zero;
		}

		internal void updatePosition()
		{
			CurrentPosition = PhysicsCurrentPosition;
			CurrentVelocity = PhysicsCurrentVelocity;
			CurrentAcceleration = PhysicsCurrentAcceleration;
		}

	}

	public class PointComparater : IEqualityComparer
	{

		public new bool Equals(object obj1, object obj2)
		{
			return ((Physics.Point)obj1).PhysicsCurrentPosition == ((Physics.Point)obj2).PhysicsCurrentPosition;
		}



		#region IEqualityComparer Members


		public int GetHashCode(object obj)
		{
			return obj.ToString().ToLower().GetHashCode();
		}

		#endregion
	}
}
