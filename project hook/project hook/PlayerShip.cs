using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	/// <summary>
	/// Description: This class contains the information regarding the player ship,
	///              i.e. power up effects
	/// 
	/// TODO:
	/// 
	/// </summary>
	public class PlayerShip : Ship
	{

		Shot[] m_Upgrades;


		int m_UpgradeLevel = 0;
		int cur = -1;


		public PlayerShip(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, Factions p_Faction, int p_Health, int p_Shield, GameTexture p_DamageEffect, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, p_Faction, p_Health, p_Shield, p_DamageEffect, p_Radius)
		{
			m_Upgrades = new Shot[2];

			Shot shot = new Shot(this);
			shot.Name = "Player Shot";
			shot.Height = 30;
			shot.Width = 90;
			shot.Texture = TextureLibrary.getGameTexture("RedShot", "3");
			shot.Radius = 30;
			shot.Damage = 1;
			shot.Bound = Collidable.Boundings.Diamond;
			shot.setAnimation("RedShot", 10);

			m_Upgrades[0] = shot;


			shot = new Shot(this);
			shot.Name = "Player Shot";
			shot.Height = 60;
			shot.Width = 110;
			shot.Texture = TextureLibrary.getGameTexture("RedShot", "3");
			shot.Radius = 30;
			shot.Damage = 1;
			shot.Bound = Collidable.Boundings.Diamond;
			shot.setAnimation("RedShot", 10);

			m_Upgrades[1] = shot;






			Weapon wep = new WeaponStraight(shot, 0.3f, 400, -MathHelper.PiOver2);

		}

		//stores the current power up effects
		List<Effect> m_EffectsArray = new List<Effect>();

		/// <summary>
		/// Description: Adds a new power up effec to the player ship.
		/// </summary>
		/// <param name="p_Effect"></param>
		public void AddEffect(Effect p_Effect)
		{
			m_EffectsArray.Add(p_Effect);
		}

		/// <summary>
		/// Description: This removes any effects that have expired.
		/// </summary>
		public void CheckEffects()
		{
			foreach (Effect i_Effect in m_EffectsArray)
			{
				if (i_Effect.Expired())
					m_EffectsArray.Remove(i_Effect);
			}
		}

		public override string ToString()
		{
			if (float.IsNaN(MaxHealth))
			{
				return "Invulnerable";
			}
			else
			{
				return "Health: " + Convert.ToInt32(Health) + " Shield: " + Convert.ToInt32(Shield);
			}
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other.Faction == Factions.PowerUp)
			{
				PowerUp p = (PowerUp)p_Other;
				m_UpgradeLevel += p.Amount;
				//Console.WriteLine("Upgrade: " + m_UpgradeLevel);
				int prev = cur;
				for (int a = 0; a < m_Upgrades.Length; a++)
				{
					if (m_UpgradeLevel > (a * 100 + 100))
					{
						prev = a;
					}
				}

				if (cur != prev)
				{
					foreach (Weapon w in Weapons)
					{
						foreach (Shot s in w.changeShotType(m_Upgrades[prev]))
						{
							addSprite(s);
						}
					}
					cur = prev;
				}


			}
			base.RegisterCollision(p_Other);


		}

		public string getUpgradeLevel()
		{
			return "Upgrade Level: " + m_UpgradeLevel.ToString();
		}

	}
}
