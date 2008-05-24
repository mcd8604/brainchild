using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob
{

	internal class CameraBody : Body
	{

		PhysicsPoint point;
		TaskCamera task;

		public bool UseLargeCameraCollision = true;
		public float CameraRadius = 2f;

		public CameraBody(Body Target)
		{
			point = new PhysicsPoint(Vector3.Zero);
			points.Add(point);

			if (UseLargeCameraCollision)
			{
				for (int i = 0; i < 6; ++i)
				{
					points.Add(new PhysicsPoint(Vector3.Zero));
				}
			}

			task = new TaskCamera(Target);
			tasks.Add(task);

			initialize();
		}

		public Vector3 getCameraPosition()
		{
			if (UseLargeCameraCollision)
			{
				Vector3 center = Vector3.Zero;
				foreach (PhysicsPoint p in points)
				{
					center += p.ExternalPosition;
					if (p.LastCollision != null)
					{
						center += p.LastCollision.Normal * CameraRadius;
					}
				}
				return center /= points.Count;
			}
			else
			{
				return point.ExternalPosition;
			}
		}

		public override void update(float TotalElapsedSeconds)
		{
			task.update(this, TotalElapsedSeconds);
			boundingBox.clear();
			foreach (Physics2.PhysicsPoint p in points)
			{
				boundingBox.expandToInclude(p.CurrentPosition);
				boundingBox.expandToInclude(p.PotentialPosition);
			}
		}

		public override void updatePosition()
		{
			foreach (Physics2.PhysicsPoint p in points)
			{
				p.updatePosition();
			}
		}

		public void setCameraOffset(Vector3 Offset)
		{
			task.OffsetVector = Offset;
		}

		public void setCameraTarget(Body Target)
		{
			task.Target = Target;
		}

	}
}
