using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Spring
	{

		public float MinimumLength = 0;
		public float MinimumLengthBeforeCompression = 1;
		public float Length = 1;
		public float MaximumLengthBeforeExtension = 1;
		public float MaximumLength = float.PositiveInfinity;
		public float LengthOffset = 0;

		public float Force = 1;

		private readonly Point A;
		private readonly Point B;

		public Spring(Point one, Point two, float theLength, float ForceConstant)
		{
			A = one;
			B = two;
			MinimumLengthBeforeCompression = Length;
			Length = theLength;
			MaximumLengthBeforeExtension = Length;
			Force = ForceConstant;
		}

		public Vector3 getForceVectorOnA()
		{
			float dist = Vector3.Distance(A.CurrentPosition, B.CurrentPosition) + LengthOffset;
			float next_dist = Vector3.Distance(A.potentialPosition, B.potentialPosition) + LengthOffset;

			Vector3 force = Vector3.Zero;

			// use spring displacement vector to avoid check?
			if (dist < MinimumLengthBeforeCompression)
			{
				// vector pointing away from B
				Vector3 dir = A.CurrentPosition - B.CurrentPosition;
				// normalize
				dir.Normalize();
				// multiply by the scalar force
				force += dir * (Force * (Length - dist));

			}
			else if (dist > MaximumLengthBeforeExtension)
			{
				Vector3 dir = B.CurrentPosition - A.CurrentPosition;

				dir.Normalize();
				force += dir * (Force * (dist - Length));

			}

			if (next_dist < MinimumLengthBeforeCompression)
			{
				// vector pointing away from B
				Vector3 dir = A.potentialPosition - B.potentialPosition;
				// normalize

				dir.Normalize();
				// multiply by the scalar force
				force += dir * (Force * (Length - dist));

			}
			else if (next_dist > MaximumLengthBeforeExtension)
			{
				Vector3 dir = B.potentialPosition - A.potentialPosition;

				dir.Normalize();
				force += dir * (Force * (dist - Length));

			}
			return force / 2f;
		}

		public Vector3 getForceVectorOnB()
		{
			float dist = Vector3.Distance(A.CurrentPosition, B.CurrentPosition) + LengthOffset;
			float next_dist = Vector3.Distance(A.potentialPosition, B.potentialPosition) + LengthOffset;

			Vector3 force = Vector3.Zero;

			// use spring displacement vector to avoid check?
			if (dist < MinimumLengthBeforeCompression)
			{
				// vector pointing away from B
				Vector3 dir = B.CurrentPosition - A.CurrentPosition;
				// normalize
				dir.Normalize();
				// multiply by the scalar force
				force += dir * (Force * (Length - dist));
			}
			else if (dist > MaximumLengthBeforeExtension)
			{
				Vector3 dir = A.CurrentPosition - B.CurrentPosition;
				dir.Normalize();
				force += dir * (Force * (dist - Length));
			}

			if (next_dist < MinimumLengthBeforeCompression)
			{
				// vector pointing away from B
				Vector3 dir = B.potentialPosition - A.potentialPosition;
				// normalize
				dir.Normalize();
				// multiply by the scalar force
				force += dir * (Force * (Length - dist));
			}
			else if (next_dist > MaximumLengthBeforeExtension)
			{
				Vector3 dir = A.potentialPosition - B.potentialPosition;
				dir.Normalize();
				force += dir * (Force * (dist - Length));
			}
			return force / 2f;
		}

		public void update()
		{
			// A spring where both ends are at the same location is invalid
			// therefore, by the Ostrich algorithm, I throw an exception
			// let me know if you get this.
			if (A.CurrentPosition == B.CurrentPosition || A.potentialPosition == B.potentialPosition)
			{
				throw new Exception("Invalid Spring State");
			}
			A.ForceThisFrame += getForceVectorOnA();
			B.ForceThisFrame += getForceVectorOnB();
		}

	}
}
