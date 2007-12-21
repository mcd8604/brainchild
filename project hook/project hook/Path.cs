using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	public class Path
	{
		string m_Strategy;

		public Path(string p_Strategy, Dictionary<String,Object> p_Values)
		{
			m_Strategy = p_Strategy;

		}

		public void CalculateMovement(){


		}

		enum Paths
		{
			Follow,
			Line

		}
		

		public static PathStrategy CreateStrategy(String strategy)
		{


			return null;
		}
	}
}
