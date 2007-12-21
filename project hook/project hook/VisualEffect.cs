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
		
        OrderedDictionary<String,GameTexture> frames;

		Sprite m_BaseSprite;
		String m_Name;

        float m_FrameLength = 1f / 5f;
        float m_Timer = 0f;
        int m_CurrentFrame = 0;
        bool m_UpdateAnimation = true;


        public int FramesPerSecond
        {
            get { return (int)(1f / m_FrameLength); }
            set { m_FrameLength = 1f / (float)value; }
        }

        public GameTexture CurrentFrame
        {
            get { return frames[m_CurrentFrame.ToString()]; }
        }

        public VisualEffect(String p_Name,Sprite p_Base,int p_FramesPerSecond)
        {
			m_BaseSprite = p_Base;
			m_Name = p_Name;
            frames = TextureLibrary.getSpriteSheet(p_Name);
			m_BaseSprite.Texture = frames[m_CurrentFrame.ToString()];
			FramesPerSecond = p_FramesPerSecond;
        }

        public void Update(GameTime gameTime)
        {
            if (m_UpdateAnimation)
            {


                m_Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (m_Timer >= m_FrameLength)
                {
                    m_Timer = 0f;
                    m_CurrentFrame = (m_CurrentFrame + 1) % frames.Count;
					m_BaseSprite.Texture = frames[m_CurrentFrame.ToString()];
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

