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
				return cur+1;
			}
		}

		public int LevelRequirement(int level)
		{
			if (level == 0)
			{
				return 0;
			}
			return m_UpgradeReqs[level-1];
		}
		
		


		public PlayerShip(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Transparency, bool p_Visible, float p_Degree, float p_zBuff, Factions p_Faction, int p_Health, int p_Shield, float p_Radius)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Transparency, p_Visible, p_Degree, p_zBuff, p_Faction, p_Health, p_Shield, p_Radius)
		{

           

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
			Shot shot = new Shot();
			shot.Name = "Player Shot1";
			shot.Height = 20;
			shot.Width = 20;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 10;
			shot.Damage = 8;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_greenball", 30);
            Weapon new_Weap = new WeaponStraight(shot, 0.3f, 300f, (float)Math.PI/2);

            //Side Shot left
			m_Upgrades.Add(shot);
			m_UpgradeReqs.Add(100);
            Shot shot1 = new Shot();
            shot1.Name = "Player Side Shot";
            shot1.Height = 10;
            shot1.Width = 30;
            shot1.Texture = TextureLibrary.getGameTexture("Shot", "");
            shot1.Radius = 10;
            shot1.Damage = 1.0f;
            shot1.Bound = Collidable.Boundings.Circle;
            //  shot1.BlendMode = Microsoft.Xna.Framework.Graphics.SpriteBlendMode.Additive;
            // shot1.setAnimation("shot_electric", 30);
            new_Weap = new WeaponStraight(shot1, 0.2f, 500, (float)Math.PI);
            m_WeapUpgrades[0].Add(new_Weap);

            //Side Shot right
            shot1 = new Shot();
            shot1.Name = "Player Side Shot";
            shot1.Height = 10;
            shot1.Width = 30;
            shot1.Texture = TextureLibrary.getGameTexture("Shot", "");
            shot1.Radius = 10;
            shot1.Damage = 1.0f;
            shot1.Bound = Collidable.Boundings.Circle;
            //   shot1.BlendMode = Microsoft.Xna.Framework.Graphics.SpriteBlendMode.Additive;
            //  shot1.setAnimation("shot_electric", 30);

            new_Weap = new WeaponStraight(shot1, 0.2f, 500, 0);
            m_WeapUpgrades[0].Add(new_Weap);


           // m_WeapUpgrades[0].Add(new_Weap);

            //Level 2
            //*********************************************************************
			shot = new Shot();
			shot.Name = "Player Homing Shot";
			shot.Height = 40;
			shot.Width = 40;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 20;
            shot.Damage = 10.0f ;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_electric", 30);
            shot.BlendMode = Microsoft.Xna.Framework.Graphics.SpriteBlendMode.Additive;
            new_Weap = new WeaponSeekChangingTarget(shot, 1f, 300,Collidable.Factions.Enemy);

			m_Upgrades.Add(shot);
			m_UpgradeReqs.Add(200);
            m_WeapUpgrades[1].Add(new_Weap);            

   
           // m_WeapUpgrades[1].Add(new_Weap);

            //Level 3
            //*********************************************************************

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
                    
                   // Weapons.Clear();
                    //Weapons = m_WeapUpgrades[cur];
                    foreach (Weapon w in Weapons)
                    {
                        foreach (Shot s in w.m_Shots)
                        {
                                                        s.Damage += s.Damage * 0.2f;
                        }
                        
                        //w.changeShotType(s);
                        w.Speed += w.Speed * 0.1f;
                        //w.Delay -= w.Delay * 0.2f;
                    }
                    foreach (Weapon w in m_WeapUpgrades[cur])
					{
                        addWeapon(w);
						
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

		public string getUpgradeLevel()
		{
			return "Upgrade Level: " + m_UpgradeLevel.ToString();
		}
	}
}
