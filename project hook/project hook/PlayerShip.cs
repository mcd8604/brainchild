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

		List<Shot> m_Upgrades;
		List<int> m_UpgradeReqs;


		int m_UpgradeLevel = 0;
		int cur = -1;


		public PlayerShip(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, Factions p_Faction, int p_Health, int p_Shield, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, p_Faction, p_Health, p_Shield, p_Radius)
		{
			m_Upgrades = new List<Shot>();
			m_UpgradeReqs = new List<int>();

			Shot shot = new Shot();
			shot.Name = "Player Shot";
			shot.Height = 20;
			shot.Width = 20;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 10;
			shot.Damage = 8;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_greenball", 30);

			m_Upgrades.Add(shot);
			m_UpgradeReqs.Add(100);

			shot = new Shot();
			shot.Name = "Player Shot";
			shot.Height = 24;
			shot.Width = 24;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 12;
			shot.Damage = 10;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_greenball", 30);

			m_Upgrades.Add(shot);
			m_UpgradeReqs.Add(200);

			shot = new Shot();
			shot.Name = "Player Shot";
			shot.Height = 28;
			shot.Width = 28;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 14;
			shot.Damage = 12;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_greenball", 30);

			m_Upgrades.Add(shot);
			m_UpgradeReqs.Add(300);

		}

		public override string ToString()
		{
			if (float.IsNaN(MaxHealth))
			{
				return "Invulnerable";
			}
			else
			{
				return "Health: " + Convert.ToInt32(Math.Ceiling(Health)) + " Shield: " + Convert.ToInt32(Math.Ceiling(Shield));
			}
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other.Faction == Factions.PowerUp)
			{
				PowerUp p = (PowerUp)p_Other;
				m_UpgradeLevel += p.Amount;
				int prev = cur;

				for (int a = 0; a < m_UpgradeReqs.Count; a++)
				{
					if (m_UpgradeLevel > m_UpgradeReqs[a])
					{
						prev = a;
					}
					else
					{
						break;
					}
				}

				if (cur != prev)
				{
					cur = prev;
					foreach (Weapon w in Weapons)
					{
						foreach (Shot s in w.changeShotType(m_Upgrades[cur]))
						{
							addSprite(s);
						}
					}
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
