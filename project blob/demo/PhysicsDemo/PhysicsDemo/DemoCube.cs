using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo
{
	public class DemoCube
	{
		public readonly List<Point> points = new List<Point>();
		public readonly List<Spring> springs = new List<Spring>();

		public static float springVal = 62.5f;



		Point ftr;
		Point ftl;
		Point fbr;
		Point fbl;
		Point btr;
		Point btl;
		Point bbr;
		Point bbl;



		public void setSpringForce(float force)
		{
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

			List<Point> tempList = new List<Point>();

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

#if TEXTURE
		public VertexPositionNormalTexture[] getTriangleVertexes()
		{
			VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[36];

			Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
			Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
			Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);
			Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);

			Vector3 normal;

			// front
			normal = new Plane(ftl.Position, fbl.Position, ftr.Position).Normal;
			vertexes[0] = new VertexPositionNormalTexture(ftl.Position, normal, textureTopLeft);
			vertexes[1] = new VertexPositionNormalTexture(fbl.Position, normal, textureBottomLeft);
			vertexes[2] = new VertexPositionNormalTexture(ftr.Position, normal, textureTopRight);
			normal = new Plane(fbl.Position, fbr.Position, ftr.Position).Normal;
			vertexes[3] = new VertexPositionNormalTexture(fbl.Position, normal, textureBottomLeft);
			vertexes[4] = new VertexPositionNormalTexture(fbr.Position, normal, textureBottomRight);
			vertexes[5] = new VertexPositionNormalTexture(ftr.Position, normal, textureTopRight);

			// back
			normal = new Plane(btl.Position, btr.Position, bbl.Position).Normal;
			vertexes[6] = new VertexPositionNormalTexture(btl.Position, normal, textureTopRight);
			vertexes[7] = new VertexPositionNormalTexture(btr.Position, normal, textureTopLeft);
			vertexes[8] = new VertexPositionNormalTexture(bbl.Position, normal, textureBottomRight);
			normal = new Plane(bbl.Position, btr.Position, bbr.Position).Normal;
			vertexes[9] = new VertexPositionNormalTexture(bbl.Position, normal, textureBottomRight);
			vertexes[10] = new VertexPositionNormalTexture(btr.Position, normal, textureTopLeft);
			vertexes[11] = new VertexPositionNormalTexture(bbr.Position, normal, textureBottomLeft);

			// top
			normal = new Plane(ftl.Position, btr.Position, btl.Position).Normal;
			vertexes[12] = new VertexPositionNormalTexture(ftl.Position, normal, textureBottomLeft);
			vertexes[13] = new VertexPositionNormalTexture(btr.Position, normal, textureTopRight);
			vertexes[14] = new VertexPositionNormalTexture(btl.Position, normal, textureTopLeft);
			normal = new Plane(ftl.Position, ftr.Position, btr.Position).Normal;
			vertexes[15] = new VertexPositionNormalTexture(ftl.Position, normal, textureBottomLeft);
			vertexes[16] = new VertexPositionNormalTexture(ftr.Position, normal, textureBottomRight);
			vertexes[17] = new VertexPositionNormalTexture(btr.Position, normal, textureTopRight);

			// bottom
			normal = new Plane(fbl.Position, bbl.Position, bbr.Position).Normal;
			vertexes[18] = new VertexPositionNormalTexture(fbl.Position, normal, textureTopLeft);
			vertexes[19] = new VertexPositionNormalTexture(bbl.Position, normal, textureBottomLeft);
			vertexes[20] = new VertexPositionNormalTexture(bbr.Position, normal, textureBottomRight);
			normal = new Plane(fbl.Position, bbr.Position, fbr.Position).Normal;
			vertexes[21] = new VertexPositionNormalTexture(fbl.Position, normal, textureTopLeft);
			vertexes[22] = new VertexPositionNormalTexture(bbr.Position, normal, textureBottomRight);
			vertexes[23] = new VertexPositionNormalTexture(fbr.Position, normal, textureTopRight);

			// left
			normal = new Plane(ftl.Position, bbl.Position, fbl.Position).Normal;
			vertexes[24] = new VertexPositionNormalTexture(ftl.Position, normal, textureTopRight);
			vertexes[25] = new VertexPositionNormalTexture(bbl.Position, normal, textureBottomLeft);
			vertexes[26] = new VertexPositionNormalTexture(fbl.Position, normal, textureBottomRight);
			normal = new Plane(btl.Position, bbl.Position, ftl.Position).Normal;
			vertexes[27] = new VertexPositionNormalTexture(btl.Position, normal, textureTopLeft);
			vertexes[28] = new VertexPositionNormalTexture(bbl.Position, normal, textureBottomLeft);
			vertexes[29] = new VertexPositionNormalTexture(ftl.Position, normal, textureTopRight);

			// right
			normal = new Plane(ftr.Position, fbr.Position, bbr.Position).Normal;
			vertexes[30] = new VertexPositionNormalTexture(ftr.Position, normal, textureTopLeft);
			vertexes[31] = new VertexPositionNormalTexture(fbr.Position, normal, textureBottomLeft);
			vertexes[32] = new VertexPositionNormalTexture(bbr.Position, normal, textureBottomRight);
			normal = new Plane(btr.Position, ftr.Position, bbr.Position).Normal;
			vertexes[33] = new VertexPositionNormalTexture(btr.Position, normal, textureTopRight);
			vertexes[34] = new VertexPositionNormalTexture(ftr.Position, normal, textureTopLeft);
			vertexes[35] = new VertexPositionNormalTexture(bbr.Position, normal, textureBottomRight);
			

			return vertexes;

		}
#else
		public VertexPositionColor[] getTriangleVertexes()
		{
			VertexPositionColor[] vertexes = new VertexPositionColor[36];
			
			// front
			vertexes[0] = new VertexPositionColor(ftl.Position, Color.Red);
			vertexes[1] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[2] = new VertexPositionColor(ftr.Position, Color.Blue);
			vertexes[3] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[4] = new VertexPositionColor(fbr.Position, Color.Green);
			vertexes[5] = new VertexPositionColor(ftr.Position, Color.Blue);

			// back
			vertexes[6] = new VertexPositionColor(btl.Position, Color.Yellow);
			vertexes[7] = new VertexPositionColor(btr.Position, Color.Black);
			vertexes[8] = new VertexPositionColor(bbl.Position, Color.Purple);
			vertexes[9] = new VertexPositionColor(bbl.Position, Color.Purple);
			vertexes[10] = new VertexPositionColor(btr.Position, Color.Black);
			vertexes[11] = new VertexPositionColor(bbr.Position, Color.Orange);

			// top
			vertexes[12] = new VertexPositionColor(ftl.Position, Color.Red);
			vertexes[13] = new VertexPositionColor(btr.Position, Color.Black);
			vertexes[14] = new VertexPositionColor(btl.Position, Color.Yellow);
			vertexes[15] = new VertexPositionColor(ftl.Position, Color.Red);
			vertexes[16] = new VertexPositionColor(ftr.Position, Color.Blue);
			vertexes[17] = new VertexPositionColor(btr.Position, Color.Black);

			// bottom
			vertexes[18] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[19] = new VertexPositionColor(bbl.Position, Color.Purple);
			vertexes[20] = new VertexPositionColor(bbr.Position, Color.Orange);
			vertexes[21] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[22] = new VertexPositionColor(bbr.Position, Color.Orange);
			vertexes[23] = new VertexPositionColor(fbr.Position, Color.Green);

			// left
			vertexes[24] = new VertexPositionColor(ftl.Position, Color.Red);
			vertexes[25] = new VertexPositionColor(bbl.Position, Color.Purple);
			vertexes[26] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[27] = new VertexPositionColor(btl.Position, Color.Yellow);
			vertexes[28] = new VertexPositionColor(bbl.Position, Color.Purple);
			vertexes[29] = new VertexPositionColor(ftl.Position, Color.Red);

			// right
			vertexes[30] = new VertexPositionColor(ftr.Position, Color.Blue);
			vertexes[31] = new VertexPositionColor(fbr.Position, Color.Green);
			vertexes[32] = new VertexPositionColor(bbr.Position, Color.Orange);
			vertexes[33] = new VertexPositionColor(btr.Position, Color.Black);
			vertexes[34] = new VertexPositionColor(ftr.Position, Color.Blue);
			vertexes[35] = new VertexPositionColor(bbr.Position, Color.Orange);
			
			return vertexes;

		}
#endif


	}
}
