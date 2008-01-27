using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{
	/// <summary>
	/// This class holds a reference to a 2D Texture
	/// and what area of that texture should be displayed. 
	/// </summary>
	public class GameTexture
	{
		#region Variables and Properties
		//The Identifying name for the texture
		//This should be unique
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

		//The tage associated with the sprite for the Texture
		private String m_Tag;
		public String Tag
		{
			get
			{
				return m_Tag;
			}
			set
			{
				m_Tag = null;
			}
		}

		//Name of the Texture asset
		//private string m_TextureName;
		//public string TextureName
		//{
		//    get
		//    {
		//        return m_TextureName;
		//    }
		//}

		//The 2D Texture the texture will read from
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

		//The area of the Texture that will be used
		//A rectangle was chosen because it will always be used by the sprite bacth
		//when being drawn.  This will reduce the amount of objects that need to be created.
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
		//The height of the texture capture rectangle
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
		//The width of the texture capture area
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
		//The x position of the texture capture rectangle
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

		//The Y position of the texture capture rectangle
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

		private Vector2 m_Center;
		public Vector2 Center
		{
			get
			{
				return m_Center;
			}
		}

		#endregion // End of variables and Properties Region

		//This initializes the Game texture.
		public GameTexture(String p_Name, String p_Tag, Texture2D p_Texture, Rectangle p_StartPosition)
		{
			Name = p_Name;
			Tag = p_Tag;
			Texture = p_Texture;

			StartPosition = p_StartPosition;
			m_Center = new Vector2(Width * 0.5f, Height * 0.5f);
		}
	}
}