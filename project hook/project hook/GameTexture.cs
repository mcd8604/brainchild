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
	internal class GameTexture
	{
		#region Variables and Properties
		//The Identifying name for the texture
		//This should be unique
		protected String m_Name;
		internal String Name
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

		//The tag associated with the sprite for the Texture
		protected int m_Tag;
		internal int Tag
		{
			get
			{
				return m_Tag;
			}
			set
			{
				m_Tag = value;
			}
		}

		//Name of the Texture asset
		//protected string m_TextureName;
		//internal string TextureName
		//{
		//    get
		//    {
		//        return m_TextureName;
		//    }
		//}

		//The 2D Texture the texture will read from
		protected Texture2D m_Texture;
		internal Texture2D Texture
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
		protected Rectangle m_StartPosition;
		internal Rectangle StartPosition
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
		internal int Height
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
		internal int Width
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
		internal int X
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
		internal int Y
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

		protected Vector2 m_Center;
		internal Vector2 Center
		{
			get
			{
				return m_Center;
			}
		}

		#endregion // End of variables and Properties Region

		//This initializes the Game texture.
		internal GameTexture(String p_Name, int p_Tag, Texture2D p_Texture, Rectangle p_StartPosition)
		{
			Name = p_Name;
			Tag = p_Tag;
			Texture = p_Texture;

			StartPosition = p_StartPosition;
			m_Center = new Vector2(Width * 0.5f, Height * 0.5f);
		}
	}
}