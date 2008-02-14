#region File Description
//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
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
	/// ParticleSystem is an abstract class that provides the basic functionality to
	/// create a particle effect. Different subclasses will have different effects,
	/// such as fire, explosions, and plumes of smoke. To use these subclasses, 
	/// simply call AddParticles, and pass in where the particles should exist
	/// </summary>
	public abstract class SpriteParticleSystem : Sprite
	{
		private static Task m_ParticleTask = new TaskStationary();

		// this number represents the maximum number of effects this particle system
		// will be expected to draw at one time. this is set in the constructor and is
		// used to calculate how many particles we will need.
		protected int m_howManyEffects;
		public int HowManyEffects
		{
			get
			{
				return m_howManyEffects;
			}
			set
			{
				m_howManyEffects = value;
			}
		}

		// the array of particles used by this system. these are reused, so that calling
		// AddParticles will not cause any allocations.
		ParticleSprite[] particles;

		// the queue of free particles keeps track of particles that are not curently
		// being used by an effect. when a new effect is requested, particles are taken
		// from this queue. when particles are finished they are put onto this queue.
		Queue<ParticleSprite> freeParticles;
		/// <summary>
		/// returns the number of particles that are available for a new effect.
		/// </summary>
		public int FreeParticleCount
		{
			get { return freeParticles.Count; }
		}

		protected String m_TextureName;
		public String TextureName
		{
			get
			{
				return m_TextureName;
			}
			set
			{
				m_TextureName = value;
			}
		}

		protected String m_TextureTag;
		public String TextureTag
		{
			get
			{
				return m_TextureTag;
			}
			set
			{
				m_TextureTag = value;
			}
		}

		protected bool m_Animated = false;
		public bool Animated
		{
			get
			{
				return m_Animated;
			}
			set
			{
				m_Animated = value;
			}
		}

		protected String m_AnimationName;
		public String AnimationName
		{
			get
			{
				return m_AnimationName;
			}
			set
			{
				m_AnimationName = value;
			}
		}

		protected int m_AnimationFPS;
		public int AnimationFPS
		{
			get
			{
				return m_AnimationFPS;
			}
			set
			{
				m_AnimationFPS = value;
			}
		}

		// This region of values control the "look" of the particle system, and should 
		// be set by deriving particle systems in the InitializeConstants method. The
		// values are then used by the virtual function InitializeParticle. Subclasses
		// can override InitializeParticle for further
		// customization.
		#region constants to be set by subclasses

		/// <summary>
		/// minNumParticles and maxNumParticles control the number of particles that are
		/// added when AddParticles is called. The number of particles will be a random
		/// number between minNumParticles and maxNumParticles.
		/// </summary>
		protected int m_MinNumParticles;
		public int MinNumParticles
		{
			get
			{
				return m_MinNumParticles;
			}
			set
			{
				m_MinNumParticles = value;
			}
		}
		protected int m_MaxNumParticles;
		public int MaxNumParticles
		{
			get
			{
				return m_MaxNumParticles;
			}
			set
			{
				m_MaxNumParticles = value;
			}
		}

		/// <summary>
		/// minInitialSpeed and maxInitialSpeed are used to control the initial velocity
		/// of the particles. The particle's initial speed will be a random number 
		/// between these two. The direction is determined by the function 
		/// PickRandomDirection, which can be overriden.
		/// </summary>
		protected float m_MinInitialSpeed;
		public float MinInitialSpeed
		{
			get
			{
				return m_MinInitialSpeed;
			}
			set
			{
				m_MinInitialSpeed = value;
			}
		}
		protected float m_MaxInitialSpeed;
		public float MaxInitialSpeed
		{
			get
			{
				return m_MaxInitialSpeed;
			}
			set
			{
				m_MaxInitialSpeed = value;
			}
		}

		/// <summary>
		/// minAcceleration and maxAcceleration are used to control the acceleration of
		/// the particles. The particle's acceleration will be a random number between
		/// these two. By default, the direction of acceleration is the same as the
		/// direction of the initial velocity.
		/// </summary>
		protected float m_MinAcceleration;
		public float MinAcceleration
		{
			get
			{
				return m_MinAcceleration;
			}
			set
			{
				m_MinAcceleration = value;
			}
		}
		protected float m_MaxAcceleration;
		public float MaxAcceleration
		{
			get
			{
				return m_MaxAcceleration;
			}
			set
			{
				m_MaxAcceleration = value;
			}
		}

		/// <summary>
		/// minRotationSpeed and maxRotationSpeed control the particles' angular
		/// velocity: the speed at which particles will rotate. Each particle's rotation
		/// speed will be a random number between minRotationSpeed and maxRotationSpeed.
		/// Use smaller numbers to make particle systems look calm and wispy, and large 
		/// numbers for more violent effects.
		/// </summary>
		protected float m_MinRotationSpeed;
		public float MinRotationSpeed
		{
			get
			{
				return m_MinRotationSpeed;
			}
			set
			{
				m_MinRotationSpeed = value;
			}
		}
		protected float m_MaxRotationSpeed;
		public float MaxRotationSpeed
		{
			get
			{
				return m_MaxRotationSpeed;
			}
			set
			{
				m_MaxRotationSpeed = value;
			}
		}

		/// <summary>
		/// minLifetime and maxLifetime are used to control the lifetime. Each
		/// particle's lifetime will be a random number between these two. Lifetime
		/// is used to determine how long a particle "lasts." Also, in the base
		/// implementation of Draw, lifetime is also used to calculate alpha and scale
		/// values to avoid particles suddenly "popping" into view
		/// </summary>
		protected float m_MinLifetime;
		public float MinLifetime
		{
			get
			{
				return m_MinLifetime;
			}
			set
			{
				m_MinLifetime = value;
			}
		}
		protected float m_MaxLifetime;
		public float MaxLifetime
		{
			get
			{
				return m_MaxLifetime;
			}
			set
			{
				m_MaxLifetime = value;
			}
		}

		/// <summary>
		/// to get some additional variance in the appearance of the particles, we give
		/// them all random scales. the scale is a value between minScale and maxScale,
		/// and is additionally affected by the particle's lifetime to avoid particles
		/// "popping" into view.
		/// </summary>
		protected float m_MinScale;
		public float MinScale
		{
			get
			{
				return m_MinScale;
			}
			set
			{
				m_MinScale = value;
			}
		}
		protected float m_MaxScale;
		public float MaxScale
		{
			get
			{
				return m_MaxScale;
			}
			set
			{
				m_MaxScale = value;
			}
		}


		#endregion

		/// <summary>
		/// Constructs a new ParticleSystem.
		/// </summary>
		/// <param name="howManyEffects">the maximum number of particle effects that
		/// are expected on screen at once.</param>
		/// <remarks>it is tempting to set the value of howManyEffects very high.
		/// However, this value should be set to the minimum possible, because
		/// it has a large impact on the amount of memory required, and slows down the
		/// Update and Draw functions.</remarks>
		protected SpriteParticleSystem(String p_Name, String p_TextureName, String p_TextureTag, int p_HowManyEffects)
			: base(p_Name, Vector2.Zero, 0, 0, null, 0f, true, 0f, Depth.GameLayer.Explosion)
		{
			TextureName = p_TextureName;
			TextureTag = p_TextureTag;

			m_howManyEffects = p_HowManyEffects;
			InitializeConstants();

			BlendMode = SpriteBlendMode.Additive;

			// calculate the total number of particles we will ever need, using the
			// max number of effects and the max number of particles per effect.
			// once these particles are allocated, they will be reused, so that
			// we don't put any pressure on the garbage collector.
			particles = new ParticleSprite[m_howManyEffects * m_MaxNumParticles];
			freeParticles = new Queue<ParticleSprite>(m_howManyEffects * m_MaxNumParticles);
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i] = new ParticleSprite();
				particles[i].Task = m_ParticleTask;
				freeParticles.Enqueue(particles[i]);
			}
		}
		protected SpriteParticleSystem(String p_Name, String p_TextureName, String p_TextureTag, String p_AnimationName, int p_AnimationFPS, int p_HowManyEffects)
			: base(p_Name, Vector2.Zero, 0, 0, null, 0f, true, 0f, Depth.GameLayer.Explosion)
		{

			TextureName = p_TextureName;
			TextureTag = p_TextureTag;
			Animated = true;
			m_AnimationName = p_AnimationName;
			AnimationFPS = p_AnimationFPS;

			m_howManyEffects = p_HowManyEffects;
			InitializeConstants();

			BlendMode = SpriteBlendMode.Additive;

			// calculate the total number of particles we will ever need, using the
			// max number of effects and the max number of particles per effect.
			// once these particles are allocated, they will be reused, so that
			// we don't put any pressure on the garbage collector.
			particles = new ParticleSprite[m_howManyEffects * m_MaxNumParticles];
			freeParticles = new Queue<ParticleSprite>(m_howManyEffects * m_MaxNumParticles);
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i] = new ParticleSprite();
				particles[i].Task = m_ParticleTask;
				freeParticles.Enqueue(particles[i]);
			}
		}

		/// <summary>
		/// this abstract function must be overriden by subclasses of ParticleSystem.
		/// It's here that they should set all the constants marked in the region
		/// "constants to be set by subclasses", which give each ParticleSystem its
		/// specific flavor.
		/// </summary>
		protected abstract void InitializeConstants();

		/// <summary>
		/// AddParticles's job is to add an effect somewhere on the screen. If there 
		/// aren't enough particles in the freeParticles queue, it will use as many as 
		/// it can. This means that if there not enough particles available, calling
		/// AddParticles will have no effect.
		/// </summary>
		/// <param name="where">where the particle effect should be created</param>
		public void AddParticles(Vector2 where)
		{
			// the number of particles we want for this effect is a random number
			// somewhere between the two constants specified by the subclasses.
			int numParticles = Game.Random.Next(m_MinNumParticles, m_MaxNumParticles);

			// create that many particles, if you can.
			for (int i = 0; i < numParticles && freeParticles.Count > 0; i++)
			{
				// grab a particle from the freeParticles queue, and Initialize it.
				ParticleSprite p = freeParticles.Dequeue();
				InitializeParticle(p, where);
			}
		}

		/// <summary>
		/// InitializeParticle randomizes some properties for a particle, then
		/// calls initialize on it. It can be overriden by subclasses if they 
		/// want to modify the way particles are created. For example, 
		/// SmokePlumeParticleSystem overrides this function make all particles
		/// accelerate to the right, simulating wind.
		/// </summary>
		/// <param name="p">the particle to initialize</param>
		/// <param name="where">the position on the screen that the particle should be
		/// </param>
		protected virtual void InitializeParticle(ParticleSprite p, Vector2 where)
		{
			// first, call PickRandomDirection to figure out which way the particle
			// will be moving. velocity and acceleration's values will come from this
			Vector2 direction = PickRandomDirection();

			// pick some random values for our particle
			float velocity =
				RandomBetween(m_MinInitialSpeed, m_MaxInitialSpeed);
			float acceleration =
				RandomBetween(m_MinAcceleration, m_MaxAcceleration);
			float lifetime =
				RandomBetween(m_MinLifetime, m_MaxLifetime);
			float scale =
				RandomBetween(m_MinScale, m_MaxScale);
			float rotationSpeed =
				RandomBetween(m_MinRotationSpeed, m_MaxRotationSpeed);

			// then initialize it with those random values. initialize will save those,
			// and make sure it is marked as active.
			if (!Animated)
			{
				p.Initialize(
					where, velocity * direction, acceleration * direction,
					lifetime, scale, rotationSpeed, TextureName, TextureTag);
			}
			else
			{
				p.Initialize(
				where, velocity * direction, acceleration * direction,
				lifetime, scale, rotationSpeed, TextureName, TextureTag, m_AnimationName, AnimationFPS);
			}
		}

		public static float RandomBetween(float min, float max)
		{
			return min + (float)Game.Random.NextDouble() * (max - min);
		}

		/// <summary>
		/// PickRandomDirection is used by InitializeParticles to decide which direction
		/// particles will move. The default implementation is a random vector in a
		/// circular pattern.
		/// </summary>
		protected virtual Vector2 PickRandomDirection()
		{
			float angle = RandomBetween(0, MathHelper.TwoPi);
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		/// <summary>
		/// overriden from DrawableGameComponent, Update will update all of the active
		/// particles.
		/// </summary>
		public override void Update(GameTime gameTime)
		{
			// calculate dt, the change in the since the last frame. the particle
			// updates will use this value.
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// go through all of the particles...
			foreach (ParticleSprite p in particles)
			{

				if (p.Active)
				{
					// ... and if they're active, update them.
					p.Update(gameTime, dt);
					// if that update finishes them, put them onto the free particles
					// queue.
					if (!p.Active)
					{
						freeParticles.Enqueue(p);
					}
				}
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// overriden from DrawableGameComponent, Draw will use ParticleSampleGame's 
		/// sprite batch to render all of the active particles.
		/// </summary>
		public override void Draw(SpriteBatch p_SpriteBatch)
		{

			foreach (ParticleSprite p in particles)
			{
				// skip inactive particles
				if (!p.Active)
					continue;

				// normalized lifetime is a value from 0 to 1 and represents how far
				// a particle is through its life. 0 means it just started, .5 is half
				// way through, and 1.0 means it's just about to be finished.
				// this value will be used to calculate alpha and scale, to avoid 
				// having particles suddenly appear or disappear.
				float normalizedLifetime = p.TimeSinceStart / p.Lifetime;

				// we want particles to fade in and fade out, so we'll calculate alpha
				// to be (normalizedLifetime) * (1-normalizedLifetime). this way, when
				// normalizedLifetime is 0 or 1, alpha is 0. the maximum value is at
				// normalizedLifetime = .5, and is
				// (normalizedLifetime) * (1-normalizedLifetime)
				// (.5)                 * (1-.5)
				// .25
				// since we want the maximum alpha to be 1, not .25, we'll scale the 
				// entire equation by 4.

				p.Transparency = 4 * normalizedLifetime * (1 - normalizedLifetime);

				// make particles grow as they age. they'll start at 75% of their size,
				// and increase to 100% once they're finished.

				p.ScaleScalar = p.OriginalSize * (.75f + .25f * normalizedLifetime);

				p.Draw(p_SpriteBatch);


			}

		}

		public new abstract SpriteParticleSystem copy();

	}
}
