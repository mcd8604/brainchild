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
    class Ship : Collidable 
    {
        //variable for the weapon that the ship currently has
        Weapon m_Weapon = new Weapon();

        public Ship(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree)
            : base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree)
        {

        }

        public Weapon Weapon
        {
            get
            {
                return m_Weapon;
            }
            set
            {
                m_Weapon = value;
            }
        }
    }
}
