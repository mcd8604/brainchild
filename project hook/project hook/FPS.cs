using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	internal class FPSSprite : TextSprite
	{
		private float m_FPS;
		internal float Value
		{
			get
			{
				return m_FPS;
			}
		}

		private float m_UpdateInterval = 1.0f;
		internal float UpdateInterval
		{
			get
			{
				return m_UpdateInterval;
			}
			set
			{
				m_UpdateInterval = value;
			}

		}

		private float m_TimeSinceLastUpdate = 0.0f;
		private int m_Framecount = 0;

		private const String m_Prefix = "FPS: ";

		internal FPSSprite(Vector2 p_Center) : base("", p_Center) { }
		internal FPSSprite(Vector2 p_Center, Color p_Color) : base("", p_Center, p_Color) { }
		internal FPSSprite(Vector2 p_Center, Color p_Color, float p_Z) : base("", p_Center, p_Color, p_Z) { }
		internal FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha) : base("", p_Center, p_Color, p_Z, p_Alpha) { }
		internal FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha, float p_Rotation) : base("", p_Center, p_Color, p_Z, p_Alpha, p_Rotation) { }
		internal FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha, float p_Rotation, Vector2 p_Scale) : base("", p_Center, p_Color, p_Z, p_Alpha, p_Rotation, p_Scale) { }
		internal FPSSprite(Vector2 p_Center, Color p_Color, float p_Z, float p_Alpha, float p_Rotation, int p_Height, int p_Width) : base("", p_Center, p_Color, p_Z, p_Alpha, p_Rotation, p_Height, p_Width) { }

		internal override void Update(GameTime p_Time)
		{
			m_TimeSinceLastUpdate += (float)p_Time.ElapsedRealTime.TotalSeconds;
			if (m_TimeSinceLastUpdate > m_UpdateInterval)
			{
				m_FPS = m_Framecount / m_TimeSinceLastUpdate;
				Text = m_Prefix + Convert.ToInt32(m_FPS).ToString();
				m_TimeSinceLastUpdate = 0.0f;
				m_Framecount = 0;
			}
		}

		internal override void Draw(SpriteBatch p_SpriteBatch)
		{
			base.Draw(p_SpriteBatch);
			++m_Framecount;
		}
	}
}
