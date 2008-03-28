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

                // use spring displacement vector to avoid check?
                if (dist < MinimumLengthBeforeCompression)
                {
                    // vector pointing away from B
                    Vector3 result = A.CurrentPosition - B.CurrentPosition;
                    // normalize
                    result.Normalize();
                    // multiply by the scalar force
                    result = result * (Force * (Length - dist));
                    return result;
                }
                else if (dist > MaximumLengthBeforeExtension)
                {
                    Vector3 result = B.CurrentPosition - A.CurrentPosition;
                    result.Normalize();
                    result = result * (Force * (dist - Length));
                    return result;
                }
            
            return Vector3.Zero;
        }

        public Vector3 getForceVectorOnB()
        {
            float dist = Vector3.Distance(A.CurrentPosition, B.CurrentPosition);

            if (float.IsInfinity(dist))
            {
                throw new Exception();
            }

            if (dist < MinimumLengthBeforeCompression)
            {
                Vector3 result = B.CurrentPosition - A.CurrentPosition;
                result.Normalize();
                result = result * (Force * (Length - dist));
                return result;
            }
            else if (dist > MaximumLengthBeforeExtension)
            {
                Vector3 result = A.CurrentPosition - B.CurrentPosition;
                result.Normalize();
                result = result * (Force * (dist - Length));
                return result;
            }

            return Vector3.Zero;
        }

        public void ApplyForces()
        {
            A.ForceThisFrame += getForceVectorOnA();
            B.ForceThisFrame += getForceVectorOnB();
        }

    }
}
