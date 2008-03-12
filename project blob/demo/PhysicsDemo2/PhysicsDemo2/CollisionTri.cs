using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PhysicsDemo2
{
	// not the best way of doing this
	public class CollisionTri
	{
		
		Plane myPlane;
		Vector3 max;
		Vector3 min;

		public CollisionTri(Vector3 point1, Vector3 point2, Vector3 point3)
		{
			myPlane = new Plane(point1, point2, point3);

			max = point1;
			min = point1;

			if (point2.X > max.X)
			{
				max.X = point2.X;
			}
			if (point2.Y > max.Y)
			{
				max.Y = point2.Y;
			}
			if (point2.Z > max.Z)
			{
				max.Z = point2.Z;
			}
			if (point2.X < min.X)
			{
				min.X = point2.X;
			}
			if (point2.Y < min.Y)
			{
				min.Y = point2.Y;
			}
			if (point2.Z < min.Z)
			{
				min.Z = point2.Z;
			}

			if (point3.X > max.X)
			{
				max.X = point3.X;
			}
			if (point3.Y > max.Y)
			{
				max.Y = point3.Y;
			}
			if (point3.Z > max.Z)
			{
				max.Z = point3.Z;
			}
			if (point3.X < min.X)
			{
				min.X = point3.X;
			}
			if (point3.Y < min.Y)
			{
				min.Y = point3.Y;
			}
			if (point3.Z < min.Z)
			{
				min.Z = point3.Z;
			}
		}

		public Vector3 doIntersect(Vector3 start, Vector3 end)
		{

			float lastVal = myPlane.DotNormal(start);
			float thisVal = myPlane.DotNormal(end);
			if (lastVal > 0 && thisVal < 0) // we were 'above' now 'behind'
			{
				//float u = lastVal / (lastVal - thisVal);
				//Vector3 newPos = (lastPos * (1 - u)) + (p.Position * u);
				
				///Vector3 newPos = start;

				//p.Velocity = Vector3.Zero;
				//p.Position = newPos;


				// check if intersection is inside max/min

				// if so, push off plane by normal

			}
			else
			{

			}
			return Vector3.Zero;
		}

	}
}
