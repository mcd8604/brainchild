using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

    /*
     * Description: This class contains information regarding the player 
     * 
     * TODO:
     *  1. Create a function to tell the player to move in each direction
     *  2. Create a function to tell the player to shoot
     *  3. Implement all tail functionality
     */
namespace project_hook
{
    public class Player
    {
        //variable for storing the player ship sprite and info
        PlayerShip m_PlayerShip;

		Score m_Score;

		public PlayerShip PlayerShip
		{
			get
			{
				return m_PlayerShip;
			}
			set
			{
				//m_PlayerShip = value;
			}
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
        public Player(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff)
        {
			this.ResetPlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff);
			m_Score = new Score(0);
		}

		public Player(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, int p_Score)
		{
			//m_PlayerShip = new PlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, Collidable.Factions.Player, 0, null, 0, null, 0);
			this.ResetPlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff);
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

		public void ResetPlayerShip(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff)
		{
			m_PlayerShip = new PlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, Collidable.Factions.Player, 1, 1, null, 0, null, 0);
		}

		public List<Shot> Shoot(GameTime p_GameTime)
		{
            return m_PlayerShip.shoot(p_GameTime);
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
		/// This function is called to draw the player ship to the screen. All movement functions should be called before this.
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
            Vector2 tempPlayerPosition = m_PlayerShip.Position;
            m_PlayerSpeed.X *= m_PlayerFriction;
            m_PlayerSpeed.Y *= m_PlayerFriction;
            tempPlayerPosition.X += ((m_PlayerSpeed.X) * (float)(p_GameTime.ElapsedGameTime.TotalSeconds));
            tempPlayerPosition.Y += ((m_PlayerSpeed.Y) * (float)(p_GameTime.ElapsedGameTime.TotalSeconds));
            tempPlayerPosition.X = MathHelper.Clamp(tempPlayerPosition.X, 0, Game1.graphics.GraphicsDevice.Viewport.Width);
			tempPlayerPosition.Y = MathHelper.Clamp(tempPlayerPosition.Y, 0, Game1.graphics.GraphicsDevice.Viewport.Height);
            m_PlayerShip.Position = tempPlayerPosition;

            PlayerShip.Update(p_GameTime);
        }
    }
}
