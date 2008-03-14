using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo3
{
	public class DemoCube
	{
		public readonly List<Point> points = new List<Point>();
		public readonly List<Spring> springs = new List<Spring>();

		public float friction = 0.9f;
		public static float springVal = 62.5f;

		Point ftr;
		Point ftl;
		Point fbr;
		Point fbl;
		Point btr;
		Point btl;
		Point bbr;
		Point bbl;

		//Point Center;

		public void setSpringForce(float force)
		{
			springVal = force;
			foreach (Spring s in springs)
			{
				s.Force = force;
			}
		}

		public Vector3 getCenter()
		{
			Vector3 ret = Vector3.Zero;
			foreach (Point p in points)
			{
			    ret += p.Position;
			}
			return ret / points.Count;
			//return Center.Position;
		}

		public DemoCube(Vector3 center, float radius)
		{
			initCube(center, radius);
		}

		private void initCube(Vector3 center, float radius)
		{

			ftr = new Point(center + new Vector3(radius, radius, radius));
			ftl = new Point(center + new Vector3(-radius, radius, radius));
			fbr = new Point(center + new Vector3(radius, -radius, radius));
			fbl = new Point(center + new Vector3(-radius, -radius, radius));
			btr = new Point(center + new Vector3(radius, radius, -radius));
			btl = new Point(center + new Vector3(-radius, radius, -radius));
			bbr = new Point(center + new Vector3(radius, -radius, -radius));
			bbl = new Point(center + new Vector3(-radius, -radius, -radius));

			//Center = new Point(center);
			//Center.mass = 0f;

			List<Point> tempList = new List<Point>();

			//tempList.Add(Center);
			tempList.Add(ftr); tempList.Add(ftl); tempList.Add(fbr); tempList.Add(fbl); tempList.Add(btr); tempList.Add(btl); tempList.Add(bbr); tempList.Add(bbl);

			foreach (Point t in tempList)
			{
				foreach (Point p in points)
				{
					springs.Add(new Spring(t, p, Vector3.Distance(t.getCurrentPosition(), p.getCurrentPosition()), springVal));
				}
				points.Add(t);
			}

		}

		public VertexPositionTexture[] getTriangleVertexes()
		{
			VertexPositionTexture[] vertices = new VertexPositionTexture[16];

			vertices[0] = new VertexPositionTexture(ftr.Position, Vector2.Zero);
			vertices[1] = new VertexPositionTexture(fbr.Position, new Vector2(0f, 1f));
			vertices[2] = new VertexPositionTexture(bbr.Position, Vector2.One);
			vertices[3] = new VertexPositionTexture(btr.Position, new Vector2(1f, 0f));
			vertices[4] = new VertexPositionTexture(btl.Position, Vector2.One);
			vertices[5] = new VertexPositionTexture(ftl.Position, new Vector2(0f, 1f));
			vertices[6] = new VertexPositionTexture(fbl.Position, Vector2.One);
			vertices[7] = new VertexPositionTexture(fbr.Position, new Vector2(1f, 0f));

			vertices[8] = new VertexPositionTexture(bbl.Position, Vector2.Zero);
			vertices[9] = new VertexPositionTexture(bbr.Position, new Vector2(0f, 1f));

			vertices[10] = new VertexPositionTexture(fbr.Position, Vector2.One);
			vertices[11] = new VertexPositionTexture(fbl.Position, new Vector2(1f, 0f));
			vertices[12] = new VertexPositionTexture(ftl.Position, Vector2.One);
			vertices[13] = new VertexPositionTexture(btl.Position, new Vector2(0f, 1f));
			vertices[14] = new VertexPositionTexture(btr.Position, Vector2.One);

			vertices[15] = new VertexPositionTexture(bbr.Position, new Vector2(1f, 0f));

			return vertices;
		}

		public void DrawMe(GraphicsDevice device)
		{
			device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 6);
			device.DrawPrimitives(PrimitiveType.TriangleFan, 8, 6);
		}


	}
}
