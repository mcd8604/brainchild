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

		protected Sprite m_ShieldSprite;

		protected SpriteParticleSystem m_ShieldDamageEffect = null;

		public void setShieldDamageEffect(String p_ShieldDamageEffectTextureName, String p_Tag)
		{
			m_ShieldDamageEffect = new ExplosionSpriteParticleSystem(Name + "_ShieldDamageEffectParticleSystem", p_ShieldDamageEffectTextureName, p_Tag, 1);
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(1f));
			EffectTask.addTask(new TaskRemove());
			m_ShieldDamageEffect.Task = EffectTask;
			addSprite(m_ShieldDamageEffect);
		}
		public void setShieldDamageEffect(String p_ShieldDamageEffectTextureName, String p_Tag, String p_ShieldDamageEffectAnimationName, int p_AnimationFPS)
		{
			m_ShieldDamageEffect = new ExplosionSpriteParticleSystem(Name + "_ShieldDamageEffectParticleSystem", p_ShieldDamageEffectTextureName, p_Tag, p_ShieldDamageEffectAnimationName, p_AnimationFPS, 1);
			TaskQueue EffectTask = new TaskQueue();
			EffectTask.addTask(new TaskWait(this.IsDead));
			EffectTask.addTask(new TaskTimer(1f));
			EffectTask.addTask(new TaskRemove());
			m_ShieldDamageEffect.Task = EffectTask;
			addSprite(m_ShieldDamageEffect);
		}

		public override float Radius
		{
			get
			{

				if (m_Shield > 0f && m_ShieldSprite != null)
				{
					return m_ShieldSprite.Width / 2;
				}
				else
				{
					return base.Radius;
				}
			}

		}

		protected float m_MaxShield = 0;
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
						m_ShieldSprite.Center = Center;
						TaskParallel ShieldTask = new TaskParallel();
						ShieldTask.addTask(new TaskAttach(this));
						ShieldTask.addTask(new TaskRotateWithTarget(this));
						m_ShieldSprite.Task = ShieldTask;
						attachSpritePart(m_ShieldSprite);
					}
				}
				else if (m_ShieldSprite != null)
				{
					m_ShieldSprite.Enabled = false;
				}
			}
		}

		protected float m_Shield = 0;
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

		protected float m_TimeSinceLastDamage = 0;
		protected float m_ShieldRegenDelay = 5;
		/// <summary>
		/// Delay after taking damage, before the shield begins regenerating, in seconds.
		/// </summary>
		public float ShieldRegenDelay
		{
			get { return m_ShieldRegenDelay; }
			set { m_ShieldRegenDelay = value; }
		}

		protected float m_ShieldRegenRate = 0.1f;
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
			Name = "Unnamed Ship";
			Z = Depth.GameLayer.Ships;
		}
		public Ship(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Visible, float p_Rotation, float p_zBuff, Factions p_Faction, int p_MaxHealth, int p_MaxShield, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Transparency, p_Visible, p_Rotation, p_zBuff, p_Faction, p_MaxHealth, p_Radius)
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

		protected override void takeDamage(float damage, Collidable from)
		{
			m_TimeSinceLastDamage = 0;

			if (Shield > 0)
			{
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
				SpawnShieldDamageEffect(Vector2.Lerp(Center, from.Center, 0.5f));
			}

			base.takeDamage(damage, from);

			if (Health <= 0)
			{
				// death effect, and remove?

				// TODO : Replace this 'static' creation with a key read from the xml, and stored in ship until it dies, then the 'drop' is dropped.
				if (Drop != null)
				{
					addSprite(Drop);
				}
				else
				{
					addSprite(new PowerUp((int)(Height * 0.5f), (int)(Radius * 0.5f), Center));
				}

				Enabled = false;

			}
		}

		protected void SpawnShieldDamageEffect(Vector2 where)
		{
			if (m_ShieldDamageEffect != null)
			{
				m_ShieldDamageEffect.AddParticles(where);
			}
		}

		protected override void Dispose()
		{
			if (m_ShieldDamageEffect != null)
			{
				m_ShieldDamageEffect.Enabled = false;
				m_ShieldDamageEffect.ToBeRemoved = true;
			}

			foreach (Weapon w in m_Weapons)
			{
				foreach (Shot s in w.getShots())
					if (s.Enabled == false)
						s.Center = new Vector2(-100, -100);
			}

		}

		private VisualEffect m_ShootAnimation;
		public VisualEffect ShootAnimation
		{
			get
			{
				return m_ShootAnimation;
			}
		}

		public void setShootAnimation(string p_Animation, int p_FramesPerSecond)
		{
			m_ShootAnimation = new VisualEffect(p_Animation, this, p_FramesPerSecond, 1);
			m_ShootAnimation.CycleRemoval = false;
			m_ShootAnimation.StopAnimation();
		}

	}
}