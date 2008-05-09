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

		public SpeedStatic(IList<CollidableStatic> Collidables, Body ParentBody)
			: base(Collidables, ParentBody)
		{
		}

		public override Vector3 getRelativeVelocity(CollisionEvent e)
		{
			Vector3 AvgVel = Vector3.Zero;
			IList<PhysicsPoint> points = e.point.ParentBody.getPoints();
			foreach (PhysicsPoint px in points)
			{
				AvgVel += px.NextVelocity;
			}
			AvgVel /= points.Count;

			AvgVel = (Vector3.Cross(e.collidable.Normal, Vector3.Cross(AvgVel, e.collidable.Normal)));

			return AvgVel * bonus;
		}
	}
}
