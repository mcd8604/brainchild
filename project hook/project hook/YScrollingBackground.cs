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
			: base(
#if !FINAL
			"scrollingBackground",
#endif
			new Vector2(0, 0), 0, 0, null, 255, true, 0, Depth.BackGroundLayer.Background)
		{
			m_WorldPosition = p_WorldPosition;
			scrollingSprites = new ArrayList((World.m_ViewPortSize.Height / p_BackgroundTexture.Height) + 2);
			for (int i = 0; i < scrollingSprites.Capacity; i++)
			{
				Sprite s = new Sprite(
#if !FINAL
					"back",
#endif
					new Vector2(0.0f, i * p_BackgroundTexture.Height), p_BackgroundTexture.Height, World.m_ViewPortSize.Width, p_BackgroundTexture, 255f, true, 0, Depth.BackGroundLayer.Background);
				scrollingSprites.Add(s);
				attachSpritePart(s);
			}
		}

		internal override void Update(Microsoft.Xna.Framework.GameTime p_Time)
		{
			base.Update(p_Time);
			foreach (Sprite s in scrollingSprites)
			{
				float dist = m_WorldPosition.BackgroundSpeed * (float)p_Time.ElapsedGameTime.TotalSeconds;
				float newY = s.Center.Y + (dist);
				if (s.Position.Y >= World.m_ViewPortSize.Height)
				{
					newY = ((Sprite)scrollingSprites[(scrollingSprites.IndexOf(s) + 1) % scrollingSprites.Count]).Center.Y - s.Height + (dist);
				}
				s.Center = new Vector2(s.Center.X, newY);
			}
#if !FINAL
			DebugCheck();
#endif
		}

#if !FINAL
		private void DebugCheck()
		{

			foreach (Sprite s in scrollingSprites)
			{
				float thisY = s.Center.Y;
				float nextY = ((Sprite)scrollingSprites[(scrollingSprites.IndexOf(s) + 1) % scrollingSprites.Count]).Center.Y;
				float diff = nextY - thisY;
				if (diff > s.Height + 0.0025)
				{
					Game.Out.WriteLine("Excessive gap in background tiles: " + (diff - s.Height));
				}
			}

		}
#endif

	}
}
