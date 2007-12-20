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
    class Player
    {
        //variable for storing the player ship sprite and info
        PlayerShip m_PlayerShip;

        Vector2 m_PlayerSpeed = new Vector2(0, 0); //The distance the player sprite is going to move next time it is drawn
		Vector2 m_PlayerSpeedBuffer = new Vector2(0, 0);
        int m_PlayerAcceleration = 10; //The increase in speed that the will happen upon a movement call
		int m_PlayerAccelerationMax = 15;
        int m_PlayerFriction = 2; //The rate at which the player sprite slows down

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
            m_PlayerShip = new PlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff);
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

		private void CalcMovement(GameTime p_GameTime, Vector2 p_PlayerSpeedBuffer)
		{
			m_MovementDelay -= p_GameTime.ElapsedGameTime.TotalSeconds;

			if (m_MovementDelay < 0)
			{
				MathHelper.Clamp(m_PlayerSpeed.X += p_PlayerSpeedBuffer.X, -m_PlayerAccelerationMax, m_PlayerAccelerationMax);
				MathHelper.Clamp(m_PlayerSpeed.Y += p_PlayerSpeedBuffer.Y, -m_PlayerAccelerationMax, m_PlayerAccelerationMax);
				m_MovementDelay = m_MovementDelayReset;
			}
		}

		/// <summary>
		/// This function is called to draw the player ship to the screen. All movement functions should be called before this.
		/// </summary>
		/// <param name="p_GameTime"></param>
		/// <param name="p_SpriteBatch"></param>
        public void DrawPlayer(GameTime p_GameTime, SpriteBatch p_SpriteBatch)
        {
			//Calculate player position based on player speed and player friction
			CalcMovement(p_GameTime, m_PlayerSpeedBuffer);
			Vector2 tempPlayerPosition = m_PlayerShip.Position;
			tempPlayerPosition.X += ((m_PlayerSpeed.X - m_PlayerFriction) * (float)(p_GameTime.ElapsedGameTime.Seconds));
			tempPlayerPosition.Y += ((m_PlayerSpeed.Y - m_PlayerFriction) * (float)(p_GameTime.ElapsedGameTime.Seconds));
			m_PlayerShip.Position = tempPlayerPosition;

            p_SpriteBatch.Draw(m_PlayerShip.Texture.Texture, m_PlayerShip.Position, Color.White);
        }
    }
}
