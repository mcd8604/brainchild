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
        short[] indices;

        private VertexDeclaration myVertexDeclaration;
		private VertexBuffer myVertexBuffer;
        private IndexBuffer myIndexBuffer;
        private int myVertexStride;
        private int myStreamOffset;
        private int myBaseVertex;
        private int myNumVertices;
        private int myStartIndex;
        private int myPrimitiveCount;
        
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

            myVertexStride = part.VertexStride;


            Type vertexType;


          /*  switch(myVertexStride) {
                case 16:
                    vertexType = typeof(VertexPositionColor);


            }
            */
            // VertexBuffer
            VertexPositionColorTexture[] tempVertices = new VertexPositionColorTexture[blobModel.Meshes[0].VertexBuffer.SizeInBytes / myVertexStride];
            mesh.VertexBuffer.GetData<VertexPositionColorTexture>(tempVertices);
           
            vertices = new VertexPositionNormalTexture[tempVertices.Length];
            for(int i = 0; i < tempVertices.Length; i++) 
            {
                vertices[i] = new VertexPositionNormalTexture(tempVertices[i].Position, Vector3.Up, tempVertices[i].TextureCoordinate);
            }
            
            // Physics Points
            List<Physics.Point> tempList = new List<Physics.Point>();
            foreach (VertexPositionNormalTexture v in vertices)
            {
                tempList.Add(new Physics.Point(center + new Vector3(v.Position.X, v.Position.Y, v.Position.Z)));
            }

            // Physics Springs
			foreach (Physics.Point t in tempList)
			{
				foreach (Physics.Point p in points)
				{
                    if (Vector3.Distance(t.getCurrentPosition(), p.getCurrentPosition()) != 0)
                    {
                        springs.Add(new Physics.Spring(t, p, Vector3.Distance(t.getCurrentPosition(), p.getCurrentPosition()), springVal));
                    }
				}
				points.Add(t);
			}

            // IndexBuffer
            int indexSize;
            if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
            {
                indexSize = sizeof(short);
            } else {
                indexSize = sizeof(int);
            }
            int numIndices = mesh.IndexBuffer.SizeInBytes / indexSize;
            indices = new short[numIndices];
            mesh.IndexBuffer.GetData<short>(indices);
            myIndexBuffer = mesh.IndexBuffer;

            // Collidables
            for (int i = 0; i < part.PrimitiveCount; i++)
            {
                //collidables.Add(new Tri(points[indices[i]], points[indices[i + 1]], points[indices[i + 2]], Color.White));
            }

            myVertexDeclaration = part.VertexDeclaration;
            myStreamOffset = part.StreamOffset;
            myBaseVertex = part.BaseVertex;
            myNumVertices = part.NumVertices;
            myStartIndex = part.StartIndex;
            myPrimitiveCount = part.PrimitiveCount;

		}

        private void updateVertices()
        {
            //update points
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].Position.Equals(points[i].Position))
                {
                    throw new Exception("PHYSICS POINT WAS NOT CHANGED IN BLOB");
                }
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
            theDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, myBaseVertex, 0, myNumVertices, myStartIndex, myPrimitiveCount);
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
