using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class CursorSprite : Sprite
	{

		public CursorSprite(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_Z)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
		{

		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			if (InputHandler.HasMouseMoved())
			{
				this.Center = InputHandler.MousePostion;
			}
		}
	}
}
