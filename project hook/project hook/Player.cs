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

        Vector2 m_PlayerSpeed = new Vector2(0, 0);
        int m_PlayerAcceleration = 10;
        int m_PlayerFriction = 5;

        //delay between swimming animation (and possibly bursts)
        double m_MovementDelay = 1.5;

        public Player(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree)
        {
            m_PlayerShip = new PlayerShip(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree);
        }

        public void MoveUp(GameTime p_GameTime)
        {
            m_MovementDelay -= p_GameTime.ElapsedGameTime.Seconds;

            if(m_MovementDelay < 0)
            {
                m_PlayerSpeed.Y = m_PlayerAcceleration;
                m_MovementDelay = 1.5;
            }
        }

        public void DrawPlayer(GameTime p_GameTime, SpriteBatch p_SpriteBatch)
        {
            UpdatePlayerPosition(p_GameTime);
            
            //p_SpriteBatch.Draw(m_PlayerShip.Texture, m_PlayerShip.Position, Color.White);
        }

        private void UpdatePlayerPosition(GameTime p_GameTime)
        {
        }
    }
}
