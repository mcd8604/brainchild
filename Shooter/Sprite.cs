using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    //This class will be the bassis for all other sprites
    class Sprite
    {

#region Getter/Setters
        #region Position
        //Positional Coords

        protected Vector2 mStartPosition;
        public Vector2 StartPosition
        {
            get
            {
                return mStartPosition;
            }
            set
            {
                mStartPosition = value;
            }

        }

        protected Vector2 mPosition;
        public Vector2 Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }

        }

        protected int mHeight;
        public int Height
        {
            get
            {
                return mHeight;
            }
            set
            {
                mHeight = value;
            }

        }

        protected int mWidth;
        public int Width
        {
            get
            {
                return mWidth;
            }
            set
            {
                mWidth = value;
            }

        }
#endregion

        #region Texture
        //Texture Variables
        protected Texture2D mTexture;
        public Texture2D Texture
        {
            get
            {
                return mTexture;
            }
            set
            {
                mTexture = (Texture2D)value;
            }
        }

        private int mTextureStartX;
        public int TextureStartX
        {
            get
            {
                return mTextureStartX;
            }
            set
            {
                mTextureStartX = value;
            }

        }

        private int mTextureStartY;
        public int TextureStartY
        {
            get
            {
                return mTextureStartY;
            }
            set
            {
                mTextureStartY = value;
            }

        }

        private int mTextureWidth;
        public int TextureWidth
        {
            get
            {
                return mTextureWidth;
            }
            set
            {
                mTextureWidth = value;
            }

        }

        private int mTextureHeight;
        public int TextureHeight
        {
            get
            {
               return mTextureHeight;
            }
            set
            {
                mTextureHeight = value;
            }

        }
        #endregion

#region Visibility/Direction
        //Determines if the Sprite can be seen
        private Boolean mVisible;
        public Boolean Visible
        {
            get
            {
                return mVisible;
            }
            set
            {
                mVisible = value;
            }

        }


        //Direction
        public enum eDirection
        {
            None,
            Left,
            Right,
            Up,
            Down
        }
        
        //The Current direction of the Sprite
        private eDirection mDirection = eDirection.None;

        public eDirection Direction
        {
            get
            {
                return mDirection;
            }

            set
            {
                mDirection = value;
            }

        }//End Direction Property
#endregion

        //The destination Rectangle
        public Rectangle Destination
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }//End Destination property
        
        //The Source rectangle (what area in the texture will be used to apply a "skin" to the sprite)
        public Rectangle Source
        {
            get
            {
                return new Rectangle(TextureStartX, TextureStartY, TextureWidth, TextureHeight);
            }
        }//End Source property

#endregion

        //This ini's all the variables for the sprite
        public Sprite(Rectangle destination, Rectangle source, Texture2D texture)
        {
            //Sets starting position  data
            StartPosition = new Vector2(destination.X, destination.Y);
            Position = new Vector2(StartPosition.X, StartPosition.Y);
            Height = destination.Height;
            Width = destination.Width;

            //Sets texture data
            Texture = texture;
            TextureStartX = source.X;
            TextureStartY = source.Y;
            TextureWidth = source.Width;
            TextureHeight = source.Height;
            
            Direction = eDirection.None;
            Visible = false;
        }

        public Rectangle getDestination()
        {
            return new Rectangle(TextureStartX, TextureStartY, TextureWidth, TextureHeight);
        }


        public virtual void update(float elapsed)
        {
            //TODO: Implment this method in children classes
        }

        public void draw(SpriteBatch theSpriteBatch)
        {

            theSpriteBatch.Draw(Texture, Destination, Source, Color.White);

        }

    }
}
