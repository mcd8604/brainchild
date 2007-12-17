using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class PlayerSprite:Sprite, iBoundedSprite
    {
        private int mXSpeed = 5;
        public int XSpeed
        {
            get
            {
                return mXSpeed;
            }
            set
            {
                mXSpeed = value;
            }
        }
        
        private int mYSpeed = 5;
        public int YSpeed
        {
            get
            {
                return mYSpeed;
            }
            set
            {
                mYSpeed = value;
            }
        }

        private Rectangle mBoundary;
        public Rectangle Boundary
        {
            get
            {
                return mBoundary;
            }
            set
            {
                mBoundary = value;
            }

        }

        //The time between shots
        float mShotDelay = 0.2f;

        //keeps track of the time between shots
        float mShootElapsed = 0;
        
       



        public PlayerSprite(Rectangle destination, Rectangle source, Texture2D texture, Rectangle Boundary)
            : base(destination, source, texture)
        {
            mBoundary = Boundary;
        }

        public void assignKeys(){
            KeyBoardManager.addMapping(Keys.A,new KeyBoardAction( this,"moveLeft","Left"));
            KeyBoardManager.addMapping(Keys.D,new KeyBoardAction( this,"moveRight","Right"));
            KeyBoardManager.addMapping(Keys.W,new KeyBoardAction( this,"moveUp","Up"));
            KeyBoardManager.addMapping(Keys.S,new KeyBoardAction( this,"moveDown","Down"));
            KeyBoardManager.addMapping(Keys.Space, new KeyBoardAction(this, "shoot", "Shoot"));
        }

        

        public void shoot(float elapsed)
          {

            mShootElapsed -= elapsed;

            if (mShootElapsed <= 0)
            {
                mShootElapsed = mShotDelay;
                ShotManager.addShot(new Shot(new Rectangle((int)(Position.X) + 28, (int)(Position.Y), 9, 74), new Rectangle(0, 0, 16, 128), TextureLibrary.get("Shot"), 0, 8, 0, 400, true));
                ShotManager.addShot(new Shot(new Rectangle((int)(Position.X) + 61, (int)(Position.Y), 9, 74), new Rectangle(0, 0, 16, 128), TextureLibrary.get("Shot"), 0, 8, 0, 400, true));
                //ShotManager.addShot(new Shot(new Rectangle((int)(Position.X) + 61, (int)(Position.Y), 9, 74), new Rectangle(0, 0, 16, 128), TextureLibrary.get("Shot"), 5, 5, 200, 400, true));
            }
            
        }

        public void update(float elapsed){
            Collisions.KeepInBounds(this);
        }

        //This is for implementing the boundable interface
        public Rectangle getBoundary()
        {
            return mBoundary;
        }

        public void moveLeft(float elapsed)
        {
             mPosition.X -= mXSpeed;
        }

        public void moveRight(float elapsed)
        {
            mPosition.X += mXSpeed;
        }
        public void moveUp(float elapsed)
        {
            mPosition.Y -= mYSpeed;
        }
        public void moveDown(float elapsed)
        {
            mPosition.Y += mYSpeed;
        }
    }
}
