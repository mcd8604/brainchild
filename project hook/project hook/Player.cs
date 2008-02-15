using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{
	/// <summary>
	/// Description: This class contains information regarding the player 
	/// 
	/// TODO:
	/// 1. Create a function to tell the player to move in each direction
	/// 2. Create a function to tell the player to shoot
	/// 3. Implement all tail functionality
	/// </summary>
	public class Player
	{
		const int INITIAL_HEALTH = 100;
		const int INITIAL_SHIELD = 75;

		//variable for storing the player ship sprite and info
		PlayerShip m_PlayerShip;

		Score m_Score;

		public PlayerShip PlayerShip
		{
			get
			{
				return m_PlayerShip;
			}
			//set
			//{
			//m_PlayerShip = value;
			//}
		}

		public Score Score
		{
			get
			{
				return m_Score;
			}

			set
			{
				m_Score = value;
			}
		}

		protected Rectangle m_Bounds;
		public Rectangle Bounds
		{
			get
			{
				return m_Bounds;
			}
			set
			{
				m_Bounds = value;
			}

		}

		Vector2 m_PlayerSpeed = new Vector2(0, 0); //The distance the player sprite is going to move next time it is drawn
		Vector2 m_PlayerSpeedBuffer = new Vector2(0, 0);
		int m_PlayerAcceleration = 100; //The increase in speed that the will happen upon a movement call
		int m_PlayerSpeedMax = 400;
		float m_PlayerFriction = .75f; //The rate at which the player sprite slows down

		//delay between swimming animation (and possibly bursts)
		double m_MovementDelay = 0;
		double m_MovementDelayReset = .2;

		/// <summary>
		///  Full constructor requires all arguments for all sprite creation.
		/// </summary>
		/// <param name="p_Name"></param>
		/// <param name="p_Position"></param>
		/// <param name="p_Height"></param>
		/// <param name="p_Width"></param>
		/// <param name="p_Texture"></param>
		/// <param name="p_Alpha"></param>
		/// <param name="p_Visible"></param>
		/// <param name="p_Degree"></param>
		/// <param name="p_zBuff"></param>
		public Player(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, Rectangle p_Bounds)
		{
			CreatePlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff);
			Bounds = p_Bounds;
			m_Score = new Score(0);
		}

		public Player(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, Rectangle p_Bounds, int p_Score)
		{
			Bounds = p_Bounds;
			//m_PlayerShip = new PlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, Collidable.Factions.Player, 0, null, 0, null, 0);
			CreatePlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff);
			m_Score = new Score((ulong)p_Score);
		}

		/// <summary>
		/// This is called to alert the player ship that it should move up
		/// </summary>
		/// <param name="p_GameTime"></param>
		public void MoveUp()
		{
			m_PlayerSpeedBuffer.Y -= m_PlayerAcceleration;
		}

		public void MoveDown()
		{
			m_PlayerSpeedBuffer.Y += m_PlayerAcceleration;
		}

		public void MoveRight()
		{
			m_PlayerSpeedBuffer.X += m_PlayerAcceleration;
		}

		public void MoveLeft()
		{
			m_PlayerSpeedBuffer.X -= m_PlayerAcceleration;
		}

		public void CreatePlayerShip(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff)
		{

			m_PlayerShip = new PlayerShip(
#if !FINAL
				p_Name,
#endif
				p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, Collidable.Factions.Player, INITIAL_HEALTH, INITIAL_SHIELD, p_Width / 2.0f);
			m_PlayerShip.setDamageEffect("Explosion", "3", "Explosion", 23);
			m_PlayerShip.setShieldDamageEffect("Explosion2", "3", "Explosion2", 23);
			m_PlayerShip.setDeathEffect("ExplosionBig", "");
			m_PlayerShip.ResetWeapons();
			m_PlayerShip.setAnimation("wing", 40);
			m_PlayerShip.Animation.StartAnimation();

			/*
			Shot shot = new Shot();
			shot.Name = "Player Shot";
			shot.Height = 16;
			shot.Width = 16;
			shot.Texture = TextureLibrary.getGameTexture("shot_greenball", "0");
			shot.Radius = 8;
			shot.Damage = 4;
			shot.Bound = Collidable.Boundings.Circle;
			shot.setAnimation("shot_greenball", 30);

			Weapon wep = new WeaponStraight(shot, 0.30f, 400, -MathHelper.PiOver2);
			m_PlayerShip.addWeapon(wep);
			 * */
		}



		public void reset()
		{
			m_PlayerShip.MaxHealth = INITIAL_HEALTH;
			m_PlayerShip.MaxShield = INITIAL_SHIELD;
			m_PlayerShip.Health = m_PlayerShip.MaxHealth;
			m_PlayerShip.Shield = m_PlayerShip.MaxShield;
			m_PlayerShip.UpgradeLevel = 0;
			m_PlayerShip.Enabled = true;
			m_PlayerShip.ToBeRemoved = false;
			m_PlayerShip.ResetWeapons();

			foreach (Weapon w in m_PlayerShip.Weapons)
			{
				foreach (Shot s in w.m_Shots)
				{
					s.m_Ship = PlayerShip;
					m_PlayerShip.addSprite(s);
				}
			}
		}

		public void Shoot()
		{
			m_PlayerShip.shoot();
		}

		private void CalcMovement(GameTime p_GameTime, Vector2 p_PlayerSpeedBuffer)
		{
			//m_MovementDelay -= p_GameTime.ElapsedGameTime.TotalSeconds;

			//if (m_MovementDelay < 0)
			//{
			MathHelper.Clamp(m_PlayerSpeed.X += p_PlayerSpeedBuffer.X, -m_PlayerSpeedMax, m_PlayerSpeedMax);
			MathHelper.Clamp(m_PlayerSpeed.Y += p_PlayerSpeedBuffer.Y, -m_PlayerSpeedMax, m_PlayerSpeedMax);
			m_MovementDelay = m_MovementDelayReset;
			m_PlayerSpeedBuffer.X = 0;
			m_PlayerSpeedBuffer.Y = 0;
			//}
		}

		/// <summary>
		/// This function is called to draw the player ship to the screen. All movement functions should be called before this
		/// </summary>
		/// <param name="p_GameTime"></param>
		/// <param name="p_SpriteBatch"></param>
		public void DrawPlayer(GameTime p_GameTime, SpriteBatch p_SpriteBatch)
		{


			//p_SpriteBatch.Draw(m_PlayerShip.Texture.Texture, m_PlayerShip.Position, Color.White);
			m_PlayerShip.Draw(p_SpriteBatch);
		}

		public void UpdatePlayer(GameTime p_GameTime)
		{
			//Calculate player position based on player speed and player friction
			CalcMovement(p_GameTime, m_PlayerSpeedBuffer);
			Vector2 tempPlayerCenter = m_PlayerShip.Center;
			m_PlayerSpeed.X *= m_PlayerFriction;
			m_PlayerSpeed.Y *= m_PlayerFriction;
			tempPlayerCenter.X += ((m_PlayerSpeed.X) * (float)(p_GameTime.ElapsedGameTime.TotalSeconds));
			tempPlayerCenter.Y += ((m_PlayerSpeed.Y) * (float)(p_GameTime.ElapsedGameTime.TotalSeconds));
			tempPlayerCenter.X = MathHelper.Clamp(tempPlayerCenter.X, m_Bounds.X + (float)PlayerShip.Width * 0.5f, m_Bounds.Width - (float)PlayerShip.Width * 0.5f);
			tempPlayerCenter.Y = MathHelper.Clamp(tempPlayerCenter.Y, m_Bounds.Y + (float)PlayerShip.Height * 0.5f, m_Bounds.Height - (float)PlayerShip.Height * 0.5f);
			m_PlayerShip.Center = tempPlayerCenter;
		}
	}
}
