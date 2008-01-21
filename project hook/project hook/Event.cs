using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Event
	{
		int m_Distance;
		public int Distance
		{
			get
			{
				return m_Distance;
			}
			set
			{
				m_Distance = value;
			}
		}
	}
}
