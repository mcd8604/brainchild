using System;
using System.Collections.Generic;
using System.Text;

namespace project_hook
{
	public class Events
	{
		public enum ValueKeys
		{
			Sprite,			//sprite to ass to screen
			Speed,			//speed that the level should scroll at
			FileName		//the file name of the next xml file to read
		}

		protected Dictionary<ValueKeys, Object> m_Values;
		public Dictionary<ValueKeys, Object> Values
		{
			get
			{
				return m_Values;
			}
			set
			{
				m_Values = value;
			}
		}

		public Events(Dictionary<ValueKeys, Object> p_Values)
		{
			m_Values = p_Values;
		}
	}
}
