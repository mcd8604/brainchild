using Microsoft.Xna.Framework;
using System.Collections;
using System;

namespace Physics
{
    public class Point : Actor
    {

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

        public Vector3 PotientialPosition = Vector3.Zero;
        public Vector3 PotientialVelocity = Vector3.Zero;
        public Vector3 potientialAcceleration = Vector3.Zero;
        public Vector3 PotientialAcceleration
        {
            get
            {
                return potientialAcceleration;
            }
            set
            {
                potientialAcceleration = value;
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

        public Point(Vector3 startPosition)
        {
            CurrentPosition = startPosition;
			PotientialPosition = CurrentPosition;
            NextPosition = CurrentPosition;
        }

        internal void updatePosition()
        {
            if (!isStatic)
            {
                CurrentPosition = NextPosition;
                CurrentVelocity = NextVelocity;
				CurrentAcceleration = Vector3.Zero;
				nextAcceleration = Vector3.Zero;
                ForceThisFrame = ForceNextFrame;
                ForceNextFrame = Vector3.Zero;
            }
        }

    }

    public class PointComparater : IEqualityComparer
    {

        public new bool Equals(object obj1, object obj2)
        {
            return ((Physics.Point)obj1).CurrentPosition == ((Physics.Point)obj2).CurrentPosition;
        }



        #region IEqualityComparer Members


        public int GetHashCode(object obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }

        #endregion
    }
}
