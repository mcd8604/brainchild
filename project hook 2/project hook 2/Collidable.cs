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
			Square
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

		private Factions m_Faction;
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

		//this is the speed of the sprite
		private float m_Speed = 0f;
		public float Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
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
					m_DamageParticleSystem = new ExplosionSpriteParticleSystem(Name + "_BloodParticleSystem", Position, DamageEffect.Width, DamageEffect.Height, DamageEffect, 255.0f, true, 0, this.Z, 1);
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
		public float Radius
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
				m_DamageParticleSystem = new ExplosionSpriteParticleSystem(Name + "_BloodParticleSystem", Position, DamageEffect.Width, DamageEffect.Height, DamageEffect, 255.0f, true, 0, this.Z, 1);
				DamageParticleSystem.setAnimation("Explosion", 10);
				DamageParticleSystem.Animation.StartAnimation();
				addSprite(DamageParticleSystem);
			}
			Radius = p_Radius;
		}


		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			if (didCollide != null)
			{
				float d = (float)Math.Min(didCollide.Damage * p_Time.ElapsedGameTime.TotalSeconds, didCollide.Health);
				if (d > 0)
				{
					takeDamage(d);
				}
				didCollide = null;
			}

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
			Health -= damage;
		}

		//public virtual void CheckCollision(Collidable p_Sprite)
		//{
		//to do:	send it's self and p_Sprite to the gameCollision class, and check for a collision.
		//			if there is a collision do some action.  no action at this point.
		//}

		public virtual void RegisterCollision(Collidable p_Other)
		{

			if (p_Other.Faction == Factions.Environment)
			{
				Center = Collision.GetMinNonCollidingCenter(this, p_Other);
			}
			else
			{
				if (Health != float.NaN)
				{
					didCollide = p_Other;
					SpawnDamageEffect(Vector2.Lerp(this.Center, p_Other.Center, 0.5f));
				}
			}
		}

		protected virtual void SpawnDamageEffect(Vector2 where)
		{
			if (DamageEffect != null)
			{
				DamageParticleSystem.AddParticles(where);
			}

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

	}
}
