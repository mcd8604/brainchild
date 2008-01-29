using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Event
	{
		private String m_Type;
		public String Type
		{
			get
			{
				return m_Type;
			}
		}

		private Collidable m_Collidable;
		public Collidable Collidable
		{
			get
			{
				return m_Collidable;
			}
		}

		private int m_Speed;
		public int Speed
		{
			get
			{
				return m_Speed;
			}
		}

		private String m_FileName;
		public String FileName
		{
			get
			{
				return m_FileName;
			}
		}

		public Event(Collidable p_Collidable)
		{
			m_Type = "Collidable";

			m_Collidable = p_Collidable;
		}
		public Event(String p_FileName, String p_Type)
		{
			m_Type = p_Type;

			m_FileName = p_FileName;
		}
		public Event(int p_Speed)
		{
			m_Type = "ChangeSpeed";

			m_Speed = p_Speed;
		}
	}
}
