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
                p.CurrentForce += Vector3.Normalize(Vector3.Cross(p.Position - CurrentPlayerCenter, Around)) * Magnitude;
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
                    //CurrentForce -= LastCollision.getPlane().Normal * (100 * Physics.TEMP_SurfaceFriction * 0.75f);
                    p.CurrentForce -= p.LastCollision.Normal() * cling.value;
                }
            }

            foreach (Spring s in playerBody.getSprings())
            {
                s.Force = resilience.value;
            }

            Vector3 CurrentPlayerCenter = playerBody.getCenter();
            float CurrentPlayerVolume = playerBody.getVolume();
            foreach (Physics.Point p in playerBody.getPoints())
            {
                p.CurrentForce += (CurrentPlayerCenter - p.Position) * (CurrentPlayerVolume - volume.value);
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
                p.value = p.origin + ((p.current - 0.5f) * (p.maximum - p.origin));
            }
            else
            {
                p.value = p.minimum + (p.current * (p.origin - p.minimum));
            }

        }

    }

    public class Property
    {
        internal float minimum;
        /// <summary>
        /// The minimum value this property is allowed, corresponding to a target of 0;
        /// This value is considered to be a constant by the physics engine.
        /// </summary>
        public float Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
            }
        }

        internal float maximum;
        /// <summary>
        /// The maximum value this property is allowed, corresponding to a target of 1;
        /// This value is considered to be a constant by the physics engine.
        /// </summary>
        public float Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
            }
        }

        internal float origin;
        /// <summary>
        /// The default value for this property, corresponding to a target of 0.5;
        /// This value is considered to be a constant by the physics engine.
        /// </summary>
        public float Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }

        internal float delta = 1f;
        /// <summary>
        /// The maximum rate of change for this property;
        /// </summary>
        public float Delta
        {
            get
            {
                return delta;
            }
            set
            {
                delta = value;
            }
        }

        internal float target = 0.5f;
        /// <summary>
        /// The target value for this property at this moment, as indicated by the user, if applicable, as a ratio from 0 to 1.
        /// </summary>
        public float Target
        {
            get
            {
                return target;
            }
            set
            {
                if (value < 0f || value > 1f)
                {
                    throw new ArgumentOutOfRangeException("Target must be between 0 and 1, given " + value);
                }
                target = value;
            }
        }

        /// <summary>
        /// The current position, as limited by delta, as a value between 0 and 1.
        /// </summary>
        internal float current = 0.5f;
        internal float value;


        internal float last; //?

    }
}
