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

		public Menu(ArrayList menuItemNames, String backgroundName)
		{
			m_selectedIndex = -1;
			m_MenuItemNames = menuItemNames;
			m_BackgroundName = backgroundName;			
		}

		public void Update()
		{
			GameTexture bgTexture = TextureLibrary.getGameTexture(m_BackgroundName, "");
			m_BackgroundSprite = new Sprite(m_BackgroundName, new Vector2(200.0f, 200.0f), bgTexture.Height, bgTexture.Width, bgTexture, 100.0f, true, Depth.BackGround.Bottom, 0);

			m_MenuItemSprites = new ArrayList();
			for (int i = 0; i < m_MenuItemNames.Capacity; i++)
			{
				GameTexture curTexture = TextureLibrary.getGameTexture((String)m_MenuItemNames[i], "");
				m_MenuItemSprites[i] = new Sprite((String)m_MenuItemNames[i], new Vector2(200.0f, 200.0f + (i * curTexture.Height)), curTexture.Height, curTexture.Width, curTexture, 100.0f, true, Depth.MidGround.Bottom, 0);
			}
		}

		public void Draw(SpriteBatch m_SpriteBatch)
		{
			m_BackgroundSprite.Draw(m_SpriteBatch);
			for (int i = 0; i < m_MenuItemSprites.Capacity; i++)
			{
				Sprite curSprite = (Sprite)m_MenuItemSprites[i];
				curSprite.Draw(m_SpriteBatch);
			}
		}

		public void testMouseClick(float mouseX, float mouseY)
		{
			//test if a menu item was clicked
		}

		public void testMouseMove(float mouseX, float mouseY)
		{
			//test if the mouse is over a menu item
		}
	
	}
}
