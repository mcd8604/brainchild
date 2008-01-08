using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class Visual : Sprite
	{
		private float m_Duration;
		public float Duration 
		{
			get
			{
				return m_Duration;
			}

			set
			{
				m_Duration = value;
			}
		}

		private double m_TimeBorn;
		public double TimeBorn
		{
			get
			{
				return m_TimeBorn;
			}

			set
			{
				m_TimeBorn = value;
			}
		}

		public Visual(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_Z, GameTime p_TimeBorn, float p_Duration)
			: base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_Z)
		{
			TimeBorn = p_TimeBorn.ElapsedGameTime.TotalMilliseconds;
			Duration = p_Duration;
		}

		public override void Update(GameTime p_Time)
		{
			base.Update(p_Time);

			if (p_Time.ElapsedGameTime.TotalMilliseconds > TimeBorn + Duration)
			{
				Visible = false;
			}

		}
	}
}
