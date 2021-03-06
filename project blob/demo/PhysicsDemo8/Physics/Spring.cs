using System;
using Microsoft.Xna.Framework;

namespace Physics
{
    public class Spring : Mover
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
			float next_dist = Vector3.Distance(A.PotientialPosition, B.PotientialPosition) + LengthOffset;

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
					Vector3 dir = A.PotientialPosition - B.PotientialPosition;
					// normalize
					dir.Normalize();
					// multiply by the scalar force
					force += dir * (Force * (Length - dist));
				}
				else if (next_dist > MaximumLengthBeforeExtension)
				{
					Vector3 dir = B.PotientialPosition - A.PotientialPosition;
					dir.Normalize();
					force += dir * (Force * (dist - Length));
				}
            
            return force;
        }

        public Vector3 getForceVectorOnB()
        {
			float dist = Vector3.Distance(A.CurrentPosition, B.CurrentPosition) + LengthOffset;
			float next_dist = Vector3.Distance(A.PotientialPosition, B.PotientialPosition) + LengthOffset;

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
				Vector3 dir = B.PotientialPosition - A.PotientialPosition;
				// normalize
				dir.Normalize();
				// multiply by the scalar force
				force += dir * (Force * (Length - dist));
			}
			else if (next_dist > MaximumLengthBeforeExtension)
			{
				Vector3 dir = A.PotientialPosition - B.PotientialPosition;
				dir.Normalize();
				force += dir * (Force * (dist - Length));
			}

			return force;
        }

        public void ApplyForces()
        {
            A.ForceThisFrame += getForceVectorOnA();
            B.ForceThisFrame += getForceVectorOnB();
        }

    }
}
