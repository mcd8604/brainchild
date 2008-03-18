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
                // Make sure we use the texture for the current part also
                m_GraphicsDevice.Textures[0] = ((BasicEffect)part.Effect).Texture;
                // Finally draw the actual triangles on the screen
                m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
            }
        }
    }
}
