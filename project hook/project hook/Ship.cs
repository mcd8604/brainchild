using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/// <summary>
	/// Description: This class contians all information regarding ships, both player and enemy
	/// 
	/// TODO:
	/// 
	/// </summary>
	public class Ship : Collidable
	{
		//variable for the weapon that the ship currently has
		protected List<Weapon> m_Weapons = new List<Weapon>();

		private Sprite m_ShieldSprite;

		private SpriteParticleSystem m_ShieldDamageParticleSystem;

		public override float Radius
		{
			get
			{
				
				if (m_Shield > 0.0 && m_ShieldSprite != null)
				{					
					return m_ShieldSprite.Width / 2;
				}
				else
				{
					return base.Radius;
				}
			}
			
		}

		private float m_MaxShield = 0;
		public float MaxShield
		{
			get
			{
				return m_MaxShield;
			}
			set
			{
				m_MaxShield = value;
				m_Shield = m_MaxShield;
				if (m_MaxShield > 0)
				{
					if (m_ShieldSprite != null)
					{
						m_ShieldSprite.Enabled = true;
					}
					else
					{
						m_ShieldSprite = new Sprite("Shield", Vector2.Zero, (int)(Width * 1.30), (int)(Height * 1.30), TextureLibrary.getGameTexture("Shield", ""), 1f, true, 0, Depth.GameLayer.Shields);
						m_ShieldSprite.Task = new TaskAttach(this);
						attachSpritePart(m_ShieldSprite);

						GameTexture DamageEffect = TextureLibrary.getGameTexture("Explosion2", "3");
						m_ShieldDamageParticleSystem = new ExplosionSpriteParticleSystem(Name + "_BloodParticleSystem", Position, (int)(DamageEffect.Width * 0.5f), (int)(DamageEffect.Height * 0.5f), DamageEffect, 1.0f, true, 0, Depth.GameLayer.Explosion, 1);
						m_ShieldDamageParticleSystem.setAnimation("Explosion2", 10);
						m_ShieldDamageParticleSystem.Animation.StartAnimation();
						addSprite(m_ShieldDamageParticleSystem);
					}
				}
				else if (m_ShieldSprite != null)
				{
					m_ShieldSprite.Enabled = false;
				}
			}
		}

		private float m_Shield = 0;
		public float Shield
		{
			get
			{
				return m_Shield;
			}
			set
			{
				m_Shield = value;
			}
		}

		private float m_TimeSinceLastDamage = 0;
		private float m_ShieldRegenDelay = 5;
		/// <summary>
		/// Delay after taking damage, before the shield begins regenerating, in seconds.
		/// </summary>
		public float ShieldRegenDelay
		{
			get { return m_ShieldRegenDelay; }
			set { m_ShieldRegenDelay = value; }
		}

		private float m_ShieldRegenRate = 0.1f;
		/// <summary>
		/// The Rate at which the shield will regenerate, in percent per second.
		/// </summary>
		public float ShieldRegenRate
		{
			get { return m_ShieldRegenRate; }
			set { m_ShieldRegenRate = value; }
		}

		public Ship()
		{
			Z = Depth.GameLayer.Ships;
		}
		public Ship(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Rotation, float p_zBuff, Factions p_Faction, int p_Health, int p_MaxShield, GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Rotation, p_zBuff, p_Faction, p_Health, p_DamageEffect, p_Radius)
		{
			MaxShield = p_MaxShield;
		}

		public void addWeapon(Weapon w)
		{
			m_Weapons.Add(w);
			foreach (Shot s in w.getShots())
			{
				addSprite(s);
			}
		}

		public void shoot()
		{
			foreach (Weapon w in m_Weapons)
			{
				w.CreateShot(this);
			}
		}

		public List<Weapon> Weapons
		{
			get
			{
				return m_Weapons;
			}
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			if (m_MaxShield > 0 && m_ShieldSprite != null)
			{
				m_ShieldSprite.Transparency = ((float)m_Shield) / ((float)m_MaxShield);
			}
			foreach (Weapon w in m_Weapons)
			{
				w.Update(p_Time);
			}

			if (m_TimeSinceLastDamage > m_ShieldRegenDelay && m_Shield < m_MaxShield)
			{
				m_Shield = MathHelper.Clamp(m_Shield + (m_MaxShield * m_ShieldRegenRate) * (float)p_Time.ElapsedGameTime.TotalSeconds, 0, m_MaxShield);
			}
			m_TimeSinceLastDamage += (float)p_Time.ElapsedGameTime.TotalSeconds;
		}

		protected override void takeDamage(float damage)
		{
			m_TimeSinceLastDamage = 0;

			if (Shield > damage)
			{
				Shield -= damage;
				damage = 0;
			}
			else
			{
				damage -= Shield;
				Shield = 0;
			}

			if (damage > 0)
			{
				this.Health -= damage;
				damage = 0;
			}

			if (this.Health <= 0)
			{
				// death effect, and remove?
				PowerUp p = new PowerUp(this, World.m_Position);
				p.Enabled = true;
				p.MaxHealth = 1000;
				p.Height = this.Height / 2;
				p.Width = this.Width / 2;
				addSprite(p);

				foreach (Weapon w in m_Weapons)
				{
					foreach (Shot s in w.getShots())
						if(s.Enabled == false)
							s.Position = new Vector2(-10, -10);
				}
				Enabled = false;

			}
		}

		protected override void SpawnDamageEffect(Vector2 where)
		{
			if (m_Shield > 0)
			{
				if (m_ShieldDamageParticleSystem != null)
				{
					m_ShieldDamageParticleSystem.AddParticles(where);
				}
			}
			else
			{
				if (DamageParticleSystem != null)
				{
					DamageParticleSystem.AddParticles(where);
				}
			}
		}


	}
}