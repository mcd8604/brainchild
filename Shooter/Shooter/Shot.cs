using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Shot:Sprite
    {
        int mXSpeed = 0;
        int mYSpeed = 0;
        int mXDistance = 0;
        int mYDistance = 0;

        Boolean mPlayerShot = false;

        public Shot(Rectangle destination, Rectangle source, Texture2D texture,int xSpeed, int ySpeed, int xDistance, int yDistance, Boolean fromPlayer)
            :base(destination,source,texture)
        {
            mXSpeed = xSpeed;
            mYSpeed = ySpeed;
            mXDistance = xDistance;
            mYDistance = yDistance;
            mPlayerShot = fromPlayer;
            Visible = true;
        }

        public override void update(float elapsed)
        {

            if (Visible)
            {
                Vector2 pos = Position;
                pos.X = pos.X + mXSpeed;
                pos.Y = pos.Y - mYSpeed;
                Position = pos;

                if (mXSpeed < 0)
                {
                    mXDistance += mXSpeed;
                }
                else
                {
                    mXDistance -= mXSpeed;
                }

                if (mYSpeed < 0)
                {
                    mYDistance += mYSpeed;
                }
                else
                {
                    mYDistance -= mYSpeed;
                }

                if (mYDistance <= 0 && mXDistance <= 0)
                {
                    Visible = false;
                }
            }
        }

    }
}
