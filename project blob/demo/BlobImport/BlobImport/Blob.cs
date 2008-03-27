using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlobImport
{
	public class Blob : Drawable, Physics.Body
	{
		public readonly List<Physics.Point> points = new List<Physics.Point>();
		public readonly List<Physics.Spring> springs = new List<Physics.Spring>();
		public readonly List<Tri> collidables = new List<Tri>();

        public static float springVal = 62.5f;

        private GraphicsDevice theDevice;

        VertexPositionNormalTexture[] vertices;
        int[] indices;

        private VertexDeclaration myVertexDeclaration;
		private VertexBuffer myVertexBuffer;
        private IndexBuffer myIndexBuffer;
        private int myVertexStride;
        private int myStreamOffset;
        private int myBaseVertex;
        private int myNumVertices;
        private int myStartIndex;
        private int myPrimitiveCount;

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

		public Blob(Model aModel)
		{
            initBlob(aModel);
		}

        public Physics.Collidable bottom;

		private void initBlob(Model blobModel)
        {
            ModelMesh mesh = blobModel.Meshes[0];
            ModelMeshPart part = mesh.MeshParts[0];

            Vector3 center = mesh.BoundingSphere.Center;

            myVertexStride = VertexPositionNormalTexture.SizeInBytes;


            Type vertexType;


			if (myVertexStride == VertexPositionColorTexture.SizeInBytes)
			{
				Console.WriteLine("VertexPositionColorTexture");
			}
            
            // VertexBuffer
            VertexPositionColorTexture[] tempVertices = new VertexPositionColorTexture[part.NumVertices];
            mesh.VertexBuffer.GetData<VertexPositionColorTexture>(tempVertices);
           
            vertices = new VertexPositionNormalTexture[tempVertices.Length];
            for(int i = 0; i < tempVertices.Length; i++) 
            {
				Vector3 testNorm = Vector3.Normalize(Vector3.Subtract(tempVertices[i].Position,center));
                vertices[i] = new VertexPositionNormalTexture(tempVertices[i].Position, testNorm, tempVertices[i].TextureCoordinate);
            }
            
            // Physics Points
            List<Physics.Point> tempList = new List<Physics.Point>();
			int num_points = 0;
            foreach (VertexPositionNormalTexture v in vertices)
            {
                tempList.Add(new Physics.Point(center + new Vector3(v.Position.X, v.Position.Y, v.Position.Z)));
				num_points++;
            }
			int check = vertices.Length;

            // Physics Springs
			foreach (Physics.Point t in tempList)
			{
				foreach (Physics.Point p in points)
				{
					float d = Vector3.Distance(t.getCurrentPosition(), p.getCurrentPosition());
                    if (d > 0 && d < 0.5f)
                    {
                        springs.Add(new Physics.Spring(t, p, Vector3.Distance(t.getCurrentPosition(), p.getCurrentPosition()), springVal * 100));
                    }
				}
				points.Add(t);
			}

            // IndexBuffer
			if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
			{
				indices = new int[vertices.Length];
				short[] temp = new short[vertices.Length];
				mesh.IndexBuffer.GetData<short>(temp);
				for (int i = 0; i < vertices.Length; i++)
					indices[i] = temp[i];
			}
			else
			{
				indices = new int[vertices.Length];
				mesh.IndexBuffer.GetData<int>(indices);
			}
			myIndexBuffer = mesh.IndexBuffer;
            // Collidables
            for (int i = 0; i < part.PrimitiveCount; i++)
            {
                //collidables.Add(new Tri(points[indices[i]], points[indices[i + 1]], points[indices[i + 2]], Color.White));
            }


            
            myStreamOffset = 0;
            myBaseVertex = 0;
            myNumVertices = vertices.Length;
            myStartIndex = 0;
            myPrimitiveCount = part.PrimitiveCount;

		}

        private void updateVertices()
        {
            //update points
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = points[i].Position;
            }
        }

        public VertexBuffer getVertexBuffer()
        {
            updateVertices();
            myVertexBuffer.SetData<VertexPositionNormalTexture>(vertices);


            //vertices[0].Position.X -= 0.01f;
            //myVertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
            return myVertexBuffer;
		}

        public IndexBuffer getIndexBuffer()
        {
            return myIndexBuffer;
        }

		public void setGraphicsDevice(GraphicsDevice device)
		{
            theDevice = device;
            myVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.SizeInBytes * vertices.Length, BufferUsage.None);
			myVertexDeclaration = new VertexDeclaration(device, VertexPositionNormalTexture.VertexElements);
		}

		public int getVertexStride()
		{
			return myVertexStride;
		}

		public void DrawMe()
        {

            theDevice.VertexDeclaration = myVertexDeclaration;
            theDevice.Indices = myIndexBuffer;
            theDevice.Vertices[0].SetSource(getVertexBuffer(), myStreamOffset, myVertexStride);
            theDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,0, 0,vertices.Length,0, myPrimitiveCount);
            theDevice.Indices = null;
            theDevice.Vertices[0].SetSource(null, 0, 0);
		}

		
        public IEnumerable<Physics.Point> getPoints()
        {
            return points;
        }

        public IEnumerable<Physics.Collidable> getCollidables()
        {
            // Disabled collision planes for softcubes until I can figure out what's wrong.

            //List<Physics.Collidable> temp = new List<Physics.Collidable>();
            //foreach ( Tri t in collidables ) 
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

			// really really rough appoximation for a blob

			
			/*
			for (int i = 0; i < vertices.Length - 2; i++)
			{

				totalVolume += getFaceVolume(vertices[i].Position, vertices[i + 1].Position, vertices[i + 2].Position);

			}
			 */


			min = getCenter();
			max = getCenter();


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

            Console.WriteLine("Original Volume Estimate: " + totalVolume);
            return totalVolume;
        }

        public float getNewVolume()
        {
            float totalVolume = 0;

            Vector3 center = getCenter();
            int[] test = indices;
            int numVertices = 0;

            for (int i = 0; i < indices.Length - 3; i=i+3)
            {

                totalVolume += getFaceVolumeTest(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position);
                numVertices++;

            }

            float test2 = (4f / 3f) * ((float)Math.PI);
            Console.WriteLine("New Volume Estimate: " + totalVolume);
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

        private float getFaceVolumeTest(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            Vector3 a = point2 - point1;
            Vector3 b = point3 - point1;
            Vector3 c = Vector3.Cross(a, b);

            float area = 0.5f * c.Length();

            Plane facePlane = new Plane(point1,point2,point3);
            //Plane centerPlane = new Plane(facePlane.Normal,facePlane.DotNormal(getCenter()));

            float distance = Vector3.Dot(facePlane.Normal, Vector3.Subtract(getCenter(),facePlane.Normal * facePlane.D));
            Vector3 closestPoint = Vector3.Subtract(getCenter(), Vector3.Multiply(facePlane.Normal, distance));
            float height = Vector3.Distance(getCenter(), closestPoint);

            float volume = height * area * (1f / 3f);
            if (float.IsNaN(volume))
                throw new Exception("Why?");

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
