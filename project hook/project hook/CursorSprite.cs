using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class CursorSprite : Sprite
	{
		public CursorSprite(
#if !FINAL
			String p_Name,
#endif
			Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_Z)
			: base(
#if !FINAL
			p_Name,
#endif
			p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
		{}
		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);
			if (InputHandler.HasMouseMoved())
			{
				Center = InputHandler.MousePosition;
			}
		}
	}
}
