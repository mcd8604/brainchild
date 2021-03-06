using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class MenuInstructions2 : Menu
	{
		double m_Delay = 20;

		internal MenuInstructions2()
			: base()
		{
			m_BackgroundName = "Instructions2";
		}

		internal override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			m_Delay -= p_Time.ElapsedGameTime.TotalSeconds;

			if (m_Delay < 0)
			{
				Menus.setCurrentMenu(Menus.MenuScreens.Instructions3);
			}
		}
		internal override void accept()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Instructions3);
		}
		internal override void cancel()
		{
			Menus.setCurrentMenu(Menus.MenuScreens.Instructions3);
		}
	}
}