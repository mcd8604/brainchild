using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine;
using Physics2;

namespace Project_blob
{

    //--------------------------------------------------------------------------------------
    // Custom types
    //--------------------------------------------------------------------------------------

    // Vertex format for blob billboards
    struct POINTVERTEX  
    {
        public Vector3 pos;
        public float       size;
        public Vector3 color;
    }; 

    // Vertex format for screen space work
    struct SCREENVERTEX     
    {
        public Vector4 pos;
        public Vector2 tCurr;
        public Vector2 tBack;
        public float       fSize;
        public Vector3 vColor;
    };

	public class Blob : BodyPressure, Drawable
	{

        public Texture2D text = null;
        public Texture2D DisplacementText = null;
        int NUM_Blobs;

        POINTVERTEX[] g_BlobPoints;

		public static float springVal = 62.5f;

		private GraphicsDevice theDevice;

		VertexPositionNormalTexture[] vertices;
		int[] indices;
		private List<int> pointsToUpdate = new List<int>(); //index is the point array index, value is the vertice to update

		private VertexDeclaration myVertexDeclaration;
		private VertexBuffer myVertexBuffer;
		private IndexBuffer myIndexBuffer;
		private int myVertexStride;
		private int myStreamOffset;
		//private int myBaseVertex;
		private int myNumVertices;
		//private int myStartIndex;
		private int myPrimitiveCount;

		private Vector3 min;
		private Vector3 max;

		public void setSpringLength(float delta)
		{

			foreach (Spring s in springs)
			{
				//s.MaximumLengthBeforeExtension += delta;
				//s.MinimumLengthBeforeCompression += delta;
                s.LengthOffset += delta;
			}

		}

		public Blob(Model aModel)
		{
			initBlob(aModel, aModel.Meshes[0].BoundingSphere.Center);
			initialize();
		}

		public Blob(Model aModel, Vector3 startPos)
		{
			initBlob(aModel, aModel.Meshes[0].BoundingSphere.Center + startPos);
			initialize();
		}

		public Collidable bottom;

