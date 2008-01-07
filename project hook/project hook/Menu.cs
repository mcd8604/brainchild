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
	class Menu
	{
        private Boolean m_visible;
        public Boolean visible
        {
            get
            {
                return m_visible;
            }
            set
            {
                if (visible)
                {
                    setSelectedIndex(0);
                }
                m_visible = value;
            }
        }

		private int m_selectedIndex;

        private String m_BackgroundName;
        private Sprite m_BackgroundSprite;

        private String m_HighlightName;
        private Sprite m_HightlightSprite;

		private ArrayList m_MenuItemNames;
		private ArrayList m_MenuItemSprites;

		public Menu()
		{
            visible = false;

            m_selectedIndex = 0;

            //lazy default values
            m_BackgroundName = "menu_background";

            m_HighlightName = "menu_highlight";

            m_MenuItemNames = new ArrayList();
            m_MenuItemNames.Add("menu_newgame");
            m_MenuItemNames.Add("menu_exitgame");
            //lazy
		}

        public void Load(GraphicsDeviceManager gdm)
        {
            TextureLibrary.LoadTexture(m_BackgroundName);
            for (int i = 0; i < m_MenuItemNames.Count; i++)
            {
                TextureLibrary.LoadTexture((String)m_MenuItemNames[i]);
            }
            TextureLibrary.LoadTexture(m_HighlightName);
            initMenuSprites();
        }

        private void initMenuSprites()
		{
            GameTexture bgTexture = TextureLibrary.getGameTexture(m_BackgroundName, "");
            float xPos = (Game1.graphics.GraphicsDevice.Viewport.Width - bgTexture.Width) / 2;
			float yPos = (Game1.graphics.GraphicsDevice.Viewport.Height - bgTexture.Height) / 2;
            m_BackgroundSprite = new Sprite(m_BackgroundName, new Vector2(xPos, yPos), bgTexture.Height, bgTexture.Width, bgTexture, 100.0f, true, 0, Depth.ForeGround.Bottom);

			m_MenuItemSprites = new ArrayList();
            
            //create each menu sprite
            for (int i = 0; i < m_MenuItemNames.Count; i++)
            {
                GameTexture curTexture = TextureLibrary.getGameTexture((String)m_MenuItemNames[i], "");
                xPos = m_BackgroundSprite.Position.X + (m_BackgroundSprite.Width - curTexture.Width) / 2;
                yPos = m_BackgroundSprite.Position.Y;
                //set position based on other menu sprites
                for (int k = 0; k < m_MenuItemSprites.Count; k++)
                {
                    Sprite mis = (Sprite)m_MenuItemSprites[k];
                    yPos += mis.Height;
                }
                
                m_MenuItemSprites.Add(new Sprite((String)m_MenuItemNames[i], new Vector2(xPos, yPos), curTexture.Height, curTexture.Width, curTexture, 100.0f, true, 0, Depth.ForeGround.Mid));
            }

            //create highlight sprite
            Sprite selSprite = (Sprite)m_MenuItemSprites[m_selectedIndex];
            GameTexture highlightTexture = TextureLibrary.getGameTexture(m_HighlightName, "");
            m_HightlightSprite = new Sprite(m_HighlightName, new Vector2(selSprite.Position.X, selSprite.Position.Y), selSprite.Texture.Height, selSprite.Texture.Width, highlightTexture, 100.0f, true, 0, Depth.ForeGround.Top);

		}

		public void Draw(SpriteBatch m_SpriteBatch)
		{
            if (m_visible)
            {
                m_BackgroundSprite.Draw(m_SpriteBatch);
                for (int i = 0; i < m_MenuItemSprites.Count; i++)
                {
                    Sprite curSprite = (Sprite)m_MenuItemSprites[i];
                    curSprite.Draw(m_SpriteBatch);
                }
                m_HightlightSprite.Draw(m_SpriteBatch);
            }
		}

        public Boolean ToggleVisibility()
        {
            return m_visible = !m_visible;
        }

        //lazy params
        //(should menu have its own KeyHandler?)
        public void checkKeys(KeyHandler keyhandler, Game1 game)
        {
            if (keyhandler.IsActionPressed(KeyHandler.Actions.Pause))
            {
                //this.Exit();
                m_visible = false;
            }

            if (keyhandler.IsActionPressed(KeyHandler.Actions.Up))
            {
                up();
            }

            if (keyhandler.IsActionPressed(KeyHandler.Actions.Down))
            {
                down();
            }

            if (keyhandler.IsActionPressed(KeyHandler.Actions.MenuAccept))
            {
                accept(game);
            }
        }

        private void up()
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

        private void down()
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

        private void setHighlightSprite() {
            if (m_HightlightSprite != null)
            {
                Sprite selSprite = (Sprite)m_MenuItemSprites[m_selectedIndex];
                m_HightlightSprite.Position = new Vector2(m_HightlightSprite.Position.X, selSprite.Position.Y);
            }
        }

        //override this method
        public void accept(Game1 game)
        {
            //lazy hard coding
            if (m_selectedIndex == 0)
            {

            }

            if (m_selectedIndex == 1)
            {
                game.Exit();
            }

        }
	
	}
}
