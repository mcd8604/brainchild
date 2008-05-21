using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{
	internal class TaskCamera : Task
	{
		public Vector3 OffsetVector = Vector3.Zero;
		public Body Target;

		private Vector3[] offsets = new Vector3[6];

		public TaskCamera(Body target)
		{
			Target = target;
			offsets[0] = Vector3.Forward;
			offsets[1] = Vector3.Backward;
			offsets[2] = Vector3.Left;
			offsets[3] = Vector3.Right;
			offsets[4] = Vector3.Up;
			offsets[5] = Vector3.Down;
		}

		public override void update(Physics2.Body b, float time)
		{
			Vector3 Center = Target.getCenter();
			Vector3 Result = Center + OffsetVector;
			IList<Physics2.PhysicsPoint> points = b.getPoints();

			if (((CameraBody)b).UseLargeCameraCollision)
			{
				for (int i = 0; i < 6; ++i)
				{
					points[i + 1].CurrentPosition = points[0].CurrentPosition;
					points[i + 1].PotentialPosition = points[0].CurrentPosition + (offsets[i] * ((CameraBody)b).CameraRadius);
				}
			}

			points[0].CurrentPosition = Center;
			points[0].PotentialPosition = Result;

			//for (int i = 0; i < 6; ++i)
			//{
			//    points[i + 1].CurrentPosition = Center;
			//    points[i + 1].PotentialPosition = Result + offsets[i];
			//}
		}

	}
}
