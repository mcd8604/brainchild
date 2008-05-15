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

			float X = 0;
			float Y = 0;
			float Z = 0;

			if (dist > MaximumLengthBeforeExtension || dist < MinimumLengthBeforeCompression)
			{
				Vector3 temp = Vector3.Normalize(A.CurrentPosition - B.CurrentPosition);
				float mult = Force * (Length - dist);

				X += temp.X * mult;
				Y += temp.Y * mult;
				Z += temp.Z * mult;
			}

			if (next_dist > MaximumLengthBeforeExtension || next_dist < MinimumLengthBeforeCompression)
			{
				Vector3 temp = Vector3.Normalize(A.PotentialPosition - B.PotentialPosition);
				float mult = Force * (Length - next_dist);

				X += temp.X * mult;
				Y += temp.Y * mult;
				Z += temp.Z * mult;
			}

			X *= 0.5f;
			Y *= 0.5f;
			Z *= 0.5f;

			A.ForceThisFrame.X += X;
			A.ForceThisFrame.Y += Y;
			A.ForceThisFrame.Z += Z;
			B.ForceThisFrame.X -= X;
			B.ForceThisFrame.Y -= Y;
			B.ForceThisFrame.Z -= Z;

		}

	}
}
