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
        [NonSerialized]
        private RenderTarget2D m_SceneRenderTarget = null;
        public RenderTarget2D SceneRanderTarget
        {
            get { return m_SceneRenderTarget; }
            set { m_SceneRenderTarget = value; }
        }
        [NonSerialized]
        private RenderTarget2D m_NormalDepthRenderTarget = null;
        public RenderTarget2D NormalDepthRenderTarget
        {
            get { return m_NormalDepthRenderTarget; }
            set { m_NormalDepthRenderTarget = value; }
        }

		[NonSerialized]
        private RenderTarget2D m_distortionMap = null;
        public RenderTarget2D DistortionMap
        {
            get { return m_distortionMap; }
            set { m_distortionMap = value; }
        }

		[NonSerialized]
        private ResolveTexture2D m_tempTarget = null;
        public ResolveTexture2D TempRenderTarget
        {
            get { return m_tempTarget; }
            set { m_tempTarget = value; }
        }

		[NonSerialized]
        private RenderTarget2D m_DepthMapRenderTarget = null;
        public RenderTarget2D DepthMapRenderTarget
        {
            get { return m_DepthMapRenderTarget; }
            set { m_DepthMapRenderTarget = value; }
        }


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

        public bool DEBUG_WireframeMode = false;

        [NonSerialized]
        //private Model _modelTemp;

        SortedList<TextureInfo, List<Drawable>> drawable_List_Level;
        SortedList<TextureInfo, List<Drawable>> drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>();
        //SortedList<short, List<Drawable>> rooms;

        public bool saveOut = false;

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

		// Why is this getting called every frame??
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
            Draw(graphicsDevice, null);
        }

        public void Draw(GraphicsDevice graphicsDevice, Blob theBlob)
        {

            RenderState renderState = graphicsDevice.RenderState;

            renderState.AlphaBlendEnable = false;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;

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
                if (DEBUG_WireframeMode)
                {
                    renderState.CullMode = CullMode.None;
                    renderState.FillMode = FillMode.WireFrame;
                    renderState.DepthBufferEnable = false;
                }

                if (m_NormalDepthRenderTarget != null)
                {
                    graphicsDevice.SetRenderTarget(0, m_NormalDepthRenderTarget);
                    EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique = EffectManager.getSingleton.GetEffect(_effectName).Techniques["NormalDepth"];
                    foreach (TextureInfo ti in drawable_List_Drawn.Keys)
                    {
                        //if (ti.SortNumber != currentTextureNumber)
                        //{
                        //    if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                        //    {
                        //        ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).Texture = TextureManager.getSingleton.GetTexture(ti.TextureName);
                        //    }
                        //    else
                        //    {
                        //        graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(ti.TextureName);
                        //        if (m_TextureParameterName != "NONE")
                        //            EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_TextureParameterName].SetValue(TextureManager.getSingleton.GetTexture(ti.TextureName));
                        //    }
                        //}

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
					if (theBlob != null)
					{
					    EffectManager.getSingleton.GetEffect("cartoonEffect").Begin();
					    foreach (EffectPass pass in EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique.Passes)
					    {
					        pass.Begin();
					        theBlob.DrawMe();
					        pass.End();
					    }
					    EffectManager.getSingleton.GetEffect("cartoonEffect").End();
					}
                }

                //if (m_DepthMapRenderTarget != null) {
                //    graphicsDevice.SetRenderTarget(0, m_DepthMapRenderTarget);
                //    graphicsDevice.Clear(Color.White);
                    
                //    EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique = EffectManager.getSingleton.GetEffect("cartoonEffect").Techniques["ShadowMap"];

					

                    //foreach (TextureInfo ti in drawable_List_Drawn.Keys) 
                    //{
                    //    //if (ti.SortNumber != currentTextureNumber)
                    //    //{
                    //    //    if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                    //    //    {
                    //    //        ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).Texture = TextureManager.getSingleton.GetTexture(ti.TextureName);
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(ti.TextureName);
                    //    //        if (m_TextureParameterName != "NONE" && _effectName != "DepthBuffer")
                    //    //            EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_TextureParameterName].SetValue(TextureManager.getSingleton.GetTexture(ti.TextureName));
                    //    //    }
                    //    //}

                    //    foreach (Drawable d in drawable_List_Drawn[ti]) {
                    //        if (d is StaticModel) {
                    //            DrawModel(m_WorldMatrix, (StaticModel)d, graphicsDevice);

                    //        } else {
                    //            DrawPrimitives(d, graphicsDevice);
                    //        }
                    //    }
                    //}

                    //if (theBlob != null)
                    //{
                    //    EffectManager.getSingleton.GetEffect("cartoonEffect").Begin();
                    //    foreach (EffectPass pass in EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique.Passes)
                    //    {
                    //        pass.Begin();
                    //        theBlob.DrawMe();
                    //        pass.End();
                    //    }
                    //    EffectManager.getSingleton.GetEffect("cartoonEffect").End();
                    //}
					
                //}

                //graphicsDevice.SetRenderTarget(0, null);
                //if (saveOut) {
                //    m_DepthMapRenderTarget.GetTexture().Save("DepthMap.bmp", ImageFileFormat.Bmp);
                //    m_DepthMapRenderTarget.GetTexture().Save("DepthMap.png", ImageFileFormat.Png);
                //    saveOut = false;
                //}

                if(m_SceneRenderTarget != null && !DEBUG_WireframeMode)
                    graphicsDevice.SetRenderTarget(0, m_SceneRenderTarget);
                else
                    graphicsDevice.SetRenderTarget(0, null);
				
				//if(m_DepthMapRenderTarget != null)
					//EffectManager.getSingleton.GetEffect("cartoonEffect").Parameters["ShadowMap"].SetValue(m_DepthMapRenderTarget.GetTexture());

                graphicsDevice.Clear(Color.CornflowerBlue);

                if(_effectName == "cartoonEffect")
                    EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique = EffectManager.getSingleton.GetEffect(_effectName).Techniques["Lambert"];
                
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
                            if (m_TextureParameterName != "NONE" && _effectName == "cartoonEffect")
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
                if(!DEBUG_WireframeMode)
                    graphicsDevice.SetRenderTarget(0, m_distortionMap);
                graphicsDevice.RenderState.DepthBufferEnable = true;

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
                            if (m_TextureParameterName != "NONE" && _effectName == "cartoonEffect")
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

                if(theBlob != null && !DEBUG_WireframeMode)
                {
                    //graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                   
                    if (EffectManager.getSingleton.GetEffect("Distorter") != null && m_distortionMap != null)
                    {

                        EffectManager.getSingleton.GetEffect("Distorter").CurrentTechnique =
                                EffectManager.getSingleton.GetEffect("Distorter").Techniques["PullIn"];
                        EffectManager.getSingleton.GetEffect("Distorter").Parameters["DistortionScale"].SetValue(0.04f);
                        Random r = new Random();
                        EffectManager.getSingleton.GetEffect("Distorter").Parameters["Time"].SetValue(r.Next()/10.0f);
                        //EffectManager.getSingleton.GetEffect("Distorter").Parameters["Time"].SetValue(r.NextDouble() * 30.0);

                        graphicsDevice.SetRenderTarget(0, m_distortionMap);
                        graphicsDevice.Clear(Color.Black);
                        
                        //graphicsDevice.Textures[0] = theBlob.DisplacementText;

                        EffectManager.getSingleton.GetEffect("Distorter").Begin();
                        foreach (EffectPass pass in EffectManager.getSingleton.GetEffect("Distorter").CurrentTechnique.Passes)
                        {
                            pass.Begin();
                            theBlob.DrawMe();
                            pass.End();
                        }
                        EffectManager.getSingleton.GetEffect("Distorter").End();
                    }
                }
                

                if (m_SceneRenderTarget != null && !DEBUG_WireframeMode)
                    ApplyPostProcessing(graphicsDevice, theBlob != null);

                graphicsDevice.RenderState.AlphaBlendEnable = true;

				if (theBlob != null)
				{
					EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique = EffectManager.getSingleton.GetEffect(_effectName).Techniques["LambertOnBlob"];
					EffectManager.getSingleton.GetEffect("cartoonEffect").Parameters["Texture"].SetValue(theBlob.text);
					EffectManager.getSingleton.GetEffect("cartoonEffect").Begin();
					foreach (EffectPass pass in EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique.Passes)
					{
						pass.Begin();
						theBlob.DrawMe();
						pass.End();
					}
					EffectManager.getSingleton.GetEffect("cartoonEffect").End();
				}

                if (DEBUG_WireframeMode)
                {
                    renderState.CullMode = CullMode.CullClockwiseFace;
                    renderState.FillMode = FillMode.Solid;
                    renderState.DepthBufferEnable = true;
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

            if (_effectName == "cartoonEffect" && EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique != EffectManager.getSingleton.GetEffect(_effectName).Techniques["ShadowMap"] &&
                EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique != EffectManager.getSingleton.GetEffect(_effectName).Techniques["ShadowedScene"])
            {
                if (d.Name == "sky")
                    EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique = EffectManager.getSingleton.GetEffect(_effectName).Techniques["SkyBox"];
                else
                    EffectManager.getSingleton.GetEffect(_effectName).CurrentTechnique = EffectManager.getSingleton.GetEffect(_effectName).Techniques["Lambert"];
            }

            if (d.Name == CurrentlySelected)
            {
                d.ShowVertices = true;
                //d.TextureName = TextureName;
            }
            else
            {
                d.ShowVertices = false;
            }

            /*for (int j = 0; j < 4; j++)
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
            }*/

            if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World = p_CurrentWorld;
            else
            {
                EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].SetValue(d.Transform);
            }

            /*while (drawStack.Count > 0)
            {
                if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                    ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World = Matrix.Multiply(drawStack.Pop(), ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World);
                else
                {
                    EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].SetValue(Matrix.Multiply(drawStack.Pop(), EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].GetValueMatrix()));
                }
			}*/
			d.DrawMe(graphicsDevice,EffectManager.getSingleton.GetEffect(_effectName), m_GameMode);
            if (EffectManager.getSingleton.GetEffect(_effectName) is BasicEffect)
                ((BasicEffect)EffectManager.getSingleton.GetEffect(_effectName)).World = currentWorld;
            else
                EffectManager.getSingleton.GetEffect(_effectName).Parameters[m_WorldParameterName].SetValue(currentWorld);
        }

        public void ApplyPostProcessing(GraphicsDevice graphicsDevice, bool blob)
        {
            graphicsDevice.SetRenderTarget(0, null);

			// Draw a fullscreen sprite to apply the postprocessing effect.
			SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

			if (m_distortionMap != null && blob)
			{
				graphicsDevice.Textures[1] = m_distortionMap.GetTexture();
				//graphicsDevice.Textures[0] = m_tempTarget;
				EffectManager.getSingleton.GetEffect("Distort").CurrentTechnique =
					EffectManager.getSingleton.GetEffect("Distort").Techniques["Distort"];

				spriteBatch.Begin(SpriteBlendMode.None,
							  SpriteSortMode.Immediate,
							  SaveStateMode.None);

				EffectManager.getSingleton.GetEffect("Distort").Begin();
				EffectManager.getSingleton.GetEffect("Distort").CurrentTechnique.Passes[0].Begin();

				spriteBatch.Draw(m_SceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

				spriteBatch.End();

				EffectManager.getSingleton.GetEffect("Distort").CurrentTechnique.Passes[0].End();
				EffectManager.getSingleton.GetEffect("Distort").End();
			}


			if (!blob)
			{
				EffectManager.getSingleton.GetEffect("cartoonEffect").Begin();
				EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique.Passes[0].Begin();
				
				spriteBatch.Begin(SpriteBlendMode.None,
							  SpriteSortMode.Immediate,
							  SaveStateMode.None);


				spriteBatch.Draw(m_SceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

				spriteBatch.End();

				EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique.Passes[0].End();
				EffectManager.getSingleton.GetEffect("cartoonEffect").End();
			}

            if (EffectManager.getSingleton.GetEffect("postprocessEffect") != null && m_tempTarget != null)
            {
                graphicsDevice.ResolveBackBuffer(m_tempTarget);
                graphicsDevice.SetRenderTarget(0, null);
                Vector2 resolution = new Vector2(m_SceneRenderTarget.Width,
                                                m_SceneRenderTarget.Height);

				// Invalid Operation Exception: The render target must not be set on the device when calling GetTexture. 
                Texture2D normalDepthTexture = m_NormalDepthRenderTarget.GetTexture();

                EffectManager.getSingleton.GetEffect("postprocessEffect").Parameters["EdgeWidth"].SetValue(1.0f);
                EffectManager.getSingleton.GetEffect("postprocessEffect").Parameters["EdgeIntensity"].SetValue(1.0f);
                EffectManager.getSingleton.GetEffect("postprocessEffect").Parameters["ScreenResolution"].SetValue(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
                EffectManager.getSingleton.GetEffect("postprocessEffect").Parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

                // Activate the appropriate effect technique.
                EffectManager.getSingleton.GetEffect("postprocessEffect").CurrentTechnique = EffectManager.getSingleton.GetEffect("postprocessEffect").Techniques["EdgeDetect"];

                
                spriteBatch.Begin(SpriteBlendMode.None,
                                  SpriteSortMode.Immediate,
                                  SaveStateMode.None);

                EffectManager.getSingleton.GetEffect("postprocessEffect").Begin();
                EffectManager.getSingleton.GetEffect("postprocessEffect").CurrentTechnique.Passes[0].Begin();

                spriteBatch.Draw(m_tempTarget, Vector2.Zero, Color.White);

                spriteBatch.End();

                EffectManager.getSingleton.GetEffect("postprocessEffect").CurrentTechnique.Passes[0].End();
                EffectManager.getSingleton.GetEffect("postprocessEffect").End();
            }
           
        }
    }
}
