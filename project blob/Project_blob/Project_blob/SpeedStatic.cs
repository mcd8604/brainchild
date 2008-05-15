using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	class SpeedStatic : BodyStatic
	{

		float bonus = 1;

		public SpeedStatic(IList<CollidableStatic> Collidables, Body ParentBody, float p_Speed)
			: base(Collidables, ParentBody)
		{
			bonus = p_Speed;
		}

		public override Vector3 getRelativeVelocity(CollisionEvent e)
		{
			return (Vector3.Cross(e.collidable.Normal, Vector3.Cross(e.point.ParentBody.getAverageVelocity(), e.collidable.Normal))) * bonus;
		}
	}
}
