using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	public class HighScore
	{
		public const int size = 10;
		private int[] List = new int[size];
		public int[] Scores
		{
			get
			{
				return List;
			}
		}

		public HighScore()
		{
			List[0] = 50000;
			List[1] = 40000;
			List[2] = 30000;
			List[3] = 20000;
			List[4] = 10000;
			List[5] = 0;

			for (int i = 6; i < size; i++)
			{
				List[i] = -1;
			}

		}

		public void addScore( int Score ) {

			int toAdd = Score;
			int temp;

			for (int i = 0; i < size; i++)
			{
				if (toAdd > List[i])
				{
					temp = List[i];
					List[i] = toAdd;
					toAdd = temp;
				}
			}

		}

	}
}
