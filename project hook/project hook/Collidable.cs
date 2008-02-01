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

		private Boundings m_Bound = Boundings.Circle;
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

		private Factions m_Faction = Factions.None;
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

		private float m_MaxHealth = float.NaN;
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
		private float m_Health = 0;
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

		//this is what effect will be shown on the sprite when it takes damage
		private GameTexture m_DamageEffect = null;
		public GameTexture DamageEffect
		{
			get
			{
				return m_DamageEffect;
			}
			set
			{
				m_DamageEffect = value;
				if (DamageEffect != null)
				{
					m_DamageParticleSystem = new ExplosionSpriteParticleSystem(Name + "_BloodParticleSystem", Position, DamageEffect.Width, DamageEffect.Height, DamageEffect, 255.0f, true, 0, Z, 1);
					DamageParticleSystem.setAnimation("Explosion", 10);
					DamageParticleSystem.Animation.StartAnimation();
					addSprite(DamageParticleSystem);
				}
			}
		}

		//this is the sprite for teh damage effect
		private Sprite m_DamageSprite;

		private SpriteParticleSystem m_DamageParticleSystem;
		public SpriteParticleSystem DamageParticleSystem
		{
			get
			{
				return m_DamageParticleSystem;
			}
		}

		//this is the radius used for collision detection
		private float m_Radius = 100f;
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

		private GameTexture m_CollisonEffect;
		public GameTexture CollisonEffect
		{
			get
			{
				return m_CollisonEffect;
			}
			set
			{
				m_CollisonEffect = value;
			}
		}

		// The damage that should be incurred on a collision against this collidable, in damage per second
		private float m_Damage = 100;
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

		protected Collidable didCollide = null;

		#endregion // End of variables and Properties Region

		public Collidable() { }
		public Collidable(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Enabled,
							float p_Rotation, float p_Z, Factions p_Faction, float p_MaxHealth, GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Transparency, p_Enabled, p_Rotation, p_Z)
		{
			Faction = p_Faction;
			MaxHealth = p_MaxHealth;
			DamageEffect = p_DamageEffect;
			if (DamageEffect != null)
			{
				m_DamageParticleSystem = new ExplosionSpriteParticleSystem(Name + "_BloodParticleSystem", Position, DamageEffect.Width, DamageEffect.Height, DamageEffect, 255.0f, true, 0, Z, 1);
				DamageParticleSystem.setAnimation("Explosion", 10);
				DamageParticleSystem.Animation.StartAnimation();
				attachSpritePart(DamageParticleSystem);
			}
			Radius = p_Radius;
		}


		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			if (didCollide != null)
			{
				float d;
				if (didCollide is Shot)
				{
					d = didCollide.Damage;
				}
				else if (float.IsNaN(didCollide.Health))
				{
					d = didCollide.Damage * (float)p_Time.ElapsedGameTime.TotalSeconds;
				}
				else
				{
					d = (float)Math.Min(didCollide.Damage * p_Time.ElapsedGameTime.TotalSeconds, didCollide.Health);
				}
				if (d > 0)
				{
					takeDamage(d);
				}
				World.m_Score.evaluateCollision(this, didCollide, d, Health <= 0);
				didCollide = null;
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

		protected virtual void takeDamage(float damage)
		{
			MathHelper.Clamp(Health -= damage,0,MaxHealth);
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
						didCollide = p_Other;
						SpawnDamageEffect(Vector2.Lerp(Center, p_Other.Center, 0.5f));
					}
				}

			}
		}

		protected virtual void SpawnDamageEffect(Vector2 where)
		{
			if (DamageEffect != null)
			{
				DamageParticleSystem.AddParticles(where);
			}

			if (this is Shot)
				Position = new Vector2(-1, 0);
		}

		public override void Draw(SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);

			if (m_DamageSprite != null)
			{
				m_DamageSprite.Draw(p_SpriteBatch);
			}
		}


		public override string ToString()
		{
			return Name.ToString() + " " + Center.ToString() + " " + Bound.ToString() + " " + Radius.ToString();
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
