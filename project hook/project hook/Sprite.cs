using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{
    /*
     * Description: This class contains the base information all sprites need.
     *              It also provides default draw and update method.
     * 
     * TODO:
     *  1. Check if rectangles can have Negative height and width values
     *  2. Add rotation support in the Draw method
     */
    public class Sprite
	{
        // The identifying name of the sprite
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

        //The start position of the sprite.
        //This will be used with pathing, distance, and resetting actions.
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

        //This is the postion that the sprite will be displayed on the screen.
        //It is the start position initially.
        //This vector will be modified to move the sprite around the screen.
        //The x and y values in the vector are floats. 
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

        //This is the height of the the sprite. 
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

        //This is the width of the sprite that will be displayed
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

        //This is the texture that the sprite will display
        //This object is retrieved from the TextureLibrary object
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

        //This is the alpha value of the sprite.
        //This will determine the sprites transparency
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

        //This will determine if the sprite is to be drawn on screen.
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

		
        //This will determine the amount of rotation applied to a sprite.
        //0.0 = 12 Oclock all textures should have this orientation.
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

        //This will create the destination rectangle sued to draw the sprite to the screen.
        //The spritebatch objetc needs this as a parameter.
		public Rectangle Destination
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
			}
		}

        //This is a constructor that has full parameters!
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

        //This will draw the sprite to the screen
		public virtual void Draw(SpriteBatch p_SpriteBatch)
		{
            //TODO -  Apply rotation to the sprite based on degree.
			p_SpriteBatch.Draw(m_Texture.Texture, Destination, m_Texture.StartPosition, Color.White);
		}

        //This update method should be overidden 
		public virtual void Update(float p_Elapsed)
		{
		}		
	}
}

/*
  * Class: Sprite
  * Authors: Karl, Eric, Mike
  * Date Created: 12/16/2007
  * 
  * Change Log:
  *     12/16/2007 - Eric, Karl, Mike - Initial Creation,  Created properties, constructor, draw and update. 
  *     12/17/2007 - Karl - Added Comments 
  */