		private void initBlob(Model blobModel, Vector3 center)
		{
			ModelMesh mesh = blobModel.Meshes[0];
			ModelMeshPart part = mesh.MeshParts[0];

			myVertexStride = VertexPositionNormalTexture.SizeInBytes;


			//Type vertexType;


			// VertexBuffer
			VertexPositionColorTexture[] tempVertices = new VertexPositionColorTexture[part.NumVertices];
			mesh.VertexBuffer.GetData<VertexPositionColorTexture>(tempVertices);
           
			vertices = new VertexPositionNormalTexture[tempVertices.Length];
			for(int i = 0; i < tempVertices.Length; i++) 
			{
				Vector3 testNorm = Vector3.Normalize(Vector3.Subtract(tempVertices[i].Position,center));
				vertices[i] = new VertexPositionNormalTexture(tempVertices[i].Position, testNorm, tempVertices[i].TextureCoordinate);
			}

            /*vertices = new VertexPositionNormalTexture[part.NumVertices];
            mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);*/

            //Hashtable pointTable = new Hashtable(new Physics.PointComparater());

			// IndexBuffer
			if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
			{
				indices = new int[(mesh.IndexBuffer.SizeInBytes) * 8 / 16];
				short[] temp = new short[(mesh.IndexBuffer.SizeInBytes) * 8 / 16];
				mesh.IndexBuffer.GetData<short>(temp);
				for (int i = 0; i < temp.Length; i++)
					indices[i] = temp[i];
			}
			else
			{
				indices = new int[(mesh.IndexBuffer.SizeInBytes) * 8 / 32];
				mesh.IndexBuffer.GetData<int>(indices);
			}
			myIndexBuffer = mesh.IndexBuffer;

			// Physics Points
			List<PhysicsPoint> tempList = new List<PhysicsPoint>();

			foreach (VertexPositionNormalTexture v in vertices)
			{
				tempList.Add(new PhysicsPoint(center + v.Position, this));
			}

            NUM_Blobs = tempList.Count;

            g_BlobPoints = new POINTVERTEX[NUM_Blobs];
            // Set initial blob states
            for (int i = 0; i < NUM_Blobs; i++)
            {
                g_BlobPoints[i].pos = new Vector3(0.0f, 0.0f, 0.0f);
                g_BlobPoints[i].size = 1.0f;
                g_BlobPoints[i].color = new Vector3(0,0.3f,0);
            }

			/*
			int num_points = 0;
			int repeated_points = 0;
			int iter = 0;
			for(int i = 0; i < vertices.Length; i++)
			{
				Physics.Point temp = new Physics.Point(center + new Vector3(vertices[i].Position.X, vertices[i].Position.Y, vertices[i].Position.Z));
				if (pointTable.ContainsKey(temp))
				{
					((List<int>)pointTable[temp]).Add(i);
					repeated_points++;
				}
				else
				{
					List<int> tableList = new List<int>();
					tableList.Add(i);
					pointTable[temp] = tableList;
					tempList.Add(temp);
					pointsToUpdate.Add(iter);
					num_points++;
				}
				iter++;
			}
            
			//int check = vertices.Length;

            

			//change indices
			for (int i = 0; i < indices.Length; i++)
			{
				Physics.Point tempPoint = new Physics.Point(center + vertices[indices[i]].Position);
				if (((List<int>)pointTable[tempPoint]).Count > 1)
				{
					indices[i] = ((List<int>)pointTable[tempPoint])[0];
				}
			}
			*/

			//weed out duplicate vertices and alert indices of the change
			//VertexPositionNormalTexture[] tempArray = new VertexPositionNormalTexture[pointTable.Keys.Count];
			//int it = 0;
			//foreach (Physics.Point p in pointTable.Keys)
			//{
			//    tempArray[it] = vertices[((List<int>)pointTable[p])[0]];
			//    for (int i = 0; i < indices.Length; i++)
			//    {
			//        if (indices[i] == ((List<int>)pointTable[p])[0])
			//        {
			//            indices[i] = it;
			//            if (i == 118)
			//                throw new Exception("Stop");
			//        }
			//    }
			//    it++;
			//}
			//vertices = tempArray;
			// Physics Springs
			foreach (PhysicsPoint t in tempList)
			{
				foreach (PhysicsPoint p in points)
				{
					float d = Vector3.Distance(t.ExternalPosition, p.ExternalPosition);
					if (d > 0)
					{
						springs.Add(new Spring(t, p, Vector3.Distance(t.ExternalPosition, p.ExternalPosition), springVal * 100));
					}
					else
					{
						Console.WriteLine("Warning! Duplicate Point!");
					}
				}
				points.Add(t);
			}


			// Collidables
			//for (int i = 0; i < part.PrimitiveCount; i++)
			//{
			//    //collidables.Add(new Tri(points[indices[i]], points[indices[i + 1]], points[indices[i + 2]], Color.White));
			//}



			myStreamOffset = 0;
			//myBaseVertex = 0;
			myNumVertices = vertices.Length;
			//myStartIndex = 0;
			myPrimitiveCount = part.PrimitiveCount;

		}

		private void updateVertices()
		{
			//update points
			for (int i = 0; i < vertices.Length/*pointsToUpdate.Count*/; i++)
			{
				vertices[i].Position = points[i].ExternalPosition;
				vertices[i].Normal = Vector3.Normalize(Vector3.Subtract(vertices[i].Position, getCenter()));
                g_BlobPoints[i].pos = vertices[i].Position;

				//vertices[pointsToUpdate[i]].Position = points[i].CurrentPosition;
				//vertices[pointsToUpdate[i]].Normal = Vector3.Normalize(Vector3.Subtract(vertices[i].Position, getCenter() ));
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
			//myIndexBuffer = new IndexBuffer(theDevice, sizeof(int) * indices.Length, BufferUsage.None, IndexElementSize.SixteenBits);
			//myIndexBuffer.SetData<int>(indices);
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
			theDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, myPrimitiveCount);
			theDevice.Indices = null;
			theDevice.Vertices[0].SetSource(null, 0, 0);
		}

        //public override IEnumerable<Collidable> getCollidables()
        //{
        //    // Disabled collision planes for softcubes until I can figure out what's wrong.

        //    //List<Physics.Collidable> temp = new List<Physics.Collidable>();
        //    //foreach ( Tri t in collidables ) 
        //    //    temp.Add( t as Physics.Collidable );
        //    //}
        //    //return temp;

        //    return new List<Collidable>();
        //}

        //public IEnumerable<Drawable> getDrawables()
        //{
        //    List<Drawable> temp = new List<Drawable>();
        //    foreach (Tri t in collidables)
        //    {
        //        temp.Add(t as Drawable);
        //    }
        //    return temp;
        //}

		public float getOldVolume()
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

			//Console.WriteLine("Original Volume Estimate: " + totalVolume);
			return totalVolume;
		}

