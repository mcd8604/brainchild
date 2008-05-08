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

		private readonly PhysicsPoint A;
		private readonly PhysicsPoint B;

		public Spring(PhysicsPoint one, PhysicsPoint two, float theLength, float ForceConstant)
		{
			A = one;
			B = two;
			MinimumLengthBeforeCompression = Length;
			Length = theLength;
			MaximumLengthBeforeExtension = Length;
			Force = ForceConstant;
		}

		public void update()
		{
			// A spring where both ends are at the same location is invalid
			// therefore, by the Ostrich algorithm, I throw an exception
			// let me know if you get this.
#if DEBUG
			if (A.CurrentPosition == B.CurrentPosition || A.PotentialPosition == B.PotentialPosition)
			{
				throw new Exception("Invalid Spring State");
			}
#endif

			float dist = Vector3.Distance(A.CurrentPosition, B.CurrentPosition) + LengthOffset;
			float next_dist = Vector3.Distance(A.PotentialPosition, B.PotentialPosition) + LengthOffset;

			Vector3 force = Vector3.Zero;

			// use spring displacement vector to avoid check?
			if (dist < MinimumLengthBeforeCompression)
			{
				force += Vector3.Normalize(A.CurrentPosition - B.CurrentPosition) * (Force * (Length - dist));
			}
			else if (dist > MaximumLengthBeforeExtension)
			{
				force += Vector3.Normalize(B.CurrentPosition - A.CurrentPosition) * (Force * (dist - Length));
			}

			if (next_dist < MinimumLengthBeforeCompression)
			{
				force += Vector3.Normalize(A.PotentialPosition - B.PotentialPosition) * (Force * (Length - dist));
			}
			else if (next_dist > MaximumLengthBeforeExtension)
			{
				force += Vector3.Normalize(B.PotentialPosition - A.PotentialPosition) * (Force * (dist - Length));
			}
			A.ForceThisFrame += (force * 0.5f);
			B.ForceThisFrame += (force * -0.5f);

		}

	}
}
