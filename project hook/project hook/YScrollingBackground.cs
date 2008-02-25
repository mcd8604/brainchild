using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class YScrollingBackground : Sprite
	{
		private Sprite[] m_SpriteArray;
		private int m_Size;
		private WorldPosition m_WorldPosition;

		internal YScrollingBackground(GameTexture p_BackgroundTexture, WorldPosition p_WorldPosition, Rectangle p_ViewPortSize)
			: base(
#if !FINAL
				"scrollingBackground",
#endif
new Vector2(0, 0), 0, 0, null, 255, true, 0, Depth.BackGroundLayer.Background)
		{
			m_WorldPosition = p_WorldPosition;
			m_Size = (int)(Math.Ceiling((float)p_ViewPortSize.Height / (float)p_BackgroundTexture.Height) + 1);
			m_SpriteArray = new Sprite[m_Size];
			for (int i = 0; i < m_Size; ++i)
			{
				Sprite s = new Sprite(
#if !FINAL
					"background",
#endif
new Vector2(0.0f, (i - 1) * p_BackgroundTexture.Height), p_BackgroundTexture.Height, p_ViewPortSize.Width, p_BackgroundTexture, 1f, true, 0, Depth.BackGroundLayer.Background);
				m_SpriteArray[i] = s;
			}
		}

		internal override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch p_SpriteBatch)
		{
			for (int i = 0; i < m_Size; ++i)
			{
				m_SpriteArray[i].Draw(p_SpriteBatch);
			}
		}

		internal override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			Vector2 newC = m_SpriteArray[0].Center;
			newC.Y += m_WorldPosition.BackgroundSpeed * (float)p_Time.ElapsedGameTime.TotalSeconds;
			if (newC.Y > (m_SpriteArray[0].Height / 2f))
			{
				newC.Y -= m_SpriteArray[0].Height;
			}
			m_SpriteArray[0].Center = newC;

			for (int i = 1; i < m_Size; ++i)
			{
				m_SpriteArray[i].Center = new Vector2(m_SpriteArray[i].Center.X, m_SpriteArray[i - 1].Center.Y + m_SpriteArray[i - 1].Height);
			}
		}
	}
}
