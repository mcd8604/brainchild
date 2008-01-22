using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public enum Type
	{
		create,
		change,
		next
	}

	public class Event
	{
		Type m_Type;
		Sprite m_Sprite;
		int m_Speed;
		String m_FileName;

		public Event(Type p_Type, Dictionary<Events.ValueKeys, Object> p_Values)
		{
			m_Type = p_Type;

			switch (m_Type)
			{
				case Type.change:
					m_Speed = (int)p_Values[Events.ValueKeys.Speed];
					break;
				case Type.create:
					m_Sprite = (Sprite)p_Values[Events.ValueKeys.Sprite];
					break;
				case Type.next:
					m_FileName = (String)p_Values[Events.ValueKeys.FileName];
					break;
			}
		}
	}
}
