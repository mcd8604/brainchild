using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Event
	{
		String m_Type;
		Sprite m_Sprite;
		int m_Speed;
		String m_FileName;

		public Event(Sprite p_Sprite)
		{
			m_Type = "Sprite";

			m_Sprite = p_Sprite;
		}
		public Event(String p_FileName)
		{
			m_Type = "FileChange";

			m_FileName = p_FileName;
		}
		public Event(int p_Speed)
		{
			m_Type = "ChangeSpeed";

			m_Speed = p_Speed;
		}
	}
}
