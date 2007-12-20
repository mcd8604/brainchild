using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{
    /*
     * Description: This class holds a reference to a 2D Texture
     *              and what area of that texture should be displayed. 
     * 
     * TODO: 
     * 
     * 
     */
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
        #endregion // End of variables and Properties Region

        //This initializes the Game texture.
		public GameTexture(String p_Name, Texture2D p_Texture, Rectangle p_StartPosition)
		{
			Name = p_Name;
			Texture = p_Texture;
			StartPosition = p_StartPosition;
		}
	}
}
/*
  * Class: Game Texture
  * Authors: Karl, Eric, Mike
  * Date Created: 12/16/2007
  * 
  * Change Log:
  *     12/16/2007 - Eric, Karl, Mike - Initial Creation,  Created properties, constructor. 
  *     12/17/2007 - Karl - Added Comments 
  */