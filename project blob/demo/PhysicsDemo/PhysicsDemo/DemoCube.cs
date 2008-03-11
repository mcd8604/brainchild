using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhysicsDemo
{
	public class DemoCube
	{
		public readonly List<Point> points = new List<Point>();
		public readonly List<Spring> springs = new List<Spring>();

		public DemoCube(Vector3 center, float radius)
		{
			initCube(center, radius);
		}

		private void initCube(Vector3 center, float radius)
		{
			
			Point ftr = new Point(center + new Vector3(radius, radius, radius));
			Point ftl = new Point(center + new Vector3(-radius, radius, radius));
			Point fbr = new Point(center + new Vector3(radius, -radius, radius));
			Point fbl = new Point(center + new Vector3(-radius, -radius, radius));
			Point btr = new Point(center + new Vector3(radius, radius, -radius));
			Point btl = new Point(center + new Vector3(-radius, radius, -radius));
			Point bbr = new Point(center + new Vector3(radius, -radius, -radius));
			Point bbl = new Point(center + new Vector3(-radius, -radius, -radius));

			List<Point> tempList = new List<Point>();

			tempList.Add(ftr); tempList.Add(ftl); tempList.Add(fbr); tempList.Add(fbl); tempList.Add(btr); tempList.Add(btl); tempList.Add(bbr); tempList.Add(bbl);

			foreach (Point t in tempList)
			{
				foreach (Point p in points)
				{
					springs.Add(new Spring(t, p, Vector3.Distance(t.getCurrentPosition(), p.getCurrentPosition()), 12.5f));
				}
				points.Add(t);
			}

		}

	}
}
