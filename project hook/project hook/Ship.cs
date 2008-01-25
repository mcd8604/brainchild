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

		private int m_MaxShield;
		public int MaxShield
		{
			get
			{
				return m_MaxShield;
			}
			set
			{
				m_MaxShield = value;
			}

		}

		private int m_Shield;
		public int Shield
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

		public Ship(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Rotation, float p_zBuff, Factions p_Faction, int p_Health, int p_Shield, GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Rotation, p_zBuff, p_Faction, p_Health, p_DamageEffect, p_Radius)
		{

			// fake logic:
			float tempangle = MathHelper.PiOver2;
			if (Faction == Factions.Player)
			{
				tempangle = -MathHelper.PiOver2;
			}


			//addWeapon(new WeaponExample(this, "FireBall", 10, 300, 500, MathHelper.PiOver2));
			//addWeapon(new WeaponExample(this, "FireBall", 10, 300, 500, MathHelper.PiOver4));
			//addWeapon(new WeaponExample(this, "FireBall", 10, 300, 500, MathHelper.PiOver4 * 3));

			//addWeapon(new WeaponExample(this, "RedShot", 10, 300, 500, tempangle));

			//m_Weapons.Add(new Weapon(this, 10, 300, 500, TextureLibrary.getGameTexture("RedShot", "1")));
			//m_Weapons.Add(new Weapon_SideShot(this, 10, 200, 500, TextureLibrary.getGameTexture("FireBall", "1")));

			m_Shield = p_Shield;
			
			m_MaxShield = m_Shield;


			if (m_MaxShield > 0)
			{
				m_ShieldSprite = new Sprite("Shield", Vector2.Zero, (int)(p_Width * 1.30), (int)(p_Height * 1.30), TextureLibrary.getGameTexture("Shield", ""), 255f, true, 0, Depth.MidGround.Bottom);
				m_ShieldSprite.Task = new TaskAttach(this);
				attachSpritePart(m_ShieldSprite);
			}
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
				int damage = shot.Damage;

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
