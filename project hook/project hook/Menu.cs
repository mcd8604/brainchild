using System;
using System.Collections;
using System.Text;

namespace project_hook
{
	class Menu
	{

		private int selectedIndex;
		private Sprite background;
		private ArrayList mMenuSprites;

		public Menu(ArrayList menuItems)
		{
			selectedIndex = -1;

			//(test stuff)
			Vector2 pos = new Vector2();
			int height = 100;
			int width = 100;			
			
			background = new Sprite("menuBackground", pos, height, width, texture);

			mMenuSprites = new ArrayList();
			for(int i = 0; i < menuItems.Capacity; i++)
			{
				Sprite s = new Sprite("blah");
				mMenuSprites.Add(s);
			}
		}

		public void draw(SpriteBatch spriteBatch)
		{
			//draw background
			background.Draw(spriteBatch);

			//draw menu items
			for (int i = 0; i < mMenuSprites.Capacity; i++)
			{
				Sprite s = (Sprite)mMenuSprites[i];
				s.Draw(spriteBatch);
			}
		}
	
	}
}
