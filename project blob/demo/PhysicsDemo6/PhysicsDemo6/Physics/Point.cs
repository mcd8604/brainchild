using Microsoft.Xna.Framework;

namespace Physics
{
    public class Point : Actor
    {
        private Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        private Vector3 velocity = Vector3.Zero;
        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
        }

        public Vector3 acceleration = Vector3.Zero;

        public Vector3 CurrentForce = Vector3.Zero;

        internal Vector3 NextPosition = Vector3.Zero;
        internal Vector3 NextVelocity = Vector3.Zero;
        internal Vector3 NextAcceleration = Vector3.Zero;
        internal Vector3 NextForce = Vector3.Zero;

        internal Collidable LastCollision = null;

        public float mass = 1;

        public bool isStatic = false;

        public Point(Vector3 startPosition)
        {
            position = startPosition;
            NextPosition = Position;
        }

        public Vector3 getCurrentPosition()
        {
            return Position;
        }

        internal void updatePosition()
        {
            if (!isStatic)
            {
                position = NextPosition;
                velocity = NextVelocity;
                acceleration = NextAcceleration;
                CurrentForce = NextForce;
                NextForce = Vector3.Zero;

                //if (LastCollision != null && Physics.TEMP_SurfaceFriction >= 1)
                //{
                //    CurrentForce -= LastCollision.getPlane().Normal * (100 * Physics.TEMP_SurfaceFriction * 0.75f );
                //}
            }
        }

    }
}
