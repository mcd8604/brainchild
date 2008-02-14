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
		List<List<Weapon>> m_WeapUpgrades;

		TextSprite levelUp;
		TaskTimer levelTime;

		public const int MAX_LEVEL = 3;
		const int MAX_SHIELD_LEVEL = 200;
		const int HEAL_AMOUNT = 20;
		const int SHIELD_INC_AMOUNT = 10;

		int m_UpgradeLevel = 0;
		public int UpgradeLevel
		{
			get
			{
				return m_UpgradeLevel;
			}
			set
			{
				m_UpgradeLevel = value;
			}
		}

		int cur = -1;
		public int CurrentLevel
		{
			get
			{
				return cur + 1;
			}
		}

		public int LevelRequirement(int level)
		{
			if (level == 0)
			{
				return 0;
			}
			else if (level > m_UpgradeReqs.Count)
			{
				return int.MaxValue;
			}
			else
			{
				return m_UpgradeReqs[level - 1];
			}
		}




		public PlayerShip(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Visible, float p_Degree, float p_zBuff, Factions p_Faction, int p_Health, int p_Shield, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Transparency, p_Visible, p_Degree, p_zBuff, p_Faction, p_Health, p_Shield, p_Radius)
		{
			cur = -1;


			//code to create the level up text
			levelUp = new TextSprite("Level Up", Vector2.Zero, Microsoft.Xna.Framework.Graphics.Color.Yellow, Depth.HUDLayer.Midground + 0.001f, 100.0f, 0.0f, 50, 100);
			TextSprite levelUp2 = new TextSprite("Level Up", Vector2.Zero, Microsoft.Xna.Framework.Graphics.Color.Black, Depth.HUDLayer.Midground + 0.002f, 100.0f, 0.0f, 50, 100);
			levelUp2.Enabled = true;
			TaskAttachAt ta = new TaskAttachAt(levelUp, new Vector2(1f, 1f));
			levelUp2.Task = ta;
			levelUp.attachSpritePart(levelUp2);
			levelUp.Enabled = false;

			TaskParallel tp = new TaskParallel();
			levelTime = new TaskTimer(1.0f);

			ta = new TaskAttachAt(this, Vector2.Zero);
			tp.addTask(levelTime);
			tp.addTask(ta);

			levelUp.Task = tp;


			m_WeapUpgrades = new List<List<Weapon>>();
			for (int a = 0; a < MAX_LEVEL; a++)
			{
				m_WeapUpgrades.Add(new List<Weapon>());
			}

			m_Upgrades = new List<Shot>();
			m_UpgradeReqs = new List<int>();


			//Level 1 Upgrades 
			//***************************************************************************
			//Player main shot
			//Shot shot = new Shot();
			//shot.Name = "Player Shot1";
			//shot.Height = 20;
			//shot.Width = 20;
			//shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			//shot.Radius = 10;
			//shot.Damage = 8;
			//shot.Bound = Collidable.Boundings.Circle;
			//shot.setAnimation("shot_greenball", 30);
			//Weapon new_Weap = new WeaponStraight(this, shot, 0.3f, 300f, (float)Math.PI/2);
			//m_Upgrades.Add(shot);

			//Side Shot left
			Shot shot = new Shot();
			shot.Name = "Player Side Shot";
			shot.Height = 10;
			shot.Width = 30;
			shot.Texture = TextureLibrary.getGameTexture("Shot", "");
			shot.Radius = 10;
			shot.Damage = 1.0f;
			shot.Bound = Collidable.Boundings.Circle;
			//  shot1.BlendMode = Microsoft.Xna.Framework.Graphics.SpriteBlendMode.Additive;
			// shot1.setAnimation("shot_electric", 30);
			m_WeapUpgrades[0].Add(new WeaponStraight(this, shot, 0.2f, 500, (float)Math.PI));

			//Side Shot right
			shot = new Shot();
			shot.Name = "Player Side Shot";
			shot.Height = 10;
			shot.Width = 30;
			shot.Texture = TextureLibrary.getGameTexture("Shot", "");
			shot.Radius = 10;
			shot.Damage = 1.0f;
			shot.Bound = Collidable.Boundings.Circle;
			//   shot1.BlendMode = Microsoft.Xna.Framework.Graphics.SpriteBlendMode.Additive;
			//  shot1.setAnimation("shot_electric", 30);
			m_WeapUpgrades[0].Add(new WeaponStraight(this, shot, 0.2f, 500, 0));


			//Level 2
			//*********************************************************************
			shot = new Shot();
			shot.Name = "Player Homing Shot";
			shot.Height = 30;
			shot.Width = 30;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 15;
			shot.Damage = 6f;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_electric", 30);
			shot.BlendMode = Microsoft.Xna.Framework.Graphics.SpriteBlendMode.Additive;

			m_Upgrades.Add(shot);
			m_WeapUpgrades[1].Add( new WeaponSeekNearestTarget(this, shot, 1.5f, 300, Collidable.Factions.Enemy));


			// m_WeapUpgrades[1].Add(new_Weap);

			//Level 3
			//*********************************************************************
			






			m_UpgradeReqs.Add(50);
			m_UpgradeReqs.Add(125);
			m_UpgradeReqs.Add(225);
			m_UpgradeReqs.Add(350);
			m_UpgradeReqs.Add(500);
			m_UpgradeReqs.Add(700);
			
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			if (levelUp.Enabled)
			{
				levelUp.Update(p_Time);
			}
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);

			if (levelUp.Enabled)
			{
				if (!levelTime.IsComplete(null))
				{
					levelUp.Draw(p_SpriteBatch);
				}
				else
				{
					levelUp.Enabled = false;
				}

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
				return "Health: " + Convert.ToInt32(Math.Ceiling(Health)) + " Shield: " + Convert.ToInt32(Math.Ceiling(Shield));
			}
		}

		public override void RegisterCollision(Collidable p_Other)
		{
			if (p_Other.Faction == Factions.PowerUp)
			{
				PowerUp p = (PowerUp)p_Other;
				handlePowerUp(p);
			}
			else if (p_Other.Faction == Factions.ClearWall)
			{
				Center = Collision.GetMinNonCollidingCenter(this, p_Other);
			}
			base.RegisterCollision(p_Other);
		}

		void handlePowerUp(PowerUp p)
		{
			if (p.Type == PowerUp.PowerType.Weapon)
			{
				m_UpgradeLevel += p.Amount;
				int prev = cur;

				for (int a = 0; a < m_UpgradeReqs.Count; a++)
				{
					if (m_UpgradeLevel >= m_UpgradeReqs[a])
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

					levelUp.Enabled = true;
					levelTime.reset();

					// Weapons.Clear();
					//Weapons = m_WeapUpgrades[cur];
					foreach (Weapon w in Weapons)
					{
						foreach (Shot s in w.m_Shots)
						{
							s.Damage += s.Damage * 0.2f;
						}

						//w.changeShotType(s);
						w.Speed += w.Speed * 0.2f;
						w.Delay -= w.Delay * 0.2f;
					}
					if (cur < m_WeapUpgrades.Count)
					{
						foreach (Weapon w in m_WeapUpgrades[cur])
						{
							addWeapon(w);
						}
					}
				}
			}
			else if (p.Type == PowerUp.PowerType.Health)
			{
				Health += HEAL_AMOUNT;
				if (Health > MaxHealth)
				{
					Health = MaxHealth;
				}
			}
			else if (p.Type == PowerUp.PowerType.Shield)
			{
				m_MaxShield += SHIELD_INC_AMOUNT;
				m_Shield += SHIELD_INC_AMOUNT;
			}
		}

		public void ResetWeapons()
		{
			cur = -1;

			Weapons.Clear();

			Shot shot = new Shot();
			shot.Name = "Player Shot";
			shot.Height = 16;
			shot.Width = 16;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 8;
			shot.Damage = 4;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_greenball", 30);

			Weapon wep = new WeaponStraight(this, shot, 0.30f, 400, -MathHelper.PiOver2);
			this.addWeapon(wep);
		}

		public string getUpgradeLevel()
		{
			return "Upgrade Level: " + m_UpgradeLevel.ToString();
		}
	}
}
