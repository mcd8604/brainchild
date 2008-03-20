using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Project_blob
{
    public class DrawableModel : Drawable
    {
        Model m_Model;
        GraphicsDevice m_GraphicsDevice;
        Matrix m_Position, m_Rotation, m_Scale;

        //priority for translation, rotation, and scale
        //index 0 = translation
        //index 1 = rotation
        //index 2 = scale
        int[] m_PriorityArray = new int[3];

        public int[] PriorityArray
        {
            get
            {
                return m_PriorityArray;
            }
            set
            {
                m_PriorityArray = value;
            }
        }
        public int TranslationPriority
        {
            get
            {
                return m_PriorityArray[0];
            }
            set
            {
                m_PriorityArray[0] = value;
            }
        }

        public int RotationPriority
        {
            get
            {
                return m_PriorityArray[1];
            }
            set
            {
                m_PriorityArray[1] = value;
            }
        }

        public int ScalePriority
        {
            get
            {
                return m_PriorityArray[2];
            }
            set
            {
                m_PriorityArray[2] = value;
            }
        }

        public Matrix Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }
        public Matrix Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
            }
        }

        public Matrix Scale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                m_Scale = value;
            }
        }

        public Model ModelObject
        {
            get
            {
                return m_Model;
            }
            set
            {
                m_Model = value;
            }
        }

        public VertexBuffer getVertexBuffer()
        {
            return null;
        }

        public DrawableModel()
        {
            for (int i = 0; i < 3; i++)
                m_PriorityArray[i] = 3;

            m_Position = Matrix.CreateTranslation(Vector3.Zero);
            m_Rotation = Matrix.CreateRotationZ(0);
            m_Scale = Matrix.CreateScale(1);

        }

        public void setGraphicsDevice(GraphicsDevice device)
        {
            m_GraphicsDevice = device;
        }

        public int getVertexStride()
        {
            return VertexPositionNormalTexture.SizeInBytes;
        }

        public void DrawMe(){}

        public void DrawMe(ModelMesh mesh)
        {
            foreach (ModelMeshPart part in mesh.MeshParts)
            {
                // Change the device settings for each part to be rendered
                m_GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
                m_GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                // Finally draw the actual triangles on the screen
                m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
            }
        }
    }
}
