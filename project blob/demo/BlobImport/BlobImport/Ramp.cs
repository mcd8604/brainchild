using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlobImport
{
    class Ramp : Drawable
    {
        public readonly List<Physics.Collidable> collidables = new List<Physics.Collidable>();

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

        Model m_Model;

        private void initRamp(Model p_Model)
        {
            m_Model = p_Model;

            ModelMesh mesh = p_Model.Meshes[0];
            ModelMeshPart part = mesh.MeshParts[0];

            myVertexStride = VertexPositionNormalTexture.SizeInBytes;


            Type vertexType;


          

            // VertexBuffer
            VertexPositionNormalTexture[] tempVertices = new VertexPositionNormalTexture[part.NumVertices];
            mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(tempVertices);

            vertices = tempVertices;
            //for (int i = 0; i < tempVertices.Length; i++)
            //{
                //Vector3 testNorm = Vector3.Normalize(Vector3.Subtract(tempVertices[i].Position, center));
                //vertices[i] = new VertexPositionNormalTexture(tempVertices[i].Position, testNorm, tempVertices[i].TextureCoordinate);
            //}

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
            // Collidables
            for (int i = 0; i < indices.Length; i=i+3)
            {
                Vector3 p0 = vertices[indices[i]].Position;
                Vector3 p1 = vertices[indices[i + 1]].Position;
                Vector3 p2 = vertices[indices[i + 2]].Position;

                collidables.Add(new StaticTri(p0,p2,p1,Color.White));
            }

            myVertexBuffer = mesh.VertexBuffer;
            myStreamOffset = 0;
            myBaseVertex = 0;
            myNumVertices = vertices.Length;
            myStartIndex = 0;
            myPrimitiveCount = part.PrimitiveCount;

            //VertexPositionNormalTexture[] tempBuff = new VertexPositionNormalTexture[part.NumVertices];
            //myVertexBuffer.GetData<VertexPositionNormalTexture>(tempBuff);

            //for (int i = 0; i < tempBuff.Length; i++)
            //{
            //    tempBuff[i].Normal = Vector3.Normalize(Vector3.Negate(tempBuff[i].Normal));
            //}
            //myVertexBuffer.SetData<VertexPositionNormalTexture>(tempBuff);
        }

        public Ramp(Model p_Model)
        {
            initRamp(p_Model);
        }

        #region Drawable Members

        public VertexBuffer getVertexBuffer()
        {
            return myVertexBuffer;   
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
            

            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                theDevice.Indices = mesh.IndexBuffer;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    theDevice.VertexDeclaration = part.VertexDeclaration;
                    theDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                    theDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    theDevice.Indices = null;
                    theDevice.Vertices[0].SetSource(null, 0, 0);
                }
            }
        }

        public List<Physics.Collidable> getCollidables()
        {
            return collidables;
        }

        #endregion
    }
}
