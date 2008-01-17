using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class TailBodySprite : Sprite
	{

		public TailBodySprite(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_Z)
            : base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
		{

		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			p_SpriteBatch.End();
			p_SpriteBatch.Begin(SpriteBlendMode.Additive);
			base.Draw(p_SpriteBatch);
			p_SpriteBatch.End();
			p_SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
		}
	}
}
