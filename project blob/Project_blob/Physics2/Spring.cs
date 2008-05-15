using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Spring
	{

		private float MinimumLengthBeforeCompression = 1;
		private float Length = 1;
		private float MaximumLengthBeforeExtension = 1;
		public float LengthOffset = 0;

		internal float Force = 10;

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

			Vector3 force = Util.Zero;

			// use spring displacement vector to avoid check?
			if (dist > MaximumLengthBeforeExtension)
			{
				force += Vector3.Normalize(B.CurrentPosition - A.CurrentPosition) * (Force * (dist - Length));

			}
			else if (dist < MinimumLengthBeforeCompression)
			{
				force += Vector3.Normalize(A.CurrentPosition - B.CurrentPosition) * (Force * (Length - dist));
			}

			if (next_dist > MaximumLengthBeforeExtension)
			{
				force += Vector3.Normalize(B.PotentialPosition - A.PotentialPosition) * (Force * (dist - Length));
			}
			else if (next_dist < MinimumLengthBeforeCompression)
			{
				force += Vector3.Normalize(A.PotentialPosition - B.PotentialPosition) * (Force * (Length - dist));

			}
			A.ForceThisFrame += (force * 0.5f);
			B.ForceThisFrame += (force * -0.5f);

		}

	}
}
