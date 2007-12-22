using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	public class Path
	{
		Paths m_Strategy;
		PathStrategy m_Path;

		public Path(Paths p_Strategy, Dictionary<String,Object> p_Values)
		{
			m_Strategy = p_Strategy;

			if (m_Strategy == Paths.Follow)
			{
				m_Path = new FollowPath(p_Values);
			}
		}

		public void CalculateMovement(){

			m_Path.CalculateMovement();
		}

		public enum Paths
		{
			Follow,
			Line

		}

	}
}
