using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Event
	{
		protected String m_Type;
		public String Type
		{
			get
			{
				return m_Type;
			}
		}

		protected Sprite m_Sprite;
		public Sprite Sprite
		{
			get
			{
				return m_Sprite;
			}
		}

		protected int m_Speed;
		public int Speed
		{
			get
			{
				return m_Speed;
			}
		}

		protected String m_FileName;
		public String FileName
		{
			get
			{
				return m_FileName;
			}
		}

		public Event(Sprite p_Sprite)
		{
			m_Type = "Sprite";

			m_Sprite = p_Sprite;
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
