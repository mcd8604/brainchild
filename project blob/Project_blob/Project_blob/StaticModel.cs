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
using System.Runtime.Serialization;

namespace Project_blob
{
    [Serializable]
    public class StaticModel : Drawable 
    {
        private String _modelName;
        
        //[NonSerialized]
        //private List<Physics.Collidable> m_collidables;
        public List<Physics.Collidable> createCollidables(Model m)
        {
            List<Physics.Collidable> collidables = new List<Physics.Collidable>();
            foreach (ModelMesh mesh in m.Meshes)
            {
                // Vertices
                int numVertices = 0;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    numVertices += part.NumVertices;
                }
                VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[numVertices];
                mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

                // Indices
                int[] indices;
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

                // Collidables
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    for (int i = 0; i < part.PrimitiveCount; i++)
                    {
                        collidables.Add(new CollidableTri(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position));
                    }
                } 
            }
            return collidables;
        }

        public String getName()
        {
            return m_Name;
        }

        private String _textureName;
        public String TextureName
        {
            get
            {
                return _textureName;
            }
            set
            {
                _textureName = value;
            }
        }

        private String m_Name;
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

        public String ModelName
        {
            get
            {
                return _modelName;
            }
            set
            {
                _modelName = value;
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

        public StaticModel(String p_Name, String fileName)
        {
            m_Name = p_Name;
            _modelName = fileName;
            TranslationPriority = 2;
            RotationPriority = 1;
            ScalePriority = 0;

            m_Position = Matrix.CreateTranslation(Vector3.Zero);
            m_Rotation = Matrix.CreateRotationZ(0);
            m_Scale = Matrix.CreateScale(1);

        }


        public int getVertexStride()
        {
            return VertexPositionNormalTexture.SizeInBytes;
        }

        public void DrawMe(){}

        public void DrawMe(ModelMesh mesh, GraphicsDevice graphicsDevice)
        {
            foreach (ModelMeshPart part in mesh.MeshParts)
            {
                // Change the device settings for each part to be rendered
                graphicsDevice.VertexDeclaration = part.VertexDeclaration;
                graphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                // Finally draw the actual triangles on the screen
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);

                if (this.ShowVertices)
                {
                    Texture2D temp = (Texture2D)graphicsDevice.Textures[0];
                    graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(TextureName);
                    graphicsDevice.DrawPrimitives(PrimitiveType.PointList, 0, part.NumVertices);
                    graphicsDevice.Textures[0] = temp;
                }
            }
        }

    }
}
