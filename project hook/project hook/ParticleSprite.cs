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
	public class ParticleSprite:Sprite
	{
		// Position, Velocity, and Acceleration represent exactly what their names
		// indicate. They are public fields rather than properties so that users
		// can directly access their .X and .Y properties.
	
		public Vector2 Velocity;
		public Vector2 Acceleration;

		// how long this particle will "live"
		private float lifetime;
		public float Lifetime
		{
			get { return lifetime; }
			set { lifetime = value; }
		}

		// how long it has been since initialize was called
		private float timeSinceStart;
		public float TimeSinceStart
		{
			get { return timeSinceStart; }
			set { timeSinceStart = value; }
		}


		// how fast does it rotate?
		private float rotationSpeed;
		public float RotationSpeed
		{
			get { return rotationSpeed; }
			set { rotationSpeed = value; }
		}

		// is this particle still alive? once TimeSinceStart becomes greater than
		// Lifetime, the particle should no longer be drawn or updated.
		public bool Active
		{
			get { return TimeSinceStart < Lifetime; }
		}


		// initialize is called by ParticleSystem to set up the particle, and prepares
		// the particle for use.
		public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration,
			float lifetime, float scale, float rotationSpeed,String p_Texture)
		{
			// set the values to the requested values
			this.Position = position;
			this.Velocity = velocity;
			this.Acceleration = acceleration;
			this.Lifetime = lifetime;
			this.Scale = scale;
			this.RotationSpeed = rotationSpeed;

			// reset TimeSinceStart - we have to do this because particles will be
			// reused.
			this.TimeSinceStart = 0.0f;

			// set rotation to some random value between 0 and 360 degrees.
			this.Rotation = ParticleSystem.RandomBetween(0, MathHelper.TwoPi);
			this.Texture = TextureLibrary.getGameTexture(p_Texture, "");
			this.setAnimation(p_Texture,23);
			this.Animation.StartAnimation();
			this.Visible = true;
			
		}

		// update is called by the ParticleSystem on every frame. This is where the
		// particle's position and that kind of thing get updated.
		public void Update(GameTime p_GameTime, float dt)
		{
			base.Update(p_GameTime);
			Velocity += Acceleration * dt;
			Position += Velocity * dt;

			Rotation += RotationSpeed * dt;

			TimeSinceStart += dt;
		}

	
	}
}

