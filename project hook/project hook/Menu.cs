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

		private int m_highlightedIndex;
		public int HighlightedIndex
		{
			get
			{
				return m_highlightedIndex;
			}
			set
			{
				m_highlightedIndex = value;
			}
		}

		private int m_selectedIndex;
		public int SelectedIndex
		{
			get
			{
				return m_selectedIndex;
			}
			set
			{
				m_selectedIndex = value;
			}
		}

		private String m_BackgroundName;
		private Sprite m_BackgroundSprite;

		private ArrayList m_MenuItemNames;
		private ArrayList m_MenuItemSprites;

		public Menu()
		{
			m_selectedIndex = 0;

            //lazy default values
            m_BackgroundName = "menu_background";

            m_MenuItemNames = new ArrayList();
            m_MenuItemNames.Add("menu_newgame");
            m_MenuItemNames.Add("menu_exitgame");
            //lazy
		}

        public void Load()
        {
            TextureLibrary.LoadTexture(m_BackgroundName);
            for (int i = 0; i < m_MenuItemNames.Count; i++)
            {
                TextureLibrary.LoadTexture((String)m_MenuItemNames[i]);
            }
            initMenuSprites();
        }

		private void initMenuSprites()
		{
			GameTexture bgTexture = TextureLibrary.getGameTexture(m_BackgroundName, "");
			m_BackgroundSprite = new Sprite(m_BackgroundName, new Vector2(200.0f, 200.0f), bgTexture.Height, bgTexture.Width, bgTexture, 100.0f, true, Depth.BackGround.Bottom, 0);

			m_MenuItemSprites = new ArrayList();
            for (int i = 0; i < m_MenuItemNames.Count; i++)
			{
				GameTexture curTexture = TextureLibrary.getGameTexture((String)m_MenuItemNames[i], "");
                m_MenuItemSprites.Add(new Sprite((String)m_MenuItemNames[i], new Vector2(200.0f, 200.0f + (i * curTexture.Height)), curTexture.Height, curTexture.Width, curTexture, 100.0f, true, Depth.MidGround.Bottom, 0));
			}
		}

		public void Draw(SpriteBatch m_SpriteBatch)
		{
			m_BackgroundSprite.Draw(m_SpriteBatch);
            for (int i = 0; i < m_MenuItemSprites.Count; i++)
			{
				Sprite curSprite = (Sprite)m_MenuItemSprites[i];
				curSprite.Draw(m_SpriteBatch);
			}
		}
	
	}
}
