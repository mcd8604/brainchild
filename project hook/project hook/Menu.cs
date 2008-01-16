using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
    /*
     * Description: 
     * 
     * TODO:
	 */
	class Menu : Sprite
	{
		protected int m_selectedIndex;

		protected String m_BackgroundName;
		protected Sprite m_BackgroundSprite;

		protected String m_HighlightName;
		protected Sprite m_HightlightSprite;

		protected ArrayList m_MenuItemNames;
		protected ArrayList m_MenuItemSprites;

		protected Boolean usingTextSprite;

		public Menu()
		{
			m_selectedIndex = 0;

			//default textures
			m_BackgroundName = "menu_background";
			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();
		}

		/*public Menu(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, 
							float p_Degree, float p_Z)
            : base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
		{
			m_selectedIndex = 0;

			//default textures
			m_BackgroundName = "menu_background";
			m_HighlightName = "menu_highlight";

			m_MenuItemNames = new ArrayList();
		}*/

        public virtual void Load(GraphicsDeviceManager gdm)
        {
            GameTexture bgTexture = TextureLibrary.getGameTexture(m_BackgroundName, "");
			float xPos = (gdm.GraphicsDevice.Viewport.Width - bgTexture.Width) / 2;
			float yPos = (gdm.GraphicsDevice.Viewport.Height - bgTexture.Height) / 2;
			m_BackgroundSprite = new Sprite(m_BackgroundName, new Vector2(xPos, yPos), bgTexture.Height, bgTexture.Width, bgTexture, 200f, true,
											0, Depth.ForeGround.Bottom);
			attachSpritePart(m_BackgroundSprite);

			m_MenuItemSprites = new ArrayList();			
			if (m_MenuItemNames.Count != 0)
			{
				if (usingTextSprite)
				{
					createTextSprites();
				}
				else
				{
					createImageSprites();
				}
			}
		}

		private void createTextSprites()
		{
			int textHeight = 32;
			int topBuffer = 32;

			//create each menu item from TextSprites
			for (int i = 0; i < m_MenuItemNames.Count; i++)
			{
				float xPos = m_BackgroundSprite.Center.X;
				float yPos = topBuffer + m_BackgroundSprite.Position.Y + (textHeight * i);

				TextSprite mis = new TextSprite((String)m_MenuItemNames[i], new Vector2(xPos, yPos), Color.White, Depth.ForeGround.Top);

				m_MenuItemSprites.Add(mis);
				attachSpritePart(mis);
			}

			//set highlighted
			setHighlightSprite();
			TextSprite selSprite = (TextSprite)m_MenuItemSprites[m_selectedIndex];
			selSprite.Color = Color.Yellow;
		}

		private void createImageSprites()
		{

			//create each menu sprite - image textures
			for (int i = 0; i < m_MenuItemNames.Count; i++)
			{
				GameTexture curTexture = TextureLibrary.getGameTexture((String)m_MenuItemNames[i], "");
				float xPos = m_BackgroundSprite.Position.X + (m_BackgroundSprite.Width - curTexture.Width) / 2;
				float yPos = m_BackgroundSprite.Position.Y;
				//set position based on other menu sprites
				for (int k = 0; k < m_MenuItemSprites.Count; k++)
				{
					Sprite s = (Sprite)m_MenuItemSprites[k];
					yPos += s.Height;
				}
				Sprite mis = new Sprite((String)m_MenuItemNames[i], new Vector2(xPos, yPos), curTexture.Height, curTexture.Width,
													curTexture, 255f, true, 0, Depth.ForeGround.Mid);
				m_MenuItemSprites.Add(mis);
				attachSpritePart(mis);
			}

			//create highlight sprite
			Sprite selSprite = (Sprite)m_MenuItemSprites[m_selectedIndex];
			GameTexture highlightTexture = TextureLibrary.getGameTexture(m_HighlightName, "");
			m_HightlightSprite = new Sprite(m_HighlightName, new Vector2(selSprite.Position.X, selSprite.Position.Y), selSprite.Texture.Height,
											selSprite.Texture.Width, highlightTexture, 255f, true, 0, Depth.ForeGround.Top);
			attachSpritePart(m_HightlightSprite);
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
            if (Game.m_KeyHandler.IsActionPressed(KeyHandler.Actions.Pause))
            {
                //this.Exit();               
            }

			if (Game.m_KeyHandler.IsActionPressed(KeyHandler.Actions.Up))
            {
                up();
            }

			if (Game.m_KeyHandler.IsActionPressed(KeyHandler.Actions.Down))
            {
                down();
            }

			if (Game.m_KeyHandler.IsActionPressed(KeyHandler.Actions.MenuAccept))
            {
                accept();
			}
        }

		public override void Draw(SpriteBatch p_SpriteBatch)
		{
			p_SpriteBatch.Begin();
			base.Draw(p_SpriteBatch);
			p_SpriteBatch.End();
		}

		protected void down()
        {
            if (m_selectedIndex < m_MenuItemSprites.Count - 1)
            {
                m_selectedIndex++;
            }
            else
            {
                m_selectedIndex = 0;
            }

            setHighlightSprite();
        }

		protected void up()
        {
            if(m_selectedIndex > 0)
            {
                m_selectedIndex--;    
            }
            else
            {
                m_selectedIndex = m_MenuItemSprites.Count - 1;
            }
            
            setHighlightSprite();
        }

        public void setSelectedIndex(int index)
        {
            if (index < 0)
            {
                index = 0;
            }
            else if(index > m_MenuItemSprites.Count - 1)
            {
                index = m_MenuItemSprites.Count - 1;
            }

            m_selectedIndex = index;
            setHighlightSprite();
        }

		protected virtual void setHighlightSprite()
		{
			Sprite selSprite = (Sprite)m_MenuItemSprites[m_selectedIndex];
			if (usingTextSprite)
			{
				foreach (TextSprite s in m_MenuItemSprites)
				{
					s.Color = Color.White;
				}
				selSprite.Color = Color.Yellow;
			}
			else
			{
				if (m_HightlightSprite != null)
				{
					m_HightlightSprite.Width = selSprite.Width;
					m_HightlightSprite.Position = selSprite.Position;
				}
            }
        }

        //override this method
        public virtual void accept()
        {
        }	
	}
}
