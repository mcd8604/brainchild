using System;
using Microsoft.Xna.Framework;

namespace Physics
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
            foreach (Physics.Point p in playerBody.getPoints())
            {
                p.ForceThisFrame += Vector3.Normalize(Vector3.Cross(p.CurrentPosition - CurrentPlayerCenter, Around)) * Magnitude;
            }
        }

        internal void update(float time)
        {
            update(cling, time);
            update(traction, time);
            update(resilience, time);
            update(volume, time);

            foreach (Point p in playerBody.getPoints())
            {
                if (p.LastCollision != null && cling.value > 0)
                {
                    p.ForceThisFrame -= p.LastCollision.Normal() * cling.value;
                }
            }

            foreach (Spring s in playerBody.getSprings())
            {
                s.Force = resilience.value;
            }

            PressureBody pb = playerBody as PressureBody;
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
