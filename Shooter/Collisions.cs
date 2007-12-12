using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Collisions
    {

        public static bool IntersectPixels(Sprite spriteA,Sprite spriteB)
        {
            Color[] spriteATextureData = new Color[spriteA.Texture.Width * spriteA.Texture.Height];
            spriteA.Texture.GetData(spriteATextureData);

            Color[] spriteBTextureData = new Color[spriteB.Texture.Width * spriteB.Texture.Height];
            spriteB.Texture.GetData(spriteBTextureData);

            Rectangle rectangleA = spriteA.Destination;
            Rectangle rectangleB = spriteB.Destination;

            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    try
                    {
                        Color colorA = spriteATextureData[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
                        Color colorB = spriteBTextureData[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }catch(Exception e){

                    }
                }
            }

            // No intersection found
            return false;
        }

        public static Boolean KeepInBounds(iBoundedSprite sprite)
        {
            Boolean aCollision = false;
            
            Rectangle mBoundary = sprite.Boundary;
            
            //Check to see if the sprite has moved out of the allowed area
            if (sprite.Position.X > mBoundary.Right - sprite.Width)
            {
                Vector2 pos = sprite.Position;

                pos.X = mBoundary.Right - sprite.Width;
                sprite.Position = pos;
                //mPosition.X = mBoundary.Right-Width;
                aCollision = true;
            }
            else if (sprite.Position.X < mBoundary.Left)
            {
                Vector2 pos = sprite.Position;

                pos.X = mBoundary.Left;
                sprite.Position = pos;             
                aCollision = true;
            }

            if (sprite.Position.Y > mBoundary.Bottom - sprite.Height)
            {
                Vector2 pos = sprite.Position;

                pos.Y = mBoundary.Bottom - sprite.Height;
                sprite.Position = pos;
            }
            else if (sprite.Position.Y < mBoundary.Top)
            {
                Vector2 pos = sprite.Position;

                pos.Y = mBoundary.Top;
                sprite.Position = pos;
                aCollision = true;
            }

            return aCollision;
        }

    }
}
