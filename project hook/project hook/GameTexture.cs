using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{
	class GameTexture
	{
		private String m_Name;
		public String Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		private Texture2D m_Texture;
		public Texture2D Texture
		{
			get
			{
				return m_Texture;
			}
			set
			{
				m_Texture = value;
			}
		}

		private Rectangle m_StartPosition;
		public Rectangle StartPosition
		{
			get
			{
				return m_StartPosition;
			}
			set
			{
				m_StartPosition = value;
			}
		}
		public int Height
		{
			get
			{
				return m_StartPosition.Height;
			}
			set
			{
				m_StartPosition.Height = value;
			}
		}
		public int Width
		{
			get
			{
				return m_StartPosition.Width;
			}
			set
			{
				m_StartPosition.Width = value;
			}
		}
		public int X
		{
			get
			{
				return m_StartPosition.X;
			}
			set
			{
				m_StartPosition.X = value;
			}
		}
		public int Y
		{
			get
			{
				return m_StartPosition.Y;
			}
			set
			{
				m_StartPosition.Y = value;
			}
		}


		public GameTexture(String p_Name, Texture2D p_Texture, Rectangle p_StartPosition)
		{
			Name = p_Name;
			Texture = p_Texture;
			StartPosition = p_StartPosition;
		}
	}
}
