using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo6
{
	public class DemoCube : Drawable, Physics.Body
	{
		public readonly List<Physics.Point> points = new List<Physics.Point>();
		public readonly List<Physics.Spring> springs = new List<Physics.Spring>();
		public readonly List<Tri> collidables = new List<Tri>();

		//public float friction = 0.9f;
		public static float springVal = 62.5f;

		Physics.Point ftr;
		Physics.Point ftl;
		Physics.Point fbr;
		Physics.Point fbl;
		Physics.Point btr;
		Physics.Point btl;
		Physics.Point bbr;
		Physics.Point bbl;

		//Point Center;

		VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[16];

		private GraphicsDevice theDevice;
		private VertexBuffer myVertexBuffer;

        //public void setSpringForce(float force)
        //{
        //    springVal = force;
        //    foreach (Physics.Spring s in springs)
        //    {
        //        s.Force = force;
        //    }
        //}

		private Vector3 min;
		private Vector3 max;

        public void setSpringLength(float delta)
        {

            foreach (Physics.Spring s in springs)
            {
                s.maximumLengthBeforeExtension += delta;
                s.minimumLengthBeforeCompression += delta;
            }

        }

		public Vector3 getCenter()
		{
			Vector3 ret = Vector3.Zero;
			foreach (Physics.Point p in points)
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

			ftr = new Physics.Point(center + new Vector3(radius, radius, radius));
			ftl = new Physics.Point(center + new Vector3(-radius, radius, radius));
			fbr = new Physics.Point(center + new Vector3(radius, -radius, radius));
			fbl = new Physics.Point(center + new Vector3(-radius, -radius, radius));
			btr = new Physics.Point(center + new Vector3(radius, radius, -radius));
			btl = new Physics.Point(center + new Vector3(-radius, radius, -radius));
			bbr = new Physics.Point(center + new Vector3(radius, -radius, -radius));
			bbl = new Physics.Point(center + new Vector3(-radius, -radius, -radius));

			//Center = new Point(center);
			//Center.mass = 0f;

			List<Physics.Point> tempList = new List<Physics.Point>();

			//tempList.Add(Center);
			tempList.Add(ftr); tempList.Add(ftl); tempList.Add(fbr); tempList.Add(fbl); tempList.Add(btr); tempList.Add(btl); tempList.Add(bbr); tempList.Add(bbl);

			foreach (Physics.Point t in tempList)
			{
				foreach (Physics.Point p in points)
				{
					springs.Add(new Physics.Spring(t, p, Vector3.Distance(t.getCurrentPosition(), p.getCurrentPosition()), springVal));
				}
				points.Add(t);
			}

			collidables.Add(new Tri(ftr, fbr, bbr, Color.White));
			collidables.Add(new Tri(ftr, bbr, btr, Color.White));
			collidables.Add(new Tri(ftr, btr, btl, Color.White));
			collidables.Add(new Tri(ftr, btl, ftl, Color.White));
			collidables.Add(new Tri(ftr, ftl, fbl, Color.White));
			collidables.Add(new Tri(ftr, fbl, fbr, Color.White));

			collidables.Add(new Tri(bbl, bbr, fbr, Color.White));
			collidables.Add(new Tri(bbl, fbr, fbl, Color.White));
			collidables.Add(new Tri(bbl, fbl, ftl, Color.White));
			collidables.Add(new Tri(bbl, ftl, btl, Color.White));
			collidables.Add(new Tri(bbl, btl, btr, Color.White));
			collidables.Add(new Tri(bbl, btr, bbr, Color.White));

			vertices[0] = new VertexPositionNormalTexture(ftr.Position, Vector3.Up, Vector2.Zero);
			vertices[1] = new VertexPositionNormalTexture(fbr.Position, Vector3.Up, new Vector2(0f, 1f));
			vertices[2] = new VertexPositionNormalTexture(bbr.Position, Vector3.Up, Vector2.One);
			vertices[3] = new VertexPositionNormalTexture(btr.Position, Vector3.Up, new Vector2(1f, 0f));
			vertices[4] = new VertexPositionNormalTexture(btl.Position, Vector3.Up, Vector2.One);
			vertices[5] = new VertexPositionNormalTexture(ftl.Position, Vector3.Up, new Vector2(0f, 1f));
			vertices[6] = new VertexPositionNormalTexture(fbl.Position, Vector3.Up, Vector2.One);
			vertices[7] = new VertexPositionNormalTexture(fbr.Position, Vector3.Up, new Vector2(1f, 0f));
			vertices[8] = new VertexPositionNormalTexture(bbl.Position, Vector3.Up, Vector2.Zero);
			vertices[9] = new VertexPositionNormalTexture(bbr.Position, Vector3.Up, new Vector2(0f, 1f));
			vertices[10] = new VertexPositionNormalTexture(fbr.Position, Vector3.Up, Vector2.One);
			vertices[11] = new VertexPositionNormalTexture(fbl.Position, Vector3.Up, new Vector2(1f, 0f));
			vertices[12] = new VertexPositionNormalTexture(ftl.Position, Vector3.Up, Vector2.One);
			vertices[13] = new VertexPositionNormalTexture(btl.Position, Vector3.Up, new Vector2(0f, 1f));
			vertices[14] = new VertexPositionNormalTexture(btr.Position, Vector3.Up, Vector2.One);
			vertices[15] = new VertexPositionNormalTexture(bbr.Position, Vector3.Up, new Vector2(1f, 0f));

            //baseVolume = (2 * radius) * (2 * radius) * (2 * radius);

		}

		public VertexPositionNormalTexture[] getTriangleVertexes()
		{

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

			/*
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
			 */

			vertices[0].Position = ftr.Position;
			vertices[0].Normal = normal_ftr;
			vertices[1].Position = fbr.Position;
			vertices[1].Normal = normal_fbr;
			vertices[2].Position = bbr.Position;
			vertices[2].Normal = normal_bbr;
			vertices[3].Position = btr.Position;
			vertices[3].Normal = normal_btr;
			vertices[4].Position = btl.Position;
			vertices[4].Normal = normal_btl;
			vertices[5].Position = ftl.Position;
			vertices[5].Normal = normal_ftl;
			vertices[6].Position = fbl.Position;
			vertices[6].Normal = normal_fbl;
			vertices[7].Position = fbr.Position;
			vertices[7].Normal = normal_fbr;
			vertices[8].Position = bbl.Position;
			vertices[8].Normal = normal_bbl;
			vertices[9].Position = bbr.Position;
			vertices[9].Normal = normal_bbr;
			vertices[10].Position = fbr.Position;
			vertices[10].Normal = normal_fbr;
			vertices[11].Position = fbl.Position;
			vertices[11].Normal = normal_fbl;
			vertices[12].Position = ftl.Position;
			vertices[12].Normal = normal_ftl;
			vertices[13].Position = btl.Position;
			vertices[13].Normal = normal_btl;
			vertices[14].Position = btr.Position;
			vertices[14].Normal = normal_btr;
			vertices[15].Position = bbr.Position;
			vertices[15].Normal = normal_bbr;
			

			return vertices;
		}

		public VertexBuffer getVertexBuffer()
		{
			myVertexBuffer.SetData<VertexPositionNormalTexture>(getTriangleVertexes());
			return myVertexBuffer;
		}

		public void setGraphicsDevice(GraphicsDevice device)
		{
			theDevice = device;
			myVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.SizeInBytes * 16, BufferUsage.None);
		}

		public int getVertexStride()
		{
			return VertexPositionNormalTexture.SizeInBytes;
		}

		public void DrawMe()
		{
			theDevice.DrawPrimitives(PrimitiveType.TriangleFan, 0, 6);
			theDevice.DrawPrimitives(PrimitiveType.TriangleFan, 8, 6);
		}

		
        public IEnumerable<Physics.Point> getPoints()
        {
            return points;
        }

        public IEnumerable<Physics.Collidable> getCollidables()
        {
            // Disabled collision planes for softcubes until I can figure out what's wrong.

            //List<Physics.Collidable> temp = new List<Physics.Collidable>();
            //foreach ( Tri t in collidables ) {
            //    temp.Add( t as Physics.Collidable );
            //}
            //return temp;

            return new List<Physics.Collidable>();
        }

        public IEnumerable<Drawable> getDrawables()
        {
            List<Drawable> temp = new List<Drawable>();
            foreach (Tri t in collidables)
            {
                temp.Add(t as Drawable);
            }
            return temp;
        }

        public IEnumerable<Physics.Spring> getSprings()
        {
            return springs;
        }
        
        public float getVolume()
        {
            // TODO

            // 1/3 * area of base ( face ) * height ( center of face to center of cube )

            float totalVolume = 0;

            Vector3 centerOfCube = getCenter();

			/*
            for (int i = 1; i < 7; ++i)
            {

                totalVolume += getFaceVolume(vertices[0].Position, vertices[i].Position, vertices[i + 1].Position);

            }

            totalVolume += getFaceVolume(vertices[0].Position, vertices[7].Position, vertices[1].Position);

            for (int i = 9; i < 15; ++i)
            {

                totalVolume += getFaceVolume(vertices[8].Position, vertices[i].Position, vertices[i + 1].Position);

            }

            totalVolume += getFaceVolume(vertices[8].Position, vertices[15].Position, vertices[9].Position);
			*/


			min = centerOfCube;
			max = centerOfCube;


			for (int i = 0; i < vertices.Length; i++)
			{

				if (vertices[i].Position.X < min.X)
				{
					min.X = vertices[i].Position.X;
				}
				if (vertices[i].Position.Y < min.Y)
				{
					min.Y = vertices[i].Position.Y;
				}
				if (vertices[i].Position.Z < min.Z)
				{
					min.Z = vertices[i].Position.Z;
				}

				if (vertices[i].Position.X > max.X)
				{
					max.X = vertices[i].Position.X;
				}
				if (vertices[i].Position.Y > max.Y)
				{
					max.Y = vertices[i].Position.Y;
				}
				if (vertices[i].Position.Z > max.Z)
				{
					max.Z = vertices[i].Position.Z;
				}

			}


			totalVolume = (max.X - min.X) * (max.Y - min.Y) * (max.Z - min.Z);


            return totalVolume;
        }

        private float getFaceVolume(Vector3 point1, Vector3 point2, Vector3 point3)
        {

            Vector3 a = point2 - point1;
            Vector3 b = point3 - point1;
            Vector3 c = Vector3.Cross(a, b);

            float area = 0.5f * c.Length();

            // not correct, need length along perpendicular vector
            Vector3 center = (point1 + point2 + point3) / 3f;

            float height = Vector3.Distance(getCenter(), center);

            float volume = height * area * (1f / 3f);

            return volume;

        }

        //public float baseVolume = 10f;
        //public float idealVolume = 10f;

        //public void update()
        //{

        //    Vector3 center = getCenter();
        //    float volume = getVolume();

        //    foreach (Physics.Point p in points)
        //    {

        //        p.CurrentForce += (center - p.Position) * (volume - idealVolume) * (1f);

        //    }

        //}

	}
}
