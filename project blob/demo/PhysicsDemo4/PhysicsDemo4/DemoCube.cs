using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo4
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

		public VertexPositionNormalTexture[] getTriangleVertexes()
		{
			VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[16];


			// ----- normals?

			Vector3 normal0;
			Vector3 normal1;
			Vector3 normal2;
			Vector3 normal3;
			Vector3 normal4;
			Vector3 normal5;
			Vector3 normal6;
			Vector3 normal7;
			Vector3 normal8;
			Vector3 normal9;
			Vector3 normal10;
			Vector3 normal11;

			normal0 = new Plane(ftl.Position, fbl.Position, ftr.Position).Normal;
			normal1 = new Plane(fbl.Position, fbr.Position, ftr.Position).Normal;
			normal2 = new Plane(btl.Position, btr.Position, bbl.Position).Normal;
			normal3 = new Plane(bbl.Position, btr.Position, bbr.Position).Normal;
			normal4 = new Plane(ftl.Position, btr.Position, btl.Position).Normal;
			normal5 = new Plane(ftl.Position, ftr.Position, btr.Position).Normal;
			normal6 = new Plane(fbl.Position, bbl.Position, bbr.Position).Normal;
			normal7 = new Plane(fbl.Position, bbr.Position, fbr.Position).Normal;
			normal8 = new Plane(ftl.Position, bbl.Position, fbl.Position).Normal;
			normal9 = new Plane(btl.Position, bbl.Position, ftl.Position).Normal;
			normal10 = new Plane(ftr.Position, fbr.Position, bbr.Position).Normal;
			normal11 = new Plane(btr.Position, ftr.Position, bbr.Position).Normal;

			//sum the normals of each plane that a vector is a part of, then normalize the result
			//this allows for gradual lighting over a plane
			Vector3 normal_ftl = Vector3.Normalize(Vector3.Add(normal9, Vector3.Add(normal4, Vector3.Add(normal5, Vector3.Add(normal0, normal8)))));
			Vector3 normal_fbl = Vector3.Normalize(Vector3.Add(normal6, Vector3.Add(normal7, Vector3.Add(normal1, Vector3.Add(normal0, normal8)))));
			Vector3 normal_ftr = Vector3.Normalize(Vector3.Add(normal5, Vector3.Add(normal11, Vector3.Add(normal10, Vector3.Add(normal0, normal1)))));
			Vector3 normal_fbr = Vector3.Normalize(Vector3.Add(normal7, Vector3.Add(normal1, normal10)));

			Vector3 normal_bbl = Vector3.Normalize(Vector3.Add(normal9, Vector3.Add(normal8, Vector3.Add(normal2, Vector3.Add(normal6, normal3)))));
			Vector3 normal_bbr = Vector3.Normalize(Vector3.Add(normal3, Vector3.Add(normal11, Vector3.Add(normal10, Vector3.Add(normal6, normal7)))));
			Vector3 normal_btl = Vector3.Normalize(Vector3.Add(normal4, Vector3.Add(normal2, normal9)));
			Vector3 normal_btr = Vector3.Normalize(Vector3.Add(normal5, Vector3.Add(normal4, Vector3.Add(normal11, Vector3.Add(normal3, normal2)))));

			// ----- end normals


			vertices[0] = new VertexPositionNormalTexture(ftr.Position, normal_ftr, Vector2.Zero);
			vertices[1] = new VertexPositionNormalTexture(fbr.Position, normal_fbr, new Vector2(0f, 1f));
			vertices[2] = new VertexPositionNormalTexture(bbr.Position, normal_bbr, Vector2.One);
			vertices[3] = new VertexPositionNormalTexture(btr.Position, normal_btr, new Vector2(1f, 0f));
			vertices[4] = new VertexPositionNormalTexture(btl.Position, normal_btl, Vector2.One);
			vertices[5] = new VertexPositionNormalTexture(ftl.Position, normal_ftl, new Vector2(0f, 1f));
			vertices[6] = new VertexPositionNormalTexture(fbl.Position, normal_fbl, Vector2.One);
			vertices[7] = new VertexPositionNormalTexture(fbr.Position, normal_fbr, new Vector2(1f, 0f));
			vertices[8] = new VertexPositionNormalTexture(bbl.Position, normal_bbl, Vector2.Zero);
			vertices[9] = new VertexPositionNormalTexture(bbr.Position, normal_bbr, new Vector2(0f, 1f));
			vertices[10] = new VertexPositionNormalTexture(fbr.Position, normal_fbr, Vector2.One);
			vertices[11] = new VertexPositionNormalTexture(fbl.Position, normal_fbl, new Vector2(1f, 0f));
			vertices[12] = new VertexPositionNormalTexture(ftl.Position, normal_ftl, Vector2.One);
			vertices[13] = new VertexPositionNormalTexture(btl.Position, normal_btl, new Vector2(0f, 1f));
			vertices[14] = new VertexPositionNormalTexture(btr.Position, normal_btr, Vector2.One);
			vertices[15] = new VertexPositionNormalTexture(bbr.Position, normal_bbr, new Vector2(1f, 0f));

			return vertices;
		}

		public void DrawMe(GraphicsDevice device)
		{
			device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 6);
			device.DrawPrimitives(PrimitiveType.TriangleFan, 8, 6);
		}


	}
}
