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
    //This class holds a list of VectorLists to be drawn to the screen
    public class Display
    {
        SortedList<TextureInfo, List<Drawable>> drawable_List_Level;
        SortedList<TextureInfo, List<Drawable>> drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>();
        VertexDeclaration m_VertexDeclaration;
        Texture2D m_BlackTexture;

        public Texture2D BlackTexture
        {
            get { return m_BlackTexture; }
            set { m_BlackTexture = value; }
        }

        bool m_ShowAxis = false;
        public bool ShowAxis
        {
            get
            {
                return m_ShowAxis;
            }
            set
            {
                m_ShowAxis = value;
            }
        }

        String m_CurrentlySelected = "";
        public String CurrentlySelected
        {
            get
            {
                return m_CurrentlySelected;
            }
            set
            {
                m_CurrentlySelected = value;
            }
        }

        public SortedList<TextureInfo, List<Drawable>> DrawnList
        {
            get
            {
                return drawable_List_Drawn;
            }
            set
            {
                drawable_List_Drawn = value;
            }
        }

        Effect m_Effect;
        public Effect CurrentEffect
        {
            get
            {
                return m_Effect;
            }
            set
            {
                m_Effect = value;
            }
        }

        String m_TextureParameterName = "";
        public String TextureParameterName
        {
            get
            {
                return m_TextureParameterName;
            }
            set
            {
                m_TextureParameterName = value;
            }
        }

        String m_WorldParameterName = "";
        public String WorldParameterName
        {
            get
            {
                return m_WorldParameterName;
            }
            set
            {
                m_WorldParameterName = value;
            }
        }

        Matrix m_WorldMatrix;
        public Matrix WorldMatrix
        {
            get
            {
                return m_WorldMatrix;
            }
            set
            {
                m_WorldMatrix = value;
            }
        }

        String m_TechniqueName = null;
        public String TechniqueName
        {
            get
            {
                return m_TechniqueName;
            }
            set
            {
                m_TechniqueName = value;
            }
        }

        public Display(Matrix p_World, Matrix p_View, Matrix p_Projection, VertexDeclaration p_VertexDeclaration)
        {
            m_VertexDeclaration = p_VertexDeclaration;
            m_VertexDeclaration.GraphicsDevice.RenderState.CullMode =
                CullMode.CullCounterClockwiseFace;

            m_Effect = new BasicEffect(m_VertexDeclaration.GraphicsDevice, null);
            ((BasicEffect)m_Effect).Alpha = 1.0f;
            ((BasicEffect)m_Effect).DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            ((BasicEffect)m_Effect).SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            ((BasicEffect)m_Effect).SpecularPower = 5.0f;
            ((BasicEffect)m_Effect).AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            ((BasicEffect)m_Effect).DirectionalLight0.Enabled = true;
            ((BasicEffect)m_Effect).DirectionalLight0.DiffuseColor = Vector3.One;
            ((BasicEffect)m_Effect).DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            ((BasicEffect)m_Effect).DirectionalLight0.SpecularColor = Vector3.One;

            ((BasicEffect)m_Effect).LightingEnabled = true;
            ((BasicEffect)m_Effect).TextureEnabled = true;

            drawable_List_Level = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());
            drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());

            ((BasicEffect)m_Effect).World = p_World;
            m_WorldMatrix = p_World;

            ((BasicEffect)m_Effect).View = p_View;
            ((BasicEffect)m_Effect).Projection = p_Projection;
        }

        public Display(Matrix p_World, VertexDeclaration p_VertexDeclaration, Effect p_Effect, String p_WorldParameterName, 
            String p_TextureParameterName, String p_TechniqueName)
        {
            m_VertexDeclaration = p_VertexDeclaration;
            m_VertexDeclaration.GraphicsDevice.RenderState.CullMode =
                CullMode.CullCounterClockwiseFace;

            drawable_List_Level = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());
            drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());

            m_Effect = p_Effect;
            m_WorldMatrix = p_World;
            m_WorldParameterName = p_WorldParameterName;
            m_TextureParameterName = p_TextureParameterName;

            m_TechniqueName = p_TechniqueName;

        }

        public void Draw()
        {
            if(m_TechniqueName != null)
                m_Effect.CurrentTechnique = m_Effect.Techniques[m_TechniqueName];

            int currentTextureNumber = drawable_List_Drawn.Keys[0].SortNumber;
            //m_GraphicsDevice.Textures[0] = vertexBuffer_List_Drawn.Keys[0].TextureObject;
            if (m_Effect is BasicEffect)
            {
                ((BasicEffect)m_Effect).Texture = drawable_List_Drawn.Keys[0].TextureObject;
            }
            else
            {
                m_VertexDeclaration.GraphicsDevice.Textures[0] = drawable_List_Drawn.Keys[0].TextureObject;
                if(m_TextureParameterName != "NONE")
                    m_Effect.Parameters[m_TextureParameterName].SetValue(drawable_List_Drawn.Keys[0].TextureObject);
            }

            m_VertexDeclaration.GraphicsDevice.VertexDeclaration = m_VertexDeclaration;

            //m_VertexDeclaration.GraphicsDevice.RenderState.CullMode = CullMode.None;
            //m_VertexDeclaration.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            //m_VertexDeclaration.GraphicsDevice.RenderState.DepthBufferEnable = false;
            if (ShowAxis)
            {
                VertexPositionNormalTexture[] axisVertices = new VertexPositionNormalTexture[6];

                axisVertices[0] = new VertexPositionNormalTexture(new Vector3(-2, 0, 0), Vector3.Zero, Vector2.Zero);
                axisVertices[1] = new VertexPositionNormalTexture(new Vector3(2, 0, 0), Vector3.Zero, new Vector2(0, 1));
                axisVertices[2] = new VertexPositionNormalTexture(new Vector3(0, -2, 0), Vector3.Zero, Vector2.Zero);
                axisVertices[3] = new VertexPositionNormalTexture(new Vector3(0, 2, 0), Vector3.Zero, new Vector2(0, 1));
                axisVertices[4] = new VertexPositionNormalTexture(new Vector3(0, 0, -2), Vector3.Zero, Vector2.Zero);
                axisVertices[5] = new VertexPositionNormalTexture(new Vector3(0, 0, 2), Vector3.Zero, new Vector2(0, 1));

                VertexBuffer vb = new VertexBuffer(m_VertexDeclaration.GraphicsDevice, VertexPositionNormalTexture.SizeInBytes * 6, BufferUsage.None);
                vb.SetData<VertexPositionNormalTexture>(axisVertices);

                //VertexStream tempBuffer = m_VertexDeclaration.GraphicsDevice.Vertices[0];
                m_VertexDeclaration.GraphicsDevice.Vertices[0].SetSource(vb, 0, VertexPositionNormalTexture.SizeInBytes);
                Texture temp = m_VertexDeclaration.GraphicsDevice.Textures[0];
                m_VertexDeclaration.GraphicsDevice.Textures[0] = BlackTexture;
                m_Effect.Begin();

                foreach (EffectPass pass in m_Effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    m_VertexDeclaration.GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 6);
                    pass.End();
                }
                m_Effect.End();
                m_VertexDeclaration.GraphicsDevice.Textures[0] = temp;
                //m_VertexDeclaration.GraphicsDevice.Vertices[0].SetSource(tempBuffer.VertexBuffer,0,tempBuffer.VertexStride);
            }

            foreach (TextureInfo ti in drawable_List_Drawn.Keys)
            {
                if (ti.SortNumber != currentTextureNumber)
                {
                    if(m_Effect is BasicEffect)
                    {
                        ((BasicEffect)m_Effect).Texture = ti.TextureObject;
                    }
                    else
                    {
                        m_VertexDeclaration.GraphicsDevice.Textures[0] = ti.TextureObject;
                        if (m_TextureParameterName != "NONE")
                            m_Effect.Parameters[m_TextureParameterName].SetValue(ti.TextureObject);
                    }
                }

                foreach (Drawable d in drawable_List_Drawn[ti])
                {
                    if (d is DrawableModel)
                    {
                        DrawModel(m_WorldMatrix, (DrawableModel)d);
        
                    }
                    else
                    {
                        DrawPrimitives(d);
                    }
                }
            }
        }

        public void DrawPrimitives(Drawable d)
        {
            m_VertexDeclaration.GraphicsDevice.Vertices[0].SetSource(d.getVertexBuffer(), 0, d.getVertexStride());

            m_Effect.Begin();
            foreach (EffectPass pass in m_Effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                d.DrawMe();
                pass.End();
            }
            m_Effect.End();
        }

        public void DrawModel(Matrix p_CurrentWorld, DrawableModel d)
        {
            Stack<Matrix> drawStack = new Stack<Matrix>();
            Matrix currentWorld = p_CurrentWorld;


            if (d.Name == CurrentlySelected)
            {
                d.ShowVertices = true;
                d.PointTexture = BlackTexture;
            }
            else
            {
                d.ShowVertices = false;
            }

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (d.PriorityArray[i] == j)
                    {
                        switch (i)
                        {
                            case 0:
                                if(d.Position != null)
                                    drawStack.Push(d.Position);
                                break;
                            case 1:
                                if(d.Rotation != null)
                                    drawStack.Push(d.Rotation);
                                break;
                            case 2:
                                if(d.Scale != null)
                                    drawStack.Push(d.Scale);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (m_Effect is BasicEffect)
                ((BasicEffect)m_Effect).World = p_CurrentWorld;
            else
                m_Effect.Parameters[m_WorldParameterName].SetValue(p_CurrentWorld);

            while (drawStack.Count > 0)
            {
                if (m_Effect is BasicEffect)
                    ((BasicEffect)m_Effect).World = Matrix.Multiply(drawStack.Pop(), ((BasicEffect)m_Effect).World);
                else
                    m_Effect.Parameters[m_WorldParameterName].SetValue(Matrix.Multiply(drawStack.Pop(), m_Effect.Parameters[m_WorldParameterName].GetValueMatrix()));
            }
            
            foreach (ModelMesh mesh in d.ModelObject.Meshes)
            {
                m_VertexDeclaration.GraphicsDevice.Indices = mesh.IndexBuffer;
                m_Effect.Begin();

                // Loop through each pass in the effect like we do elsewhere
                foreach (EffectPass pass in m_Effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    d.DrawMe(mesh);
                    pass.End();
                }
                m_Effect.End();
            }
            if(m_Effect is BasicEffect)
                ((BasicEffect)m_Effect).World = currentWorld;
            else
                m_Effect.Parameters[m_WorldParameterName].SetValue(currentWorld);
        }
    }
}
