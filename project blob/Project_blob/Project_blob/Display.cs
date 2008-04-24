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
    [Serializable]
    public class Display
    {
        private bool m_GameMode = false;
        public bool GameMode
        {
            get { return m_GameMode; }
            set { m_GameMode = value; }
        }

        private String _textureName;
        public String TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        private String _effectName;
        public String EffectName
        {
            get
            {
                return _effectName;
            }
            set
            {
                _effectName = value;
            }
        }

        [NonSerialized]
        private Model _modelTemp;

        SortedList<TextureInfo, List<Drawable>> drawable_List_Level;
        SortedList<TextureInfo, List<Drawable>> drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>();

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

		private StaticModel m_skyBox;
		public StaticModel SkyBox
		{
			get { return m_skyBox; }
			set { m_skyBox = value; }
		}

        public void WipeDrawn()
        {
            drawable_List_Drawn.Clear();
			if(m_skyBox != null)
				AddToBeDrawn(m_skyBox);
        }

        public void AddToBeDrawn(Drawable p_Drawable)
        {
            if (drawable_List_Drawn.Keys.Contains(p_Drawable.GetTextureKey()))
            {
                List<Drawable> temp = drawable_List_Drawn[p_Drawable.GetTextureKey()];
                temp.Add(p_Drawable);
                drawable_List_Drawn[p_Drawable.GetTextureKey()] = temp;
            }
            else
            {
                List<Drawable> temp = new List<Drawable>();
                temp.Add(p_Drawable);
                drawable_List_Drawn[p_Drawable.GetTextureKey()] = temp;
            }
        }

        public Display(Matrix p_World, Matrix p_View, Matrix p_Projection)
        {
            _effectName = "basic";
            _textureName = "point_text";
            ShowAxis = true;
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).Alpha = 1.0f;
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).SpecularPower = 5.0f;
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).DirectionalLight0.Enabled = true;
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).DirectionalLight0.DiffuseColor = Vector3.One;
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).DirectionalLight0.SpecularColor = Vector3.One;

            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).LightingEnabled = true;
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).TextureEnabled = true;

            drawable_List_Level = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());
            drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());

            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World = p_World;
            m_WorldMatrix = p_World;

            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).View = p_View;
            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).Projection = p_Projection;
        }

        public Display(Matrix p_World, String effectName, String p_WorldParameterName, 
            String p_TextureParameterName, String p_TechniqueName)
        {
            drawable_List_Level = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());
            drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());

            _effectName = effectName;
            _textureName = "point_text";
            
            ShowAxis = true;

            m_WorldMatrix = p_World;
            m_WorldParameterName = p_WorldParameterName;
            m_TextureParameterName = p_TextureParameterName;

            m_TechniqueName = p_TechniqueName;

        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            if (drawable_List_Drawn.Count > 0)
            {
                if (m_TechniqueName != null)
                    EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique = EffectManager.getSingleton.GetEffect(_effectName).Techniques[m_TechniqueName];

                int currentTextureNumber = -1;
                //graphicsDevice.Textures[0] = vertexBuffer_List_Drawn.Keys[0].TextureObject;
                if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                {
                    ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).Texture = TextureManager.getSingleton.GetTexture(drawable_List_Drawn.Keys[0].TextureName);
                }
                else
                {
                    graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(drawable_List_Drawn.Keys[0].TextureName);
                    if (m_TextureParameterName != "NONE")
                        EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_TextureParameterName].SetValue(TextureManager.getSingleton.GetTexture(drawable_List_Drawn.Keys[0].TextureName));
                }

                //graphicsDevice.RenderState.CullMode = CullMode.None;
                //graphicsDevice.RenderState.FillMode = FillMode.WireFrame;
                //graphicsDevice.RenderState.DepthBufferEnable = false;

                if (ShowAxis)
                {
                    VertexPositionNormalTexture[] axisVertices = new VertexPositionNormalTexture[6];

                    axisVertices[0] = new VertexPositionNormalTexture(new Vector3(-2, 0, 0), Vector3.Zero, Vector2.Zero);
                    axisVertices[1] = new VertexPositionNormalTexture(new Vector3(2, 0, 0), Vector3.Zero, new Vector2(0, 1));
                    axisVertices[2] = new VertexPositionNormalTexture(new Vector3(0, -2, 0), Vector3.Zero, Vector2.Zero);
                    axisVertices[3] = new VertexPositionNormalTexture(new Vector3(0, 2, 0), Vector3.Zero, new Vector2(0, 1));
                    axisVertices[4] = new VertexPositionNormalTexture(new Vector3(0, 0, -2), Vector3.Zero, Vector2.Zero);
                    axisVertices[5] = new VertexPositionNormalTexture(new Vector3(0, 0, 2), Vector3.Zero, new Vector2(0, 1));

                    VertexBuffer vb = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.SizeInBytes * 6, BufferUsage.None);
                    vb.SetData<VertexPositionNormalTexture>(axisVertices);

                    //VertexStream tempBuffer = m_VertexDeclaration.GraphicsDevice.Vertices[0];
                    graphicsDevice.Vertices[0].SetSource(vb, 0, VertexPositionNormalTexture.SizeInBytes);
                    Texture temp = graphicsDevice.Textures[0];
                    graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(TextureName);
                    EffectManager.getSingleton.GetEffect(_effectName).Begin();

                    foreach (EffectPass pass in EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 6);
                        pass.End();
                    }
                    EffectManager.getSingleton.GetEffect(_effectName).End();
                    graphicsDevice.Textures[0] = temp;
                    //graphicsDevice.Vertices[0].SetSource(tempBuffer.VertexBuffer,0,tempBuffer.VertexStride);
                }

                foreach (TextureInfo ti in drawable_List_Drawn.Keys)
                {
                    if (ti.SortNumber != currentTextureNumber)
                    {
                        if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                        {
                            ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).Texture = TextureManager.getSingleton.GetTexture(ti.TextureName);
                        }
                        else
                        {
                            graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(ti.TextureName);
                            if (m_TextureParameterName != "NONE")
                                EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_TextureParameterName].SetValue(TextureManager.getSingleton.GetTexture(ti.TextureName));
                        }
                    }

                    foreach (Drawable d in drawable_List_Drawn[ti])
                    {
                        if (d is StaticModel)
                        {
                            DrawModel(m_WorldMatrix, (StaticModel)d, graphicsDevice);

                        }
                        else
                        {
                            DrawPrimitives(d, graphicsDevice);
                        }
                    }
                }
            }
        }

        public void DrawPrimitives(Drawable d, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Vertices[0].SetSource(d.getVertexBuffer(), 0, d.getVertexStride());

            EffectManager.getSingleton.GetEffect(_effectName).Begin();
            foreach (EffectPass pass in EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique.Passes)
            {
                pass.Begin();
                d.DrawMe();
                pass.End();
            }
            EffectManager.getSingleton.GetEffect(_effectName).End();
        }

        public void DrawModel(Matrix p_CurrentWorld, StaticModel d, GraphicsDevice graphicsDevice)
        {
            Stack<Matrix> drawStack = new Stack<Matrix>();
            Matrix currentWorld = p_CurrentWorld;

            if (d.Name == "sky")
                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xEnableLighting"].SetValue(false);
            else
                EffectManager.getSingleton.GetEffect(_effectName).Parameters["xEnableLighting"].SetValue(true);

            if (d.Name == CurrentlySelected)
            {
                d.ShowVertices = true;
                //d.TextureName = TextureName;
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

            if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World = p_CurrentWorld;
            else
            {
                EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].SetValue(currentWorld);
            }

            while (drawStack.Count > 0)
            {
                if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                    ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World = Matrix.Multiply(drawStack.Pop(), ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World);
                else
                {
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].SetValue(Matrix.Multiply(drawStack.Pop(), EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].GetValueMatrix()));
                }
            }
            _modelTemp = ModelManager.getSingleton.GetModel(d.ModelName);
            if (_modelTemp != null)
            {
                foreach (ModelMesh mesh in _modelTemp.Meshes)
                {
                    graphicsDevice.Indices = mesh.IndexBuffer;
                    EffectManager.getSingleton.GetEffect(_effectName).Begin();

                    // Loop through each pass in the effect like we do elsewhere
                    foreach (EffectPass pass in EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique.Passes)
                    {
                        pass.Begin();
                        d.DrawMe(mesh, graphicsDevice, m_GameMode);
                        pass.End();
                    }
                    EffectManager.getSingleton.GetEffect(_effectName).End();
                }
            }
            if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World = currentWorld;
            else
                EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].SetValue(currentWorld);
        }
    }
}
