using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
    class BodyStaticConveyerBelt :BodyStatic
    {

        Vector3 vel = new Vector3(10, 10, 0);

        public BodyStaticConveyerBelt(IList<CollidableStatic> Collidables, Body ParentBody)
			: base(Collidables, ParentBody)
		{
		}

        public override Vector3 getRelativeVelocity(CollisionEvent e)
        {
            return vel;
        }

    }
}
