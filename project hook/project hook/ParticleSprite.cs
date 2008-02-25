#region File Description
//-----------------------------------------------------------------------------
// Particle.cs
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
	/// <summary>
	/// particles are the little bits that will make up an effect. each effect will
	/// be comprised of many of these particles. They have basic physical properties,
	/// such as position, velocity, acceleration, and rotation. They'll be drawn as
	/// sprites, all layered on top of one another, and will be very pretty.
	/// </summary>
	internal class ParticleSprite : Sprite
	{
		// Position, Velocity, and Acceleration represent exactly what their names
		// indicate. They are internal fields rather than properties so that users
		// can directly access their .X and .Y properties.

		internal Vector2 Velocity;
		internal Vector2 Acceleration;

		// how long this particle will "live"
		protected float lifetime;
		internal float Lifetime
		{
			get { return lifetime; }
			set { lifetime = value; }
		}

		// how long it has been since initialize was called
		protected float timeSinceStart;
		internal float TimeSinceStart
		{
			get { return timeSinceStart; }
			set { timeSinceStart = value; }
		}


		// how fast does it rotate?
		protected float rotationSpeed;
		internal float RotationSpeed
		{
			get { return rotationSpeed; }
			set { rotationSpeed = value; }
		}

		// is this particle still alive? once TimeSinceStart becomes greater than
		// Lifetime, the particle should no longer be drawn or updated.
		internal bool Active
		{
			get { return TimeSinceStart < Lifetime; }
		}

		protected float originalSize;
		internal float OriginalSize
		{
			get { return originalSize; }
		}


		// initialize is called by ParticleSystem to set up the particle, and prepares
		// the particle for use.
		internal void Initialize(Vector2 center, Vector2 velocity, Vector2 acceleration,
			float lifetime, float scale, float rotationSpeed, String p_Texture, int p_Tag)
		{
			// set the values to the requested values
			Center = center;
			Velocity = velocity;
			Acceleration = acceleration;
			Lifetime = lifetime;
			ScaleScalar = scale;
			originalSize = scale;
			RotationSpeed = rotationSpeed;

			// reset TimeSinceStart - we have to do this because particles will be
			// reused.
			TimeSinceStart = 0.0f;

			// set rotation to some random value between 0 and 360 degrees.
			Rotation = SpriteParticleSystem.RandomBetween(0, MathHelper.TwoPi);
			Texture = TextureLibrary.getGameTexture(p_Texture, p_Tag);
			Enabled = true;
		}

		internal void Initialize(Vector2 center, Vector2 velocity, Vector2 acceleration,
			float lifetime, float scale, float rotationSpeed, String p_Texture, int p_Tag, String p_AnimationName, int p_FPS)
		{
			// set the values to the requested values
			Center = center;
			Velocity = velocity;
			Acceleration = acceleration;
			Lifetime = lifetime;
			ScaleScalar = scale;
			originalSize = scale;
			RotationSpeed = rotationSpeed;

			// reset TimeSinceStart - we have to do this because particles will be
			// reused.
			TimeSinceStart = 0.0f;

			// set rotation to some random value between 0 and 360 degrees.
			Rotation = SpriteParticleSystem.RandomBetween(0, MathHelper.TwoPi);
			Texture = TextureLibrary.getGameTexture(p_Texture, p_Tag);
			setAnimation(p_AnimationName, p_FPS);
			Animation.StartAnimation();
			Enabled = true;
		}

		// update is called by the ParticleSystem on every frame. This is where the
		// particle's position and that kind of thing get updated.
		internal void Update(GameTime p_GameTime, float dt)
		{
			base.Update(p_GameTime);
			Velocity += Acceleration * dt;
			Center += Velocity * dt;

			Rotation += RotationSpeed * dt;

			TimeSinceStart += dt;
		}


	}
}

