using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo6
{
    public class BounceStaticTri : StaticTri
    {

        public BounceStaticTri(Vector3 point1, Vector3 point2, Vector3 point3, Color color)
            :base(point1, point2, point3, color)
		{}

        public override bool shouldPhysicsBlock(Physics.Point p)
        {

            p.NextForce += Normal() * 1000f;

            return true;
        }

    }
}
