using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
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

		private Sprite background;
		private ArrayList menuSprites;

		public Menu(ArrayList menuItems)
		{
			m_selectedIndex = -1;

			menuSprites = new ArrayList();
			for(int i = 0; i < menuItems.Capacity; i++)
			{
				//Sprite s = new Sprite("blah");
				//menuSprites.Add(s);
			}
		}

		public void draw(SpriteBatch spriteBatch)
		{
			//draw background
			background.Draw(spriteBatch);

			//draw menu items
			for (int i = 0; i < menuSprites.Capacity; i++)
			{
				//Sprite s = (Sprite)menuSprites[i];
				//s.Draw(spriteBatch);
			}
		}
	
	}
}
