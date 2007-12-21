using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	public class PathStrategy
	{
		protected Dictionary<String, Object> m_Values;
		public PathStrategy(Dictionary<String, Object> p_Values)
		{
			m_Values = p_Values;
		}

		public virtual void CalculateMovement(){

		}
	}
}