		private float idealVolume = 10;
		public override float getIdealVolume()
		{
			return idealVolume;
		}
		public override void setIdealVolume(float volume)
		{
			idealVolume = volume;
		}

		public override float getPotentialVolume()
		{
			float totalVolume = 0;

			Vector3 center = getCenter();

			for (int i = 0; i < indices.Length - 3; i = i + 3)
			{
				/*
				Vector3 p1 = new Vector3();
				Vector3 p2 = new Vector3();
				Vector3 p3 = new Vector3();
				for (int j = 0; j < pointsToUpdate.Count; j++)
				{

					if (pointsToUpdate[j] == indices[i])
						p1 = points[j].potentialPosition;
					if (pointsToUpdate[j] == indices[i+1])
						p2 = points[j].potentialPosition;
					if (pointsToUpdate[j] == indices[i + 2])
						p3 = points[j].potentialPosition;
				}
				*/
				totalVolume += getPotentialFaceVolumeTest(points[indices[i]].PotentialPosition, points[indices[i + 1]].PotentialPosition, points[indices[i + 2]].PotentialPosition);
				//totalVolume += getFaceVolumeTest(p1, p2, p3);
			}

			//Console.WriteLine("Next Volume Estimate: " + totalVolume);
			return totalVolume;
		}

		public override float getVolume()
		{
			float totalVolume = 0;

			Vector3 center = getCenter();

			for (int i = 0; i < indices.Length - 3; i = i + 3)
			{
				/*
				Vector3 p1 = new Vector3();
				Vector3 p2 = new Vector3();
				Vector3 p3 = new Vector3();
				for (int j = 0; j < pointsToUpdate.Count; j++)
				{

					if (pointsToUpdate[j] == indices[i])
						p1 = points[j].CurrentPosition;
					if (pointsToUpdate[j] == indices[i + 1])
						p2 = points[j].CurrentPosition;
					if (pointsToUpdate[j] == indices[i + 2])
						p3 = points[j].CurrentPosition;
				}
				totalVolume += getFaceVolumeTest(p1, p2, p3);
				*/
				totalVolume += getFaceVolumeTest(points[indices[i]].ExternalPosition, points[indices[i + 1]].ExternalPosition, points[indices[i + 2]].ExternalPosition);
			}

			//Console.WriteLine("Volume Estimate: " + totalVolume);
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

			float area = .5f * c.Length();

			Plane facePlane = new Plane(point1, point2, point3);
			//Plane centerPlane = new Plane(facePlane.Normal,facePlane.DotNormal(getCenter()));

			// float distance = Vector3.Dot(facePlane.Normal, Vector3.Subtract(getCenter(),facePlane.Normal * facePlane.D));
			//Vector3 closestPoint = Vector3.Subtract(getCenter(), Vector3.Multiply(facePlane.Normal, distance));
			//float height = Vector3.Distance(getCenter(), closestPoint);

			Vector3 center = getCenter();
			//negation because we are drawing the planes upside down
			float distanceToCenter = Vector3.Dot(Vector3.Negate(Vector3.Normalize(facePlane.Normal)), center);
			float height = MathHelper.Distance(distanceToCenter, facePlane.D);

			float volume = height * area * (1f / 3f);
			if (float.IsNaN(volume))
				throw new Exception("Not Good");

			return volume;
		}

