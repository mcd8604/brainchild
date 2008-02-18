using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
    class WeaponUpgradeBar:Sprite
    {
       
        //the ship who's shield and health will be displayed
        PlayerShip m_Target;

        //The poistion the shields and heatlh will be drawn
		Sprite weapons;
        Sprite blackS;

     
        int width;
        int height;
      //  Vector2 offset;
      //  bool attach = false;

     

        public WeaponUpgradeBar(PlayerShip p_Ship, Vector2 pos, int p_Width, int p_Height)
        {
            m_Target = p_Ship;
            width = p_Width;
            height = p_Height;
            ini(pos);
			setBars();
        }

        private void ini(Vector2 pos)
        {
            weapons = new Sprite(
#if !FINAL
				"WeaponBar",
#endif
				pos, height, width, TextureLibrary.getGameTexture("WeaponBar"), 200, true, 0.0f, Depth.HUDLayer.Foreground);

            blackS = new Sprite();
            blackS.Texture = TextureLibrary.getGameTexture("black");
            blackS.Z = Depth.HUDLayer.Midground;
            blackS.Enabled = true;
            blackS.Width = width;
            blackS.Height = height;
            blackS.Alpha = 255;
			blackS.Position = pos;
        }

		public override Vector2 Position
		{
			get
			{
				return blackS.Position;
			}
			set
			{
				blackS.Position = value;
			}
		}

		public override Vector2 Center
		{
			get
			{
				return blackS.Center;
			}
			set
			{
				blackS.Center = value;
			}
		}

        private void setBars()
        {

			if (m_Target.CurrentLevel == PlayerShip.MAX_LEVEL)
			{
				weapons.Width = width;
				weapons.Position = blackS.Position;
			}
			else
			{

				int val = m_Target.UpgradeLevel;
				int levelReq = m_Target.LevelRequirement(m_Target.CurrentLevel + 1);
				int prevlevel = m_Target.LevelRequirement(m_Target.CurrentLevel);
				float div =  (float)(val - prevlevel)/(float)(levelReq - prevlevel) ;
				int w = (int)(width * div);
				if (w > 0)
				{

				}
				weapons.Width = w;
				//c = weapons.Center;
				//c.X = this.Center.X;// -m_Target.Radius / 2;
				//c.Y = this.Center.Y + offset.Y + height;// + height; ;
				//weapons.Center = c;

				//blackS.Center = weapons.Center;
				weapons.Position = blackS.Position;
			}
		}


		internal override void Update(GameTime p_Time)
        {
            base.Update(p_Time);
            setBars();


        }

		internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
        {
			if (weapons != null)
			{
				weapons.Draw(p_SpriteBatch);
				blackS.Draw(p_SpriteBatch);
			}
        }
    }
}
