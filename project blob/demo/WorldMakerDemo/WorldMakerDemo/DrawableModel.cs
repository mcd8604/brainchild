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

namespace WorldMakerDemo
{
    public class DrawableModel : Drawable
    {

        Texture2D m_PointTexture;
        public Texture2D PointTexture
        {
            get
            {
                return m_PointTexture;
            }
            set
            {
                m_PointTexture = value;
            }
        }

        String m_Name;
        public String Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
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

        bool m_ShowVertices = false;
        public bool ShowVertices
        {
            get
            {
                return m_ShowVertices;
            }
            set
            {
                m_ShowVertices = value;
            }
        }

        public VertexBuffer getVertexBuffer()
        {
            return null;
        }

        public DrawableModel(String p_Name)
        {
            m_Name = p_Name;

            TranslationPriority = 2;
            RotationPriority = 1;
            ScalePriority = 0;

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

                if (this.ShowVertices)
                {
                    Texture2D temp = (Texture2D)m_GraphicsDevice.Textures[0];
                    m_GraphicsDevice.Textures[0] = PointTexture;
                    m_GraphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, part.NumVertices);
                    m_GraphicsDevice.Textures[0] = temp;
                }
            }
        }
    }
}
