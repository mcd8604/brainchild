using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class YScrollingBackground : Sprite
	{
		private ArrayList scrollingSprites;
		private WorldPosition m_WorldPosition;

		public YScrollingBackground(GameTexture p_BackgroundTexture, WorldPosition p_WorldPosition)
			: base("scrollingBackground", new Vector2(0, 0), 0, 0, null, 255, true, 0, Depth.BackGroundLayer.Background)
		{
			m_WorldPosition = p_WorldPosition;
			scrollingSprites = new ArrayList((World.m_ViewPortSize.Height / p_BackgroundTexture.Height) + 2);
			for (int i = 0; i < scrollingSprites.Capacity; i++)
			{
				Sprite s = new Sprite("back", new Vector2(0.0f, i * p_BackgroundTexture.Height), p_BackgroundTexture.Height, World.m_ViewPortSize.Width, p_BackgroundTexture, 255f, true, 0, Depth.BackGroundLayer.Background);
				scrollingSprites.Add(s);
				attachSpritePart(s);
			}
		}

		public override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			foreach (Sprite s in scrollingSprites)
			{
				float dist = m_WorldPosition.BackgroundSpeed * (float)p_Time.ElapsedGameTime.TotalSeconds;
				float newY = s.Position.Y + (dist);
				if (s.Position.Y >= World.m_ViewPortSize.Height)
				{
					newY = 0 - s.Height + (dist);
				}
				s.Position = new Vector2(s.Position.X, newY);
			}
		}
	}
}
