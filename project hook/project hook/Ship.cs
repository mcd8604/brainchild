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
		List<Weapon> m_Weapons;

		private Sprite m_ShieldSprite;

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

		public Ship(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, Factions p_Faction, int p_Health, int p_Shield, Path p_Path, int p_Speed, GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, p_Faction, p_Health, p_Path, p_Speed, p_DamageEffect, p_Radius)
		{
			m_Weapons = new List<Weapon>();
			m_Weapons.Add(new Weapon(this, 10, 300, 500, TextureLibrary.getGameTexture("RedShot", "1")));
			m_Weapons.Add(new Weapon_SideShot(this, 10, 200, 500, TextureLibrary.getGameTexture("FireBall", "1")));

			m_Shield = p_Shield;

			if (m_Shield > 0)
			{
				m_ShieldSprite = new Sprite("Shield", Vector2.Zero, (int)(p_Width * 1.30), (int)(p_Height * 1.30), TextureLibrary.getGameTexture("Shield", ""), 255f, true, 0, Depth.MidGround.Bottom);
				Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();

				dic.Add(PathStrategy.ValueKeys.Target, this);
				dic.Add(PathStrategy.ValueKeys.Base, m_ShieldSprite);
				m_ShieldSprite.Path = new Path(Paths.Follow, dic);

				attachSpritePart(m_ShieldSprite);
			}
		}

		public void shoot(GameTime p_GameTime)
		{
			foreach (Weapon w in m_Weapons)
			{
				List<Sprite> shots = w.CreateShots(p_GameTime);
				if (shots != null)
				{
					addSprites(shots);
				}
			}
		}

		public List<Weapon> Weapons
		{
			get
			{
				return m_Weapons;
			}
			set
			{
				m_Weapons = value;
			}
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			if (m_ShieldSprite != null)
			{
				m_ShieldSprite.Visible = (Shield > 0);
			}
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other is Shot)
			{
				if (this.Health <= 0)
				{
					// death effect, and remove?
				}
				Shot shot = (Shot)p_Other;
				this.Health -= shot.Damage;

				//Possible attach the explosion sprite to the ship
			}

			base.RegisterCollision(p_Other);
		}
	}
}
