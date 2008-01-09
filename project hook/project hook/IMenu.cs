using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	interface IMenu
	{
		void Load(GraphicsDeviceManager gdm);

		void Draw(SpriteBatch p_SpriteBatch);

		Boolean ToggleVisibility();

		void checkKeys(KeyHandler keyhandler);

		void setSelectedIndex(int index);

		void accept();
	
	}
}
