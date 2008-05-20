using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	class SpeedStatic : Body
	{

		float bonus = 1;

		public SpeedStatic(List<CollidableStatic> Collidables, Body ParentBody, float p_Speed)
			: base(Collidables, ParentBody)
		{
			bonus = p_Speed;
		}

		public override Vector3 getRelativeVelocity(CollisionEvent e)
		{
			// debugging:

			Vector3 av = e.point.ParentBody.getAverageVelocity();

			Vector3 cross1 = Vector3.Cross(av , e.collidable.Normal);

			Vector3 cross2 = Vector3.Cross(e.collidable.Normal, cross1);

			return cross2 * bonus;
		}
	}
}
