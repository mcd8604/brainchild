using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Wintellect.PowerCollections;

namespace project_hook
{
	public class VisualEffect
	{

		OrderedDictionary<String, GameTexture> frames;

		Sprite m_BaseSprite;
		String m_Name;

		float m_FrameLength = 0.2f;
		float m_Timer = 0f;
		int m_CurrentFrame = 0;
		bool m_UpdateAnimation = true;

		//Number of animation cycles the visualeffect exists
		int m_Cycles;
		int m_CycleCount;
		bool m_CycleRemoval;//remove the sprite after cycles are up

		public int FramesPerSecond
		{
			get { return (int)(1f / m_FrameLength); }
			set { m_FrameLength = 1f / (float)value; }
		}

		public GameTexture CurrentFrame
		{
			get { return frames[m_CurrentFrame.ToString()]; }
		}

		public VisualEffect(String p_Name, Sprite p_Base, int p_FramesPerSecond)
		{
			m_BaseSprite = p_Base;
			m_Name = p_Name;
			frames = TextureLibrary.getSpriteSheet(p_Name);
			m_BaseSprite.Texture = frames[m_CurrentFrame.ToString()];
			FramesPerSecond = p_FramesPerSecond;
			m_CycleRemoval = false;
		}

		public VisualEffect(String p_Name, Sprite p_Base, int p_FramesPerSecond, int p_Cycles)
		{
			m_BaseSprite = p_Base;
			m_Name = p_Name;
			frames = TextureLibrary.getSpriteSheet(p_Name);
			m_BaseSprite.Texture = frames[m_CurrentFrame.ToString()];
			FramesPerSecond = p_FramesPerSecond;
			m_Cycles = p_Cycles;
			m_CycleCount = 0;
			m_CycleRemoval = true;
		}

		public void Update(GameTime gameTime)
		{
			if (m_UpdateAnimation)
			{
				//check if cycles up
				if (m_CycleRemoval && m_CycleCount >= m_Cycles)
				{
					//lazy sprite removal
					m_BaseSprite.Enabled = false;
				}

				m_Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (m_Timer >= m_FrameLength)
				{
					m_Timer = 0f;
					m_CurrentFrame = (m_CurrentFrame + 1) % frames.Count;
					m_BaseSprite.Texture = frames[m_CurrentFrame.ToString()];

					//increment cycles if needed
					if (m_CurrentFrame == 0)
					{
						m_CycleCount++;
					}
				}
			}
		}

		public void Reset()
		{
			m_CurrentFrame = 0;
			m_Timer = 0f;
		}

		public void StartAnimation()
		{
			m_UpdateAnimation = true;
		}

		public void StopAnimation()
		{
			m_UpdateAnimation = false;
		}





	}
}

