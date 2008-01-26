using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

/*
 * Description: This class contians all information regarding ships, both player and enemy
 * 
 * TODO:
 *  
 */
namespace project_hook
{
	public class Ship : Collidable
	{
		//variable for the weapon that the ship currently has
		protected List<Weapon> m_Weapons = new List<Weapon>();

		private Sprite m_ShieldSprite;

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
					m_ShieldSprite = new Sprite("Shield", Vector2.Zero, (int)(Width * 1.30), (int)(Height * 1.30), TextureLibrary.getGameTexture("Shield", ""), 1f, true, 0, Depth.GameLayer.Shields);
					m_ShieldSprite.Task = new TaskAttach(this);
					attachSpritePart(m_ShieldSprite);
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
				w.CreateShot( this );
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

			if (m_MaxShield > 0 && m_ShieldSprite !=null)
			{
				m_ShieldSprite.Transparency = ((float)m_Shield) / ((float)m_MaxShield);
			}
			foreach (Weapon w in m_Weapons)
			{
				w.Update(p_Time);
			}
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other is Shot)
			{
				if (this.Health <= 0)
				{
					// death effect, and remove?
					Enabled = false;
				}

				Shot shot = (Shot)p_Other;
				float damage = shot.Damage;

				if (Shield < damage)
				{
					damage -= Shield;
					Shield = 0;
				}
				else{
					Shield -= damage;
					damage = 0;
				}

				if (damage > 0)
				{
					this.Health -= damage;
					damage = 0;
				}

				//Possible attach the explosion sprite to the ship
			}
			else if (p_Other.Faction == Factions.Environment)
			{
				// really bad collision pushback stuff..
				// temporary?
				Vector2 temp = Center;
				float deltaX = Center.X - p_Other.Center.X;
				float deltaY = Center.Y - p_Other.Center.Y;

				if (Math.Abs(deltaX) > Math.Abs(deltaY))
				{

					// horizontal collision
					if (temp.X > p_Other.Center.X)
					{
						temp.X = p_Other.Center.X + Radius + p_Other.Radius;
					}
					else
					{
						temp.X = p_Other.Center.X - (Radius + p_Other.Radius);
					}

				}
				else
				{

					// vertical
					if (temp.Y > p_Other.Center.Y)
					{
						temp.Y = p_Other.Center.Y + Radius + p_Other.Radius;
					}
					else
					{
						temp.Y = p_Other.Center.Y - (Radius + p_Other.Radius);
					}

				}

				Center = temp;

			}

			base.RegisterCollision(p_Other);
		}
	}
}
