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
	public class ExplosionSpriteParticleSystem : SpriteParticleSystem
	{
		/// <summary>
		/// Initial angle of the particles - specified in radians
		/// </summary>
		protected float m_Direction;
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
		protected float m_Theta;
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

		public ExplosionSpriteParticleSystem(
#if !FINAL
			String p_Name,
#endif
			String p_TextureName, String p_TextureTag, int p_HowManyEffects)
			: base(
#if !FINAL
			p_Name,
#endif
			p_TextureName, p_TextureTag, p_HowManyEffects )
		{}
		public ExplosionSpriteParticleSystem(
#if !FINAL
			String p_Name,
#endif
			String p_TextureName, String p_TextureTag, String p_AnimationName, int p_AnimationFPS, int p_HowManyEffects)
			: base(
#if !FINAL
			p_Name,
#endif
			p_TextureName, p_TextureTag, p_AnimationName, p_AnimationFPS, p_HowManyEffects)
		{}

		/// <summary>
		/// Set up the constants that will give this particle system its behavior and
		/// properties.
		/// </summary>
		protected override void InitializeConstants()
		{
			// high initial speed with lots of variance.  make the values closer
			// together to have more consistently circular explosions.
			m_MinInitialSpeed = 4;
			m_MaxInitialSpeed = 80;

			// doesn't matter what these values are set to, acceleration is tweaked in
			// the override of InitializeParticle.
			m_MinAcceleration = 0;
			m_MaxAcceleration = 0;

			// explosions should be relatively short lived
			m_MinLifetime = 0.5f;
			m_MaxLifetime = 1.0f;

			m_MinScale = 0.75f;
			m_MaxScale = 1.0f;

			m_MinNumParticles = 10;
			m_MaxNumParticles = 40;

			m_MinRotationSpeed = -MathHelper.PiOver4;
			m_MaxRotationSpeed = MathHelper.PiOver4;

			//default to 360 degree range
			Direction = 0;
			Theta = (float)Math.PI;

		}

		protected override void InitializeParticle(ParticleSprite p, Vector2 where)
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
		protected override Vector2 PickRandomDirection()
		{
			float angle = RandomBetween(Direction - Theta, Direction + Theta);
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}


		public override SpriteParticleSystem copy()
		{
			ExplosionSpriteParticleSystem esps = new ExplosionSpriteParticleSystem(
#if !FINAL
				Name,
#endif
				TextureName, TextureTag, HowManyEffects);
			esps.MinInitialSpeed = MinInitialSpeed;
			esps.MaxInitialSpeed = MaxInitialSpeed;
			esps.MinLifetime = MinLifetime;
			esps.MaxLifetime = MaxLifetime;
			esps.MinNumParticles = MinNumParticles;
			esps.MaxNumParticles = MaxNumParticles;
			esps.MinScale = MinScale;
			esps.MaxScale = MaxScale;
			return esps;
		}

	}
}
