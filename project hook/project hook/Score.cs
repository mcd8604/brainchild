using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	class Score
	{
		int m_ScoreTotal;

		public int ScoreTotal
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

		public Score(int p_Score)
		{
			m_ScoreTotal = p_Score;
		}

		public int RegisterHit()
		{
			m_ScoreTotal += 10;
			return m_ScoreTotal;
		}
	}
}
