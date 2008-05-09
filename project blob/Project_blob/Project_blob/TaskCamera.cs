using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	class TaskCamera : Task
	{
		public Vector3 OffsetVector = Vector3.Zero;
		public Body Target;

		public TaskCamera(Body target)
		{
			Target = target;
		}

		public override void update(Physics2.Body b, float time)
		{
			Vector3 Center = Target.getCenter();
			Vector3 Result = Center + OffsetVector;
			foreach (PhysicsPoint p in b.getPoints())
			{
				p.CurrentPosition = Center;
				p.PotentialPosition = Result;
			}
		}

	}
}
