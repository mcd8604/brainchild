#region File Description
//-----------------------------------------------------------------------------
// ExplosionParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace project_hook
{
    public class BloodParticleSystem : ParticleSystem
	{
		/// <summary>
		/// Initial angle of the particles - specified in radians
		/// </summary>
		private float m_Direction;
		public float Direction
		{
			get
			{
				return m_Direction;
			}

			set
			{
				m_Direction = value;
			}
		}

		/// <summary>
		/// Range of the offset from the initial angle - specified in radians
		/// </summary>
		private float m_Theta;
		public float Theta
		{
			get
			{
				return m_Theta;
			}

			set
			{
				m_Theta = value;
			}
		}

        public BloodParticleSystem(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, 
			float p_Degree, float p_Z, int p_HowManyEffects)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z, p_HowManyEffects)
        {
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            // high initial speed with lots of variance.  make the values closer
            // together to have more consistently circular explosions.
            minInitialSpeed = 40;
			maxInitialSpeed = 80;

            // doesn't matter what these values are set to, acceleration is tweaked in
            // the override of InitializeParticle.
            minAcceleration = 0;
            maxAcceleration = 0;

            // explosions should be relatively short lived
            minLifetime = .5f;
            maxLifetime = 1.0f;

            minScale = .01f;
            maxScale = .2f;

            minNumParticles = 50;
            maxNumParticles = 100;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

			//default to 360 degree range
			Direction = 0;
			Theta = (float)Math.PI;

        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);
            
            // The base works fine except for acceleration. Explosions move outwards,
            // then slow down and stop because of air resistance. Let's change
            // acceleration so that when the particle is at max lifetime, the velocity
            // will be zero.

            // We'll use the equation vt = v0 + (a0 * t). (If you're not familar with
            // this, it's one of the basic kinematics equations for constant
            // acceleration, and basically says:
            // velocity at time t = initial velocity + acceleration * t)
            // We'll solve the equation for a0, using t = p.Lifetime and vt = 0.
            p.Acceleration = -p.Velocity / p.Lifetime;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override Vector2  PickRandomDirection()
		{
			float angle = RandomBetween(Direction - Theta, Direction + Theta);
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}
    }
}
