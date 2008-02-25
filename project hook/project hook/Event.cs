using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class Event
	{
		internal enum Types
		{
			CreateSprite,
			CreateSprites,
			ChangeFile,
			ChangeSpeed,
			LoadBMP,
			PleaseLoadBMP,
			EndGame
		}

		protected Types m_Type;
		internal Types Type
		{
			get
			{
				return m_Type;
			}
		}
		internal void setType(string type)
		{
			m_Type = (Types)Enum.Parse(typeof(Types), type, true);
		}

		protected Sprite m_Sprite;
		internal Sprite Sprite
		{
			get
			{
				return m_Sprite;
			}
		}

		protected List<Sprite> m_Sprites;
		internal List<Sprite> Sprites
		{
			get
			{
				return m_Sprites;
			}
		}

		protected int m_Speed;
		internal int Speed
		{
			get
			{
				return m_Speed;
			}
		}

		protected String m_FileName;
		internal String FileName
		{
			get
			{
				return m_FileName;
			}
		}


		internal Event(Sprite p_Sprite)
		{
			m_Type = Types.CreateSprite;
			m_Sprite = p_Sprite;
		}
		internal Event(List<Sprite> p_Sprites)
		{
			m_Type = Types.CreateSprites;
			m_Sprites = p_Sprites;
		}
		internal Event(String p_FileName, Types p_Type)
		{
			m_Type = p_Type;
			m_FileName = p_FileName;
		}
		internal Event(String p_FileName, String p_Type)
		{
			setType(p_Type);
			m_FileName = p_FileName;
		}
		internal Event(int p_Speed)
		{
			m_Type = Types.ChangeSpeed;
			m_Speed = p_Speed;
		}
		internal Event(Types p_Type)
		{
			m_Type = p_Type;
		}
	}
}
