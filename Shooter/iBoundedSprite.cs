using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Shooter
{
    interface iBoundedSprite
    {
        Rectangle Boundary
        {
            get;
            //set;
        }
        Vector2 Position
        {
            get;
            set;
        }
        int Width
        {
            get;
            //set;
        }

        int Height
        {
            get;
            //set;
        }
    }
}
