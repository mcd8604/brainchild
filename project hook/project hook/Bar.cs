using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
    class Bar
    {        //the ship who's shield and health will be displayed
        Collidable m_Target;

        //The poistion the shields and heatlh will be drawn
        Sprite shields;
        Sprite blackS;
        
        Sprite health;
        Sprite blackH;
     
        int width;
        int height;

        int maxVal;
        int curVal;

        bool direction = false;
        bool ChangeTextureSource = false;
        Vector2 offset;

        private Sprite m_BarFrontSprite;
        private Sprite m_BarBackSprite;



        public Bar() {
          
        }

        public Bar(GameTexture p_FrontTexture, GameTexture p_BackTexture, Vector2 pos, int width, int height,int maxValue)
        {
            GameTexture frontText = new GameTexture(p_FrontTexture.Name, p_FrontTexture.Tag, p_FrontTexture.Texture, p_FrontTexture.StartPosition);
            GameTexture backText = new GameTexture(p_BackTexture.Name, p_BackTexture.Tag, p_BackTexture.Texture, p_BackTexture.StartPosition);
            m_BarFrontSprite = new Sprite("bar", pos, height, width, frontText);
            m_BarBackSprite = new Sprite("bar", pos, height, width, backText);
            
        }

        private void ini()
        {
        

        }

        private void setBars()
        {
            /*
                Vector2 c;
                if (m_Target is Ship)
                {
                    Ship t_Ship = (Ship)m_Target;
                    if (t_Ship.MaxShield > 0)
                    {
                        = this.Center.Y + offset.Y;
                        shie c = shields.Center;
                        c.X = this.Center.X;
                        c.Ylds.Center = c;

                        shields.Width = (int)(width * t_Ship.Shield / t_Ship.MaxShield);
                        blackS.Center = shields.Center;
                        shields.Position = blackS.Position; 
                   
                    }
                }

                health.Width = (int)(width * m_Target.Health / m_Target.MaxHealth);        
                c = health.Center;
                c.X = this.Center.X;// -m_Target.Radius / 2;
                c.Y = this.Center.Y + offset.Y + height;// + height; ;
                health.Center = c;

                

                blackH.Center = health.Center;
                health.Position = blackH.Position; 
            */
        }


      

        
    }
}

 