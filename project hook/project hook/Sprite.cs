using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{
	class Sprite
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

		private Vector2 m_StartPosition;
		public Vector2 StartPosition
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

		private Vector2 m_Position;
		public Vector2 Position
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}		

		private int m_Height;
		public int Height
		{
			get
			{
				return m_Height;
			}
			set
			{
				m_Height = value;
			}
		}

		private int m_Width;
		public int Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				m_Width = value;
			}
		}

		private GameTexture m_Texture;
		public GameTexture Texture
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

		private float m_Alpha;
		public float Alpha
		{
			get
			{
				return m_Alpha;
			}
			set
			{
				m_Alpha = value;
			}
		}

		private bool m_Visible;
		public bool Visible
		{
			get
			{
				return m_Visible;
			}
			set
			{
				m_Visible = value;
			}
		}

		//0=12 Oclock
		private float m_Degree;
		public float Degree
		{
			get
			{
				return m_Degree;
			}
			set
			{
				m_Degree = value;
			}
		}

		public Rectangle Destination
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
			}
		}



		public Sprite(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree)
		{
			Name = p_Name;
			StartPosition = p_Position;
			Position = StartPosition;
			Height = p_Height;
			Width = p_Width;
			Texture = p_Texture;
			Alpha = p_Alpha;
			Visible = p_Visible;
			Degree = p_Degree;
		}



		public virtual void Draw(SpriteBatch p_SpriteBatch)
		{
			p_SpriteBatch.Draw(m_Texture.Texture, Destination, m_Texture.StartPosition, Color.White);
		}

		public virtual void Update(float p_Elapsed)
		{
		}		
	}
}
