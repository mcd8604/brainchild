using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class HealthBar:Sprite
    {

        //the ship who's shield and health will be displayed
        Collidable m_Target;

        //The poistion the shields and heatlh will be drawn
        Sprite shields;
        Sprite blackS;
        
        Sprite health;
        Sprite blackH;
        int width;
        int height;
        Vector2 offset;
        bool attach = false;

        public HealthBar(Collidable p_Ship) {
            m_Target = p_Ship;
            attach = true;
            TaskAttach t = new TaskAttach(m_Target);
            this.Task = t;
            
            width = m_Target.Width / 2;

            height = m_Target.Height / 25;
            if (height < 5)
            {
                    height = 5;
            }
          
            offset = new Vector2(0,m_Target.Radius / 2 );

            ini();
            setBars();

            m_Target.attachSpritePart(this);
        }

        public HealthBar(Collidable p_Ship, Vector2 pos, int p_Width, int p_Height)
        {
            m_Target = p_Ship;
            width = p_Width;
            height = p_Height;
            this.Position = pos;
              ini();
            setBars();
        }
        private void ini()
        {
            shields = new Sprite("HealthBar", new Vector2(this.Center.X, this.Center.Y),
                                      height, width, TextureLibrary.getGameTexture("shieldBar", ""), 200, true, 0.0f, Depth.HUDLayer.Foreground);
            health = new Sprite("HealthBar", new Vector2(this.Center.X, this.Center.Y - 20),
                                   height, width, TextureLibrary.getGameTexture("healthBar", ""), 200, true, 0.0f, Depth.HUDLayer.Foreground); ;

            blackS = new Sprite();
            blackS.Texture = TextureLibrary.getGameTexture("black", "");
            blackS.Z = Depth.HUDLayer.Midground;
            blackS.Enabled = true;
            blackS.Width = width;
            blackS.Height = height;
            blackS.Alpha = 255;

            blackH = new Sprite();
            blackH.Texture = TextureLibrary.getGameTexture("black", "");
            blackH.Z = Depth.HUDLayer.Midground;
            blackH.Enabled = true;
            blackH.Width = width;
            blackH.Height = height;
            blackH.Alpha = 255;

        }

        private void setBars()
        {
                Vector2 c;
                if (m_Target is Ship)
                {
                    Ship t_Ship = (Ship)m_Target;
                    if (t_Ship.MaxShield > 0)
                    {
                         c = shields.Center;
                        c.X = this.Center.X;
                        c.Y = this.Center.Y + offset.Y;
                        shields.Center = c;


                        shields.Width = (int)(width * t_Ship.Shield / t_Ship.MaxShield);
                        blackS.Center = shields.Center;
                   
                    }
                }

                health.Width = (int)(width * m_Target.Health / m_Target.MaxHealth);        
                c = health.Center;
                c.X = this.Center.X;// -m_Target.Radius / 2;
                c.Y = this.Center.Y + offset.Y + height;// + height; ;
                health.Center = c;

                

                blackH.Center = health.Center;
                
            
        }


        public override void Update(GameTime p_Time)
        {
            base.Update(p_Time);
            setBars();


        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
        {
            if (health != null)
            {
                health.Draw(p_SpriteBatch);
                blackH.Draw(p_SpriteBatch);
            }

            if (shields != null)
            {
                shields.Draw(p_SpriteBatch);
                blackS.Draw(p_SpriteBatch);
            }
        }
    }
}
