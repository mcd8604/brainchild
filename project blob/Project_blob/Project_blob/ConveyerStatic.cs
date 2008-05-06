using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
    class ConveyerStatic :BodyStatic
    {

        Vector3 vel = new Vector3(10, 10, 0);

        public ConveyerStatic(IList<CollidableStatic> Collidables, Body ParentBody)
			: base(Collidables, ParentBody)
		{
		}

        public override Vector3 getRelativeVelocity(PhysicsPoint p)
        {
            return vel;
        }

    }
}
