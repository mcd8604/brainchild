using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class Event
	{

		public enum Types
		{
			CreateSprite,
			ChangeFile,
			ChangeSpeed,
			LoadBMP
		}

		protected Types m_Type;
		public Types Type
		{
			get
			{
				return m_Type;
			}
		}
		public void setType(string type)
		{
			m_Type = (Types)Enum.Parse(typeof(Types), type, true);
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
			m_Type = Types.CreateSprite;
			m_Sprite = p_Sprite;
		}
		public Event(String p_FileName, Types p_Type)
		{
			m_Type = p_Type;
			m_FileName = p_FileName;
		}
		public Event(String p_FileName, String p_Type)
		{
			setType(p_Type);
			m_FileName = p_FileName;
		}
		public Event(int p_Speed)
		{
			m_Type = Types.ChangeSpeed;
			m_Speed = p_Speed;
		}
	}
}
