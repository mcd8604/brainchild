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
		Weapon m_Weapon; 

        public Ship(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff, Factions p_Faction, int p_Health, Path p_Path, int p_Speed, GameTexture p_DamageEffect, float p_Radius)
            : base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff, p_Faction, p_Health, p_Path, p_Speed, p_DamageEffect, p_Radius)
        {
			m_Weapon = new Weapon(this, 10, 0, 5, TextureLibrary.getGameTexture("RedShot", "1"));
			Sprite shield = new Sprite("Shield", Vector2.Zero, (int)(p_Width *1.30), (int)(p_Height * 1.30), TextureLibrary.getGameTexture("Shield",""), 100, true, 0, Depth.MidGround.Bottom);
			Dictionary<String,Object> dic = new Dictionary<string,object>();
			dic.Add("Target",this);
			dic.Add("Base",shield);
			shield.Path = new Path(Path.Paths.Follow, dic);
			attachSpritePart(shield);
			
			
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
