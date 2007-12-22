using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Score
	{
		ulong m_ScoreTotal;
		int m_Multiplyer = 0;
		double m_ComboTime = 3;
		double m_LastFiredTime = 0;
		
		public ulong ScoreTotal
		{
			get
			{
				return m_ScoreTotal;
			}
		}

		public Score()
		{
			m_ScoreTotal = 0;
		}

		public Score(ulong p_Score)
		{
			m_ScoreTotal = p_Score;
		}

		public ulong RegisterHit(GameTime p_GameTime)
		{
			double timeDiff = p_GameTime.TotalGameTime.TotalSeconds - m_LastFiredTime;
			
			if (timeDiff >= m_ComboTime)
			{
				m_Multiplyer = 1;
			}
			else
			{
				m_Multiplyer++;
			}

			m_ScoreTotal += 10ul * (ulong)m_Multiplyer;
			m_LastFiredTime = p_GameTime.TotalGameTime.TotalSeconds;

			return m_ScoreTotal;
		}
	}
}
