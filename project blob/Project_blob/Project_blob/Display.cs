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
    //This class holds a list of VectorLists to be drawn to the screen
    //This is a framework, nothing in here is complete!!
    class Display
    {
        SortedList<TextureInfo, List<Drawable>> drawable_List_Level;
        SortedList<TextureInfo, List<Drawable>> drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>();
        VertexDeclaration m_VertexDeclaration;
        Effect m_CurrentEffect;

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

        BasicEffect be;
        public BasicEffect TestEffect
        {
            get
            {
                return be;
            }
            set
            {
                be = value;
            }
        }

        public Display(Matrix p_World, Matrix p_View, Matrix p_Projection, EffectPool p_EffectPool, VertexDeclaration p_VertexDeclaration)
        {
            m_VertexDeclaration = p_VertexDeclaration;
            m_VertexDeclaration.GraphicsDevice.RenderState.CullMode =
                CullMode.CullCounterClockwiseFace;

            be = new BasicEffect(m_VertexDeclaration.GraphicsDevice, null);
            be.Alpha = 1.0f;
            be.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            be.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            be.SpecularPower = 5.0f;
            be.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            be.DirectionalLight0.Enabled = true;
            be.DirectionalLight0.DiffuseColor = Vector3.One;
            be.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            be.DirectionalLight0.SpecularColor = Vector3.One;

            be.LightingEnabled = true;
            be.TextureEnabled = true;

            drawable_List_Level = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());
            drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());

            be.World = p_World;
            be.View = p_View;
            be.Projection = p_Projection;
        }

        public void Draw()
        {
            int currentTextureNumber = drawable_List_Drawn.Keys[0].SortNumber;
            //m_GraphicsDevice.Textures[0] = vertexBuffer_List_Drawn.Keys[0].TextureObject;
            be.Texture = drawable_List_Drawn.Keys[0].TextureObject;
            m_VertexDeclaration.GraphicsDevice.VertexDeclaration = m_VertexDeclaration;

            //m_VertexDeclaration.GraphicsDevice.RenderState.CullMode = CullMode.None;
            //m_VertexDeclaration.GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            //m_VertexDeclaration.GraphicsDevice.RenderState.DepthBufferEnable = false;

            foreach (TextureInfo ti in drawable_List_Drawn.Keys)
            {
                if (ti.SortNumber != currentTextureNumber)
                    be.Texture = ti.TextureObject;

                foreach (Drawable d in drawable_List_Drawn[ti])
                {
                    if (d is DrawableModel)
                    {
                        DrawModel(be.World, (DrawableModel)d);
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

            be.Begin();
            foreach (EffectPass pass in be.CurrentTechnique.Passes)
            {
                pass.Begin();
                d.DrawMe();
                pass.End();
            }
            be.End();
        }

        public void DrawModel(Matrix p_CurrentWorld, DrawableModel d)
        {
            Stack<Matrix> drawStack = new Stack<Matrix>();
            Matrix currentWorld = p_CurrentWorld;

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

            be.World = p_CurrentWorld;
            while (drawStack.Count > 0)
                be.World = Matrix.Multiply(drawStack.Pop(), be.World);

            //be.World = Matrix.Add(Matrix.CreateRotationZ(MathHelper.ToRadians(90)),be.World);
            
            foreach (ModelMesh mesh in d.ModelObject.Meshes)
            {
                m_VertexDeclaration.GraphicsDevice.Indices = mesh.IndexBuffer;
                be.Begin();

                // Loop through each pass in the effect like we do elsewhere
                foreach (EffectPass pass in be.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    d.DrawMe(mesh);
                    pass.End();
                }
                be.End();
            }
            be.World = currentWorld;
        }
    }
}
