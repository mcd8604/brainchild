using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/// <summary>
	/// A Subclass of Sprite, representing those sprites that are checked for collision
	/// </summary>
	public class Collidable : Sprite
	{
		#region Variables and Properties
		//which faction does this sprite belong to
		public enum Factions
		{
			Player,
			PowerUp,
			Enemy,
			Environment,
			None,
			Blood
		}

		/// <summary>
		/// Set of available mathimatical bounding areas a collidable may have for collision detection.
		/// </summary>
		public enum Boundings
		{
			Circle,
			Diamond,
			Square,
			Rectangle,
			Triangle
		}

		protected Boundings m_Bound = Boundings.Circle;
		public Boundings Bound
		{
			get
			{
				return m_Bound;
			}
			set
			{
				m_Bound = value;
			}
		}

		protected Factions m_Faction = Factions.None;
		public Factions Faction
		{
			get
			{
				return m_Faction;
			}
			set
			{
				m_Faction = value;
			}
		}

		protected float m_MaxHealth = float.NaN;
		public float MaxHealth
		{
			get
			{
				return m_MaxHealth;
			}
			set
			{
				m_MaxHealth = value;
				m_Health = m_MaxHealth;
			}
		}

		//this is how much health this sprite has
		protected float m_Health = 0;
		public float Health
		{
			get
			{
				return m_Health;
			}
			set
			{
				m_Health = value;
			}
		}

		public override bool ToBeRemoved
		{
			get
			{
				return base.ToBeRemoved;
			}
			set
			{
				if (m_DamageEffect != null)
				{
					m_DamageEffect.Enabled = Enabled;
					m_DamageEffect.ToBeRemoved = value;
				}
				base.ToBeRemoved = value;
			}
		}

		protected bool m_Grabbable = true;
		public bool Grabbable
		{
			set
			{
				m_Grabbable = value;
			}
			get
			{
				return m_Grabbable;
			}
		}

		protected Collidable m_Drop = null;
		public Collidable Drop
		{
			get { return m_Drop; }
			set { m_Drop = value; }
		}

		protected float m_DestructionScore = 100;
		public float DestructionScore
		{
			get { return m_DestructionScore; }
			set { m_DestructionScore = value; }
		}

		protected SpriteParticleSystem m_DamageEffect = null;

		public void setDamageEffect( String p_DamageEffectTextureName, String p_Tag ) {
			m_DamageEffect = new ExplosionSpriteParticleSystem(Name + "_DamageEffectParticleSystem", p_DamageEffectTextureName, p_Tag, 1);
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(1f));
			EffectTask.addTask(new TaskRemove());
			m_DamageEffect.Task = EffectTask;
			addSprite(m_DamageEffect);
		}
		public void setDamageEffect(String p_DamageEffectTextureName, String p_Tag, String p_DamageEffectAnimationName, int p_AnimationFPS )
		{
			m_DamageEffect = new ExplosionSpriteParticleSystem(Name + "_DamageEffectParticleSystem", p_DamageEffectTextureName, p_Tag, p_DamageEffectAnimationName, p_AnimationFPS, 1);
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(1f));
			EffectTask.addTask(new TaskRemove());
			m_DamageEffect.Task = EffectTask;
			addSprite(m_DamageEffect);
		}

		protected SpriteParticleSystem m_DeathEffect = null;

		public void setDeathEffect(String p_DeathEffectTextureName, String p_Tag)
		{
			m_DeathEffect = new ExplosionSpriteParticleSystem(Name + "_DeathEffectParticleSystem", p_DeathEffectTextureName, p_Tag, 1);
			m_DeathEffect.MaxLifetime = 1.0f;
			m_DeathEffect.MinInitialSpeed = 10;
			m_DeathEffect.MaxInitialSpeed = 100;
			m_DeathEffect.MinNumParticles = 100;
			m_DeathEffect.MaxNumParticles = 200;
			m_DeathEffect.MinScale = 1f;
			m_DeathEffect.MaxScale = 0.001f;
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(2f));
			EffectTask.addTask(new TaskRemove());
			m_DeathEffect.Task = EffectTask;
			addSprite(m_DeathEffect);
		}
		public void setDeathEffect(String p_DeathEffectTextureName, String p_Tag, String p_DeathEffectAnimationName, int p_AnimationFPS)
		{
			m_DeathEffect = new ExplosionSpriteParticleSystem(Name + "_DeathEffectParticleSystem", p_DeathEffectTextureName, p_Tag, p_DeathEffectAnimationName, p_AnimationFPS, 1);
			m_DeathEffect.MaxLifetime = 1.0f;
			m_DeathEffect.MinInitialSpeed = 10;
			m_DeathEffect.MaxInitialSpeed = 100;
			m_DeathEffect.MinNumParticles = 100;
			m_DeathEffect.MaxNumParticles = 200;
			m_DeathEffect.MinScale = 1f;
			m_DeathEffect.MaxScale = 0.001f;
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(2f));
			EffectTask.addTask(new TaskRemove());
			m_DeathEffect.Task = EffectTask;
			addSprite(m_DeathEffect);
		}

		//this is the radius used for collision detection
		protected float m_Radius = 100f;
		public virtual float Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}

		// The damage that should be incurred on a collision against this collidable, in damage per second
		protected float m_Damage = 100;
		public float Damage
		{
			get
			{
				return m_Damage;
			}
			set
			{
				m_Damage = value;
			}
		}

		protected Queue<Collidable> didCollide = new Queue<Collidable>();

		#endregion // End of variables and Properties Region

		public Collidable() {
			Name = "Unnamed Collidable";
		}
		public Collidable(Collidable p_Collidable)
		{
			if (p_Collidable.Animation != null)
			{
				Animation = new VisualEffect(p_Collidable.Animation.Name, this, p_Collidable.Animation.FramesPerSecond);
			}
			BlendMode = p_Collidable.BlendMode;
			Bound = p_Collidable.Bound;
			Color = p_Collidable.Color;
			Damage = p_Collidable.Damage;
			Enabled = p_Collidable.Enabled;
			Faction = p_Collidable.Faction;
			Height = p_Collidable.Height;
			MaxHealth = p_Collidable.MaxHealth;
			Health = p_Collidable.Health;
			Name = p_Collidable.Name;
			Position = p_Collidable.Position;
			Radius = p_Collidable.Radius;
			Rotation = p_Collidable.Rotation;
			RotationDegrees = p_Collidable.RotationDegrees;
			Scale = p_Collidable.Scale;
			Task = p_Collidable.Task;
			Texture = p_Collidable.Texture;
			ToBeRemoved = p_Collidable.ToBeRemoved;
			Transparency = p_Collidable.Transparency;
			Width = p_Collidable.Width;
			Z = p_Collidable.Z;

			// temp
			setDamageEffect("Explosion", "3", "Explosion", 23);
			setDeathEffect("ExplosionBig", "");
		}
		public Collidable(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Enabled,
							float p_Rotation, float p_Z, Factions p_Faction, float p_MaxHealth, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Transparency, p_Enabled, p_Rotation, p_Z)
		{
			Faction = p_Faction;
			MaxHealth = p_MaxHealth;
			Radius = p_Radius;
		}


		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			while (didCollide.Count > 0 && !float.IsNaN(Health) && Health > 0)
			{
				Collidable DC = didCollide.Dequeue();
				float d;
				if (DC is Shot)
				{
					d = DC.Damage;
				}
				else if (float.IsNaN(DC.Health))
				{
					d = DC.Damage * (float)p_Time.ElapsedGameTime.TotalSeconds;
				}
				else
				{
					d = (float)Math.Min(DC.Damage * p_Time.ElapsedGameTime.TotalSeconds, DC.Health);
				}
				if (d > 0)
				{
					takeDamage(d, DC);
				}
				World.m_Score.evaluateCollision(this, DC, d, Health <= 0);
			}

			if (Position.Y > (Game.graphics.GraphicsDevice.Viewport.Height * 1.25) || Position.Y < (0 - (Game.graphics.GraphicsDevice.Viewport.Height * .25)) ||
				Position.X > Game.graphics.GraphicsDevice.Viewport.Width || Position.X < 0)
			{
				if (this is Shot)
				{
					((Shot)this).CheckShip();
				}
				else
				{
					//ToBeRemoved = true;
					if (this is Ship)
						//if (((Ship)this).Faction != Collidable.Factions.Player)
						((Ship)this).Health = 0;
				}
			}

			if(!(this is Shot))
				ToBeRemoved = IsDead();
		}

		public virtual Boolean IsDead()
		{
			if (MaxHealth == float.NaN)
			{
				return false;
			}
			else if (Health <= 0)
			{
				return true;
			}
			return false;
		}

		protected virtual void takeDamage(float damage, Collidable from)
		{
			Health = MathHelper.Clamp(Health - damage,0,MaxHealth);
			if (Health <= 0)
			{
				SpawnDeathEffect(Center);
				Dispose();
			}
			else if (damage > 0)
			{
				SpawnDamageEffect(Vector2.Lerp(Center, from.Center, 0.5f));
			}
		}

		public virtual void RegisterCollision(Collidable p_Other)
		{

			if (Faction != Factions.Blood && p_Other.Faction != Factions.Blood)
			{

				if (p_Other.Faction == Factions.Environment)
				{
					Center = Collision.GetMinNonCollidingCenter(this, p_Other);
				}
				else
				{
					if (!float.IsNaN(Health))
					{
						didCollide.Enqueue(p_Other);
					}
				}

			}
		}

		protected virtual void SpawnDamageEffect(Vector2 where)
		{
			if (m_DamageEffect != null)
			{
				m_DamageEffect.AddParticles(where);
			}
		}

		protected virtual void SpawnDeathEffect(Vector2 where)
		{
			if (m_DeathEffect != null)
			{
				m_DeathEffect.AddParticles(where);
			}
		}

		protected virtual void Dispose()
		{
			if (m_DamageEffect != null)
			{
				m_DamageEffect.Enabled = false;
				m_DamageEffect.ToBeRemoved = true;
			}

			if (m_DeathEffect != null)
			{
				TaskSequence temp = new TaskSequence();
				temp.addTask(new TaskTimer(2));
				temp.addTask(new TaskRemove());
				m_DeathEffect.Task = temp;
			}

		}

		public override void Draw(SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);
		}

		public virtual void captured()
		{
			if (Animation != null)
			{
				Animation.StopAnimation();
			}
		}
	}
}
