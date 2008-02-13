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
		private int m_CycleCount;
		private bool m_CycleRemoval;//remove the sprite after cycles are up
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

		public int FrameCount
		{
			get { return (int)(frames.Keys.Count); }
		}


	//	public GameTexture CurrentFrame
//		{//
	//		get { return frames[m_CurrentFrame.ToString()]; }
//		}

		public int CurrentFrame
		{
			set { m_CurrentFrame = value; }
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
					this.StopAnimation();
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

