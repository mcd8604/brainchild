using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class YScrollingBackground : Sprite
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

		private ArrayList scrollingSprites;

		public YScrollingBackground(GameTexture p_BackgroundTexture)
			: base("scrollingBackground", new Vector2(0, 0), 0, 0, null, 255, true, 0, Depth.BackGround.Bottom)
		{
			scrollingSprites = new ArrayList((World.m_ViewPortSize.Height / p_BackgroundTexture.Height) + 2);
			for (int i = 0; i < scrollingSprites.Capacity; i++)
			{
				Sprite s = new Sprite("back", new Vector2(0.0f, i * p_BackgroundTexture.Height), p_BackgroundTexture.Height, World.m_ViewPortSize.Width, p_BackgroundTexture, 255f, true, 0, Depth.BackGround.Bottom);
				scrollingSprites.Add(s);
				attachSpritePart(s);
			}
			m_ScrollSpeed = 1;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			foreach (Sprite s in scrollingSprites)
			{
				float newY = s.Position.Y + (m_ScrollSpeed);
				if (s.Position.Y >= World.m_ViewPortSize.Height)
				{
					newY = 0 - s.Height + (m_ScrollSpeed);
				}
				s.Position = new Vector2(s.Position.X, newY);
			}
		}
	}
}
