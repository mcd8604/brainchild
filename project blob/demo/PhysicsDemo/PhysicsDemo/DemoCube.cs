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
		public VertexPositionTexture[] getTriangleVertexes()
		{
			VertexPositionTexture[] vertexes = new VertexPositionTexture[36];

			Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
			Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
			Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);
			Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);

			// front
			vertexes[0] = new VertexPositionTexture(ftl.Position, textureTopLeft);
			vertexes[1] = new VertexPositionTexture(fbl.Position, textureBottomLeft);
			vertexes[2] = new VertexPositionTexture(ftr.Position, textureTopRight);
			vertexes[3] = new VertexPositionTexture(fbl.Position, textureBottomLeft);
			vertexes[4] = new VertexPositionTexture(fbr.Position, textureBottomRight);
			vertexes[5] = new VertexPositionTexture(ftr.Position, textureTopRight);

			// back
			vertexes[6] = new VertexPositionTexture(btl.Position, textureTopRight);
			vertexes[7] = new VertexPositionTexture(btr.Position, textureTopLeft);
			vertexes[8] = new VertexPositionTexture(bbl.Position, textureBottomRight);
			vertexes[9] = new VertexPositionTexture(bbl.Position, textureBottomRight);
			vertexes[10] = new VertexPositionTexture(btr.Position, textureTopLeft);
			vertexes[11] = new VertexPositionTexture(bbr.Position, textureBottomLeft);

			// top
			vertexes[12] = new VertexPositionTexture(ftl.Position, textureBottomLeft);
			vertexes[13] = new VertexPositionTexture(btr.Position, textureTopRight);
			vertexes[14] = new VertexPositionTexture(btl.Position, textureTopLeft);
			vertexes[15] = new VertexPositionTexture(ftl.Position, textureBottomLeft);
			vertexes[16] = new VertexPositionTexture(ftr.Position, textureBottomRight);
			vertexes[17] = new VertexPositionTexture(btr.Position, textureTopRight);

			// bottom
			vertexes[18] = new VertexPositionTexture(fbl.Position, textureTopLeft);
			vertexes[19] = new VertexPositionTexture(bbl.Position, textureBottomLeft);
			vertexes[20] = new VertexPositionTexture(bbr.Position, textureBottomRight);
			vertexes[21] = new VertexPositionTexture(fbl.Position, textureTopLeft);
			vertexes[22] = new VertexPositionTexture(bbr.Position, textureBottomRight);
			vertexes[23] = new VertexPositionTexture(fbr.Position, textureTopRight);

			// left
			vertexes[24] = new VertexPositionTexture(ftl.Position, textureTopRight);
			vertexes[25] = new VertexPositionTexture(bbl.Position, textureBottomLeft);
			vertexes[26] = new VertexPositionTexture(fbl.Position, textureBottomRight);
			vertexes[27] = new VertexPositionTexture(btl.Position, textureTopLeft);
			vertexes[28] = new VertexPositionTexture(bbl.Position, textureBottomLeft);
			vertexes[29] = new VertexPositionTexture(ftl.Position, textureTopRight);

			// right
			vertexes[30] = new VertexPositionTexture(ftr.Position, textureTopLeft);
			vertexes[31] = new VertexPositionTexture(fbr.Position, textureBottomLeft);
			vertexes[32] = new VertexPositionTexture(bbr.Position, textureBottomRight);
			vertexes[33] = new VertexPositionTexture(btr.Position, textureTopRight);
			vertexes[34] = new VertexPositionTexture(ftr.Position, textureTopLeft);
			vertexes[35] = new VertexPositionTexture(bbr.Position, textureBottomRight);
			

			return vertexes;

		}
#else
		public VertexPositionColor[] getTriangleVertexes()
		{
			VertexPositionColor[] vertexes = new VertexPositionColor[36];
			
			// front
			vertexes[0] = new VertexPositionColor(ftl.Position, Color.White);
			vertexes[1] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[2] = new VertexPositionColor(ftr.Position, Color.White);
			vertexes[3] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[4] = new VertexPositionColor(fbr.Position, Color.White);
			vertexes[5] = new VertexPositionColor(ftr.Position, Color.White);

			// back
			vertexes[6] = new VertexPositionColor(btl.Position, Color.White);
			vertexes[7] = new VertexPositionColor(btr.Position, Color.White);
			vertexes[8] = new VertexPositionColor(bbl.Position, Color.White);
			vertexes[9] = new VertexPositionColor(bbl.Position, Color.White);
			vertexes[10] = new VertexPositionColor(btr.Position, Color.White);
			vertexes[11] = new VertexPositionColor(bbr.Position, Color.White);

			// top
			vertexes[12] = new VertexPositionColor(ftl.Position, Color.White);
			vertexes[13] = new VertexPositionColor(btr.Position, Color.White);
			vertexes[14] = new VertexPositionColor(btl.Position, Color.White);
			vertexes[15] = new VertexPositionColor(ftl.Position, Color.White);
			vertexes[16] = new VertexPositionColor(ftr.Position, Color.White);
			vertexes[17] = new VertexPositionColor(btr.Position, Color.White);

			// bottom
			vertexes[18] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[19] = new VertexPositionColor(bbl.Position, Color.White);
			vertexes[20] = new VertexPositionColor(bbr.Position, Color.White);
			vertexes[21] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[22] = new VertexPositionColor(bbr.Position, Color.White);
			vertexes[23] = new VertexPositionColor(fbr.Position, Color.White);

			// left
			vertexes[24] = new VertexPositionColor(ftl.Position, Color.White);
			vertexes[25] = new VertexPositionColor(bbl.Position, Color.White);
			vertexes[26] = new VertexPositionColor(fbl.Position, Color.White);
			vertexes[27] = new VertexPositionColor(btl.Position, Color.White);
			vertexes[28] = new VertexPositionColor(bbl.Position, Color.White);
			vertexes[29] = new VertexPositionColor(ftl.Position, Color.White);

			// right
			vertexes[30] = new VertexPositionColor(ftr.Position, Color.White);
			vertexes[31] = new VertexPositionColor(fbr.Position, Color.White);
			vertexes[32] = new VertexPositionColor(bbr.Position, Color.White);
			vertexes[33] = new VertexPositionColor(btr.Position, Color.White);
			vertexes[34] = new VertexPositionColor(ftr.Position, Color.White);
			vertexes[35] = new VertexPositionColor(bbr.Position, Color.White);
			
			return vertexes;

		}
#endif


	}
}
