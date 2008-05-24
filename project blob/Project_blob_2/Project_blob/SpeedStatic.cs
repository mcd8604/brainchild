using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	internal class SpeedStatic : Body
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
#if DEBUG
			if (e.point.ParentBody == null)
			{
				throw new ArgumentNullException("A point collided with a speed tile that did not have a parent body!");
			}
#endif
			return Vector3.Cross(e.collidable.Normal, Vector3.Cross(e.point.ParentBody.getAverageVelocity() , e.collidable.Normal)) * bonus;
		}
	}
}
