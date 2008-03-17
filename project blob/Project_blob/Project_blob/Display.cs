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
        SortedList<TextureInfo, List<VertexBuffer>> vertexBuffer_List_Level = new SortedList<TextureInfo, List<VertexBuffer>>();
        SortedList<TextureInfo, List<VertexBuffer>> vertexBuffer_List_Drawn = new SortedList<TextureInfo, List<VertexBuffer>>();
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

        public Display(GraphicsDevice gd, EffectPool ep)
        {
            basicEffectVertexDeclaration = new VertexDeclaration(
                gd, VertexPositionNormalTexture.VertexElements);

            be = new BasicEffect(gd, ep);
            be.AmbientLightColor = new Vector3(.75f, .75f, .75f);
            be.DiffuseColor = Vector3.One;
            be.SpecularPower = 5.0f;
            be.SpecularColor = new Vector3(.25f, .25f, .25f);
            be.LightingEnabled = true;
            be.TextureEnabled = true;
        }

        public void Draw(Matrix p_World, Matrix p_View, Matrix p_Projection)
        {
            be.World = p_World;
            be.View = p_View;
            be.Projection = p_Projection;

        }
    }
}