        private float getPotentialFaceVolumeTest(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            Vector3 a = point2 - point1;
            Vector3 b = point3 - point1;
            Vector3 c = Vector3.Cross(a, b);

            float area = .5f * c.Length();

            Plane facePlane = new Plane(point1, point2, point3);
            //Plane centerPlane = new Plane(facePlane.Normal,facePlane.DotNormal(getCenter()));

            // float distance = Vector3.Dot(facePlane.Normal, Vector3.Subtract(getCenter(),facePlane.Normal * facePlane.D));
            //Vector3 closestPoint = Vector3.Subtract(getCenter(), Vector3.Multiply(facePlane.Normal, distance));
            //float height = Vector3.Distance(getCenter(), closestPoint);

            Vector3 center = getPotentialCenter();
            //negation because we are drawing the planes upside down
            float distanceToCenter = Vector3.Dot(Vector3.Negate(Vector3.Normalize(facePlane.Normal)), center);
            float height = MathHelper.Distance(distanceToCenter, facePlane.D);

            float volume = height * area * (1f / 3f);
            if (float.IsNaN(volume))
                throw new Exception("Not Good");

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

        //-----------------------------------------------------------------------------
        // Fill the vertex buffer for the blob objects
        //-----------------------------------------------------------------------------
        void FillBlobVB(Matrix pmatWorldView, BasicCamera g_Camera)
        {

            SCREENVERTEX[] pBlobVertex = new SCREENVERTEX[6 * NUM_Blobs];
          
            //V_RETURN( g_pBlobVB->Lock( 0, 0, (void**)&pBlobVertex, D3DLOCK_DISCARD ) );
            
            POINTVERTEX [] blobPos = new POINTVERTEX[ NUM_Blobs ];
            
            for( int i=0; i < NUM_Blobs; ++i )
            {
                // Transform point to camera space
                Vector3 blobPosCamera;
                blobPosCamera = Vector3.Transform(g_BlobPoints[i].pos, pmatWorldView );
                
                blobPos[i] = g_BlobPoints[i];
                blobPos[i].pos.X = blobPosCamera.X;
                blobPos[i].pos.Y = blobPosCamera.Y;
                blobPos[i].pos.Z = blobPosCamera.Z;
            }

            int posCount=0;
            for( int i=0; i < NUM_Blobs; ++i )
            {
                Vector4 BlobscreenPos;

                // For calculating billboarding
                Vector4 billOfs = new Vector4(blobPos[i].size,blobPos[i].size,blobPos[i].pos.Z,1);
                Vector4 billOfsScreen;

                // Transform to screenspace
                Matrix pmatProjection = g_Camera.Projection;
                BlobscreenPos = Vector4.Transform(blobPos[i].pos, pmatProjection);
                billOfsScreen = Vector4.Transform(billOfs, pmatProjection);

                // Project
                BlobscreenPos = Vector4.Multiply(BlobscreenPos, 1.0f / BlobscreenPos.W);
                billOfsScreen = Vector4.Multiply(billOfsScreen, 1.0f / billOfsScreen.W);

                Vector2 [] vTexCoords = 
                {
                    new Vector2(0.0f,0.0f),
                    new Vector2(1.0f,0.0f),
                    new Vector2(0.0f,1.0f),
                    new Vector2(0.0f,1.0f),
                    new Vector2(1.0f,0.0f),
                    new Vector2(1.0f,1.0f),
                };

                Vector4 [] vPosOffset =
                {
                    new Vector4(-billOfsScreen.X,-billOfsScreen.Y,0.0f,0.0f),
                    new Vector4( billOfsScreen.X,-billOfsScreen.Y,0.0f,0.0f),
                    new Vector4(-billOfsScreen.X, billOfsScreen.Y,0.0f,0.0f),
                    new Vector4( billOfsScreen.X,-billOfsScreen.Y,0.0f,0.0f),
                    new Vector4( billOfsScreen.X, billOfsScreen.Y,0.0f,0.0f),
                };
                
                ResolveTexture2D backBuffer = new ResolveTexture2D(theDevice,theDevice.Viewport.Width, theDevice.Viewport.Height,0,SurfaceFormat.Color);
                theDevice.ResolveBackBuffer(backBuffer);
                //D3DSURFACE_DESC pBackBufferSurfaceDesc = DXUTGetD3D9BackBufferSurfaceDesc();

                // Set constants across quad
                for( int j=0; j < 6 ;++j )
                {
                    // Scale to pixels
                    pBlobVertex[posCount].pos = Vector4.Add(BlobscreenPos, vPosOffset[j] );  
                    
                    pBlobVertex[posCount].pos.X *= backBuffer.Width;             
                    pBlobVertex[posCount].pos.Y *= backBuffer.Height;
                    pBlobVertex[posCount].pos.X += 0.5f * backBuffer.Width; 
                    pBlobVertex[posCount].pos.Y += 0.5f * backBuffer.Height;
                    
                    pBlobVertex[posCount].tCurr = vTexCoords[j];
                    pBlobVertex[posCount].tBack = new Vector2((0.5f+pBlobVertex[posCount].pos.X)*(1.0f/backBuffer.Width),
                                                           (0.5f+pBlobVertex[posCount].pos.Y)*(1.0f/backBuffer.Height));
                    pBlobVertex[posCount].fSize = blobPos[i].size;
                    pBlobVertex[posCount].vColor = blobPos[i].color;

                    posCount++;
                }
            }
        }      

	    #region Drawable Members


		public TextureInfo GetTextureKey()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public BoundingBox GetBoundingBox()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public BoundingSphere GetBoundingSphere()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
