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
        SortedList<TextureInfo, List<VertexBuffer>> vertexBuffer_List_Level ;
        SortedList<TextureInfo, List<VertexBuffer>> vertexBuffer_List_Drawn = new SortedList<TextureInfo, List<VertexBuffer>>();
        VertexDeclaration m_VertexDeclaration;
        Effect m_CurrentEffect;

        public SortedList<TextureInfo, List<VertexBuffer>> DrawnList
        {
            get
            {
                return vertexBuffer_List_Drawn;
            }
            set 
            {
                vertexBuffer_List_Drawn = value; 
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
                CullMode.CullClockwiseFace;

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

            vertexBuffer_List_Level = new SortedList<TextureInfo, List<VertexBuffer>>(new TextureInfoComparer());
            vertexBuffer_List_Drawn = new SortedList<TextureInfo, List<VertexBuffer>>(new TextureInfoComparer());
            
            be.World = p_World;
            be.View = p_View;
            be.Projection = p_Projection;
        }

        public void Draw(PrimitiveType p_DrawType, int p_PrimitiveCount, int p_VertexStride)
        {

            int currentTextureNumber = vertexBuffer_List_Drawn.Keys[0].SortNumber;
            //m_GraphicsDevice.Textures[0] = vertexBuffer_List_Drawn.Keys[0].TextureObject;
            be.Texture = vertexBuffer_List_Drawn.Keys[0].TextureObject;
            m_VertexDeclaration.GraphicsDevice.VertexDeclaration = m_VertexDeclaration;

            foreach (TextureInfo ti in vertexBuffer_List_Drawn.Keys)
            {
               if(ti.SortNumber != currentTextureNumber)
                   be.Texture = ti.TextureObject;

                foreach(VertexBuffer vb in vertexBuffer_List_Drawn[ti])
                {
                   m_VertexDeclaration.GraphicsDevice.Vertices[0].SetSource(vb, 0, p_VertexStride);
                   be.Begin();
                   // Loop through each pass in the effect like we do elsewhere
                   foreach (EffectPass pass in be.CurrentTechnique.Passes)
                   {
                       pass.Begin();
                       m_VertexDeclaration.GraphicsDevice.DrawPrimitives(p_DrawType, 0, p_PrimitiveCount);
                       pass.End();
                   }
                   be.End();
                }
            }
        }
    }
}
