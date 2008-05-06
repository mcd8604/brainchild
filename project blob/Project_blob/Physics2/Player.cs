using System;
using Microsoft.Xna.Framework;

namespace Physics2
{
    public class Player
    {

        private Body playerBody = null;
        /// <summary>
        /// The Body controlled by the player.
        /// </summary>
        public Body PlayerBody
        {
            set
            {
                playerBody = value;
            }
            get
            {
                return playerBody;
            }
        }

        // Cling
        Property cling = new Property();
        /// <summary>
        /// The ability to stick to surfaces.
        /// </summary>
        public Property Cling
        {
            get
            {
                return cling;
            }
        }

        // Traction
        Property traction = new Property();
        /// <summary>
        /// The friction opposing sliding along a surface.
        /// </summary>
        public Property Traction
        {
            get
            {
                return traction;
            }
        }

        // Resilience
        Property resilience = new Property();
        /// <summary>
        /// The resistance to deformation.
        /// </summary>
        public Property Resilience
        {
            get
            {
                return resilience;
            }
        }

        // Volume
        Property volume = new Property();
        /// <summary>
        /// The internal pressure.
        /// </summary>
        public Property Volume
        {
            get
            {
                return volume;
            }
        }

        public void applyTorque(float Magnitude, Vector3 Around)
        {
            Vector3 CurrentPlayerCenter = playerBody.getCenter();
            foreach (PhysicsPoint p in playerBody.getPoints())
            {
                p.ForceThisFrame += Vector3.Normalize(Vector3.Cross(p.CurrentPosition - CurrentPlayerCenter, Around)) * Magnitude;
            }
        }

        /// <summary>
        /// fake jump trigger
        /// </summary>
        public void jump() { jumpflag = true; }

        private bool jumpflag = false;

        public float JumpWork = 10;
        public float AirJumpWork = 5;

        internal void update(float time)
        {
            update(cling, time);
            update(traction, time);
            update(resilience, time);
            update(volume, time);

            if (jumpflag)
            {
                jumpflag = false;
                Vector3 jumpVector = Vector3.Zero;

                foreach (PhysicsPoint p in PlayerBody.points)
                {
                    if (p.LastCollision != null)
                    {
                        jumpVector += p.LastCollision.Normal;
                    }
                }

                if (jumpVector != Vector3.Zero)
                {
                    jumpVector = Vector3.Normalize(jumpVector);
                }

                // Fake Jump: TODO

                // Fake Fake Jump:
                foreach (PhysicsPoint p in playerBody.getPoints())
                {
                    p.ForceThisFrame += Vector3.Up * (AirJumpWork / time);
                    p.ForceThisFrame += jumpVector * (JumpWork / time);
                }
            }

            foreach (PhysicsPoint p in playerBody.getPoints())
            {
                if (p.LastCollision != null && cling.value > 0)
                {
                    p.ForceThisFrame -= p.LastCollision.Normal * ( cling.value * p.LastCollision.getMaterial().getCling() * 10 );
                    p.LastCollision = null;
                }
            }

            foreach (Spring s in playerBody.getSprings())
            {
                s.Force = resilience.value;
            }

            BodyPressure pb = playerBody as BodyPressure;
            if (pb != null)
            {
                pb.setIdealVolume(volume.value);
            }
        }

        private void update(Property p, float time)
        {

            if (p.target != p.current)
            {
                float diff = p.current - p.target;
                float delta = p.delta * time;
                if (Math.Abs(diff) < Math.Abs(delta))
                {
                    p.current = p.target;
                }
                else
                {
                    if (diff < 0f)
                    {
                        p.current += delta;
                    }
                    else
                    {
                        p.current -= delta;
                    }
                }
            }

            if (p.current > 0.5f)
            {
                p.value = p.origin + (((p.current - 0.5f) * 2) * (p.maximum - p.origin));
            }
            else
            {
                p.value = p.minimum + ((p.current * 2) * (p.origin - p.minimum));
            }

        }

    }
}
