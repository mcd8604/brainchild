using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysicsDemo
{
	public class Spring
	{
		float minimumLength = 0;
		float minimumLengthBeforeCompression = 1;
		float maximumLengthBeforeExtension = 1;
		float maximumLength = float.PositiveInfinity;

		float Force = 1;

		public readonly Point A;
		public readonly Point B;

		public Spring(Point one, Point two, float Length, float ForceConstant)
		{
			A = one;
			B = two;
			minimumLengthBeforeCompression = Length;
			maximumLengthBeforeExtension = Length;
			Force = ForceConstant;
		}

		public Vector3 getForceVectorOnA()
		{
			float dist = Vector3.Distance(A.getCurrentPosition(), B.getCurrentPosition());

			// use spring displacement vector to avoid check?

			if (dist < minimumLengthBeforeCompression)
			{
				// vector pointing away from B
				Vector3 result = A.getCurrentPosition() - B.getCurrentPosition();
				// normalize
				result.Normalize();
				// multiply by the scalar force
				result = result * (Force * (minimumLengthBeforeCompression - dist));
				return result;
			}
			else if (dist > maximumLengthBeforeExtension)
			{
				Vector3 result = B.getCurrentPosition() - A.getCurrentPosition();
				result.Normalize();
				result = result * (Force * (dist - maximumLengthBeforeExtension));
				return result;
			}
			return Vector3.Zero;
		}

		public Vector3 getForceVectorOnB()
		{
			// lazy
			return Vector3.Negate(getForceVectorOnA());
		}

	}
}
