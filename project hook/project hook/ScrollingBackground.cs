using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class ScrollingBackground : Sprite
	{
		private float m_ScrollSpeed;
		public float ScrollSpeed
		{
			get
			{
				return m_ScrollSpeed;
			}
			set
			{
				m_ScrollSpeed = value;
			}
		}

		public ScrollingBackground(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible,
						float p_Degree, float p_Z)
			 : base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
		{
			m_ScrollSpeed = 0;
		}

		public ScrollingBackground(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible,
						float p_Degree, float p_Z, float p_ScrollSpeed)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
		{
			m_ScrollSpeed = p_ScrollSpeed;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			float newY = Position.Y + (m_ScrollSpeed);
			Position = new Vector2(Position.X, newY);
		}
	}
}
