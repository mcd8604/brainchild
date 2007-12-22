using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Wintellect.PowerCollections;

namespace project_hook
{
    /*
     * Description: This class contains the base information all sprites need.
     *              It also provides default draw and update method.
     * 
     * TODO:
     *  1. Check if rectangles can have Negative height and width values
     */
    public class Sprite
	{
    
		protected Path m_Path;
		public Path Path
		{
			get
			{
				return m_Path;
			}
			set
			{
				m_Path = value;
			}

		}

        /*
         * BigList<T> provides a list of items, in order, with indices of the items
         * ranging from 0 to one less than the count of items in the collection.
         * BigList<T> is optimized for efficient operations on large (>1000 items) lists,
         * especially for insertions, deletions, copies, and concatinations. 
         * 
         * Is this the right collection type? What are 'parts' exactly? - Adam
         */
        private List<Sprite> m_Parts;
        public List<Sprite> Parts
        {
            get
            {
                return m_Parts;
            }
            set
            {
                Parts = value;
            }
        }

		//The animation for the sprite
		private VisualEffect m_Animation;
		public VisualEffect Animation
		{
			get
			{
				return m_Animation;
			}
		}
        
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
				//if (m_Degree > 360)
				//{
			//		m_Degree = m_Degree - 360;
			//	}
			}
		}


        //This is the Z Depth value
        private float m_Z;
        public float Z
        {
            get
            {
                return m_Z;
            }
            set
            {
                m_Z = value;
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

		//This will create the destination rectangle sued to draw the sprite to the screen.
		//The spritebatch objetc needs this as a parameter.
		public Rectangle DrawDestination
		{
			get
			{
				//Rectangle center = center;
			
//				Vector2 m_Center = new Vector2(m_Texture.Width / 2.0f, m_Texture.Height / 2.0f);
				return new Rectangle((int)(Position.X + Width /2), (int)(Position.Y + Height/2 ), Width, Height);
			}
		}

		private Vector2 m_Center;
		public Vector2 Center
		{
			get
			{
				return new Vector2(Position.X + m_Center.X,Position.Y + m_Center.Y);
			}
			set
			{
				m_Position.X = value.X - m_Center.X;
                m_Position.Y = value.Y - m_Center.Y;
			}
		}

        //This is a constructor that has full parameters!
		public Sprite(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_Z)
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
            Z = p_Z;
			m_Center = new Vector2(Width / 2.0f, Height / 2.0f);
		}
				
		//sets the anmmation for the object.
		public void setAnimation(string p_Animation, int p_FramesPerSecond)
		{
			m_Animation = new VisualEffect(p_Animation, this,p_FramesPerSecond);
			m_Animation.StopAnimation();
		}
        
        
        //This will draw the sprite to the screen
		public virtual void Draw(SpriteBatch p_SpriteBatch)
		{
			if (!Visible || Texture != null)
			{
				if (m_Parts != null && m_Parts.Count > 0)
				{

					foreach (Sprite t_Sprite in m_Parts)
					{
						t_Sprite.Draw(p_SpriteBatch);
					}
				}
                

				//Rectangle draw = DrawDestination;
				//Vector2 center = Center;
				//Draws the current sprite.
				if (rot)
				{
					p_SpriteBatch.Draw(m_Texture.Texture, DrawDestination, m_Texture.StartPosition, Color.White, m_Degree,
									  Texture.Center, SpriteEffects.None, m_Z);
				}
				else
				{
					p_SpriteBatch.Draw(m_Texture.Texture, Destination, m_Texture.StartPosition, Color.White,0,Vector2.Zero,SpriteEffects.None,m_Z);
				}
				//m_Texture.Center
			}
		}

        //This update method should be overidden 
		public virtual void Update(GameTime p_Time)
		{
			if (m_Animation != null)
			{
				m_Animation.Update(p_Time);
			}
			if (Parts != null)
			{
				for (int a = 0; a < Parts.Count; a++)
				{
					Parts[a].Update(p_Time);
					
				}
			}

			if (Path != null)
			{
				Path.CalculateMovement(p_Time);
			}
		}

		public void attachSpritePart(Sprite p_Sprite)
        {
            if (m_Parts == null)
            {
                m_Parts = new List<Sprite>();
            }

            m_Parts.Add(p_Sprite);

        }

		private static Boolean rot = true;
		public static void DrawWithRot()
		{
			rot = !rot;
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