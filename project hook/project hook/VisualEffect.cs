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

		GameTexture[] m_framesArray;

		Sprite m_BaseSprite;
		String m_Name;
		public String Name
		{
			get
			{
				return m_Name;
			}
		}

		float m_FrameLength = 0.2f;
		float m_Timer = 0f;
		int m_CurrentFrame = 0;
		bool m_UpdateAnimation = true;

		//Number of animation cycles the visualeffect exists
		private int m_Cycles;
		private int m_CycleCount = 0;
		private bool m_CycleRemoval = false;//remove the sprite after cycles are up
		public bool CycleRemoval
		{
			get
			{
				return m_CycleRemoval;
			}
			set
			{
				m_CycleRemoval = value;
			}
		}

		public int FramesPerSecond
		{
			get { return (int)(1f / m_FrameLength); }
			set { m_FrameLength = 1f / (float)value; }
		}

		private int m_FrameCount;
		public int FrameCount
		{
			get { return m_FrameCount; }
		}

		public int CurrentFrame
		{
			set { m_CurrentFrame = value; }
		}

		public VisualEffect(VisualEffect p_ToCopy, Sprite p_NewBaseSprite)
		{

			m_Name = p_ToCopy.m_Name;
			m_BaseSprite = p_NewBaseSprite;
			m_FrameLength = p_ToCopy.m_FrameLength;

			buildArray(TextureLibrary.getSpriteSheet(m_Name));

			if (p_ToCopy.m_CycleRemoval)
			{
				m_Cycles = p_ToCopy.m_Cycles;
				m_CycleRemoval = true;
			}
		}


		public VisualEffect(String p_Name, Sprite p_Base, int p_FramesPerSecond)
		{
			m_BaseSprite = p_Base;
			m_Name = p_Name;

			buildArray(TextureLibrary.getSpriteSheet(m_Name));

			m_BaseSprite.Texture = m_framesArray[0];
			FramesPerSecond = p_FramesPerSecond;
		}

		public VisualEffect(String p_Name, Sprite p_Base, int p_FramesPerSecond, int p_Cycles)
		{
			m_BaseSprite = p_Base;
			m_Name = p_Name;

			buildArray(TextureLibrary.getSpriteSheet(m_Name));

			m_BaseSprite.Texture = m_framesArray[0];
			FramesPerSecond = p_FramesPerSecond;
			m_Cycles = p_Cycles;
			m_CycleRemoval = true;
		}

		private void buildArray(OrderedDictionary<string, GameTexture> dic)
		{
			m_FrameCount = dic.Count;
			m_framesArray = new GameTexture[m_FrameCount];
			for (int i = 0; i < m_FrameCount; i++)
			{
				m_framesArray[i] = dic[i.ToString()];
			}
		}

		public void Update(GameTime gameTime)
		{
			if (m_UpdateAnimation)
			{

				m_Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (m_Timer >= m_FrameLength)
				{
					m_Timer = 0f;
					++m_CurrentFrame;

					if (m_CurrentFrame >= m_FrameCount)
					{
						m_CurrentFrame = 0;
						if (m_CycleRemoval)
						{
							++m_CycleCount;
							//check if cycles up
							if (m_CycleCount >= m_Cycles)
							{
								//lazy sprite removal
								this.StopAnimation();
							}
						}
					}

					m_BaseSprite.Texture = m_framesArray[m_CurrentFrame];

				}
			}
		}

		public void Reset()
		{
			m_CurrentFrame = 0;
			m_Timer = 0f;
			m_CycleCount = 0;
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

