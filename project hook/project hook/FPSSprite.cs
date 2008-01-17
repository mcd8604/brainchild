using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class FPSSprite : TextSprite
	{

		protected FPS m_fps = new FPS();
		protected static String m_Prefix = "FPS: ";

		public FPSSprite(Vector2 p_Center)
			: base("", p_Center)
		{ }

		public FPSSprite(Vector2 p_Center, Color p_Color)
			: base("", p_Center, p_Color)
		{ }

		public FPSSprite(Vector2 p_Center, Color p_Color, float p_Z)
			: base("", p_Center, p_Color, p_Z)
		{ }

		public FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha)
			: base("", p_Center, p_Color, p_Z, p_Alpha)
		{ }

		public FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha, float p_Rotation)
			: base("", p_Center, p_Color, p_Z, p_Alpha, p_Rotation)
		{ }

		public FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha, float p_Rotation, Vector2 p_Scale)
			: base("", p_Center, p_Color, p_Z, p_Alpha, p_Rotation, p_Scale)
		{ }

		public FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha, float p_Rotation, int p_Height, int p_Width)
			: base("", p_Center, p_Color, p_Z, p_Alpha, p_Rotation, p_Height, p_Width)
		{ }

		public override void Update(GameTime p_Time)
		{
			m_fps.Update(p_Time);
			base.Text = m_Prefix + m_fps.ToString();
		}

		public override void Draw(SpriteBatch p_SpriteBatch)
		{
			m_fps.Draw(p_SpriteBatch);
			base.Draw(p_SpriteBatch);
		}

	}
}
