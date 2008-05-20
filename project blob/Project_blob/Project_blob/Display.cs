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

namespace Project_blob {
	//This class holds a list of VectorLists to be drawn to the screen
	public class Display {
		[NonSerialized]
		private const float blurAmount = 2f;

		[NonSerialized]
		private RenderTarget2D m_SceneRenderTarget = null;
		public RenderTarget2D SceneRanderTarget {
			get { return m_SceneRenderTarget; }
			set {
				m_SceneRenderTarget = value;
				resolution = new Vector2(m_SceneRenderTarget.Width, m_SceneRenderTarget.Height);
			}
		}

		private Vector2 resolution = Vector2.Zero;

		Texture2D normalDepthTexture = null;

		[NonSerialized]
		private RenderTarget2D m_NormalDepthRenderTarget = null;
		public RenderTarget2D NormalDepthRenderTarget {
			get { return m_NormalDepthRenderTarget; }
			set { m_NormalDepthRenderTarget = value; }
		}

		[NonSerialized]
		private RenderTarget2D m_distortionMap = null;
		public RenderTarget2D DistortionMap {
			get { return m_distortionMap; }
			set { m_distortionMap = value; }
		}

		[NonSerialized]
		private ResolveTexture2D m_tempTarget = null;
		public ResolveTexture2D TempRenderTarget {
			get { return m_tempTarget; }
			set { m_tempTarget = value; }
		}

		[NonSerialized]
		private RenderTarget2D m_DepthMapRenderTarget = null;
		public RenderTarget2D DepthMapRenderTarget {
			get { return m_DepthMapRenderTarget; }
			set { m_DepthMapRenderTarget = value; }
		}


		private bool m_GameMode = false;
		public bool GameMode {
			get { return m_GameMode; }
			set { m_GameMode = value; }
		}

		private string _textureName;
		public string TextureName {
			get { return _textureName; }
			set { _textureName = value; }
		}

		private string _effectName;
		public string EffectName {
			get {
				return _effectName;
			}
			set {
				_effectName = value;
			}
		}

		[NonSerialized]
		Effect m_cartoonEffect;
		public Effect CartoonEffect {
			get { return m_cartoonEffect; }
			set { m_cartoonEffect = value; }
		}

		[NonSerialized]
		Effect m_postProcessEffect;
		public Effect PostProcessEffect {
			get { return m_postProcessEffect; }
			set { m_postProcessEffect = value; }
		}

		[NonSerialized]
		Effect m_distort;
		public Effect Distort {
			get { return m_distort; }
			set { m_distort = value; }
		}

		[NonSerialized]
		Effect m_distorter;
		public Effect Distorter {
			get { return m_distorter; }
			set { m_distorter = value; }
		}

		[NonSerialized]
		private SpriteBatch m_SpriteBatch;

		public bool DEBUG_WireframeMode = false;

		[NonSerialized]
		//private Model _modelTemp;

		//SortedList<TextureInfo, List<Drawable>> drawable_List_Level;
		//SortedList<TextureInfo, List<Drawable>> drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>();

		//Two dimensional list of drawables. 
		//First dimension index is TextureID
		//All drawables added to the second 
		//dimension must share the same texture
		List<List<Drawable>> m_DrawList;
		//SortedList<short, List<Drawable>> rooms;

		public bool saveOut = false;

		bool m_ShowAxis = false;
		public bool ShowAxis {
			get {
				return m_ShowAxis;
			}
			set {
				m_ShowAxis = value;
			}
		}

		string m_CurrentlySelected = string.Empty;
		public string CurrentlySelected {
			get {
				return m_CurrentlySelected;
			}
			set {
				m_CurrentlySelected = value;
			}
		}

		/*public SortedList<TextureInfo, List<Drawable>> DrawnList
		{
			get
			{
				return drawable_List_Drawn;
			}
			set
			{
				drawable_List_Drawn = value;
			}
		}*/

		public List<List<Drawable>> DrawnList {
			get {
				return m_DrawList;
			}
			set {
				m_DrawList = value;
			}
		}

		string m_TextureParameterName = string.Empty;
		public string TextureParameterName {
			get {
				return m_TextureParameterName;
			}
			set {
				m_TextureParameterName = value;
			}
		}

		string m_WorldParameterName = string.Empty;
		public string WorldParameterName {
			get {
				return m_WorldParameterName;
			}
			set {
				m_WorldParameterName = value;
			}
		}

		Matrix m_WorldMatrix;
		public Matrix WorldMatrix {
			get {
				return m_WorldMatrix;
			}
			set {
				m_WorldMatrix = value;
			}
		}

		string m_TechniqueName = null;
		public string TechniqueName {
			get {
				return m_TechniqueName;
			}
			set {
				m_TechniqueName = value;
			}
		}

		private StaticModel m_skyBox;
		public StaticModel SkyBox {
			get { return m_skyBox; }
			set { m_skyBox = value; }
		}

		public void WipeDrawn() {
			m_DrawList.Clear();
			if (m_skyBox != null)
				AddToBeDrawn(m_skyBox);
		}

		public void AddToBeDrawn(Drawable p_Drawable) {
			/*if (drawable_List_Drawn.Keys.Contains(p_Drawable.GetTextureKey()))
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
			}*/

			if (!m_DrawList[p_Drawable.GetTextureID()].Contains(p_Drawable))
				m_DrawList[p_Drawable.GetTextureID()].Add(p_Drawable);
#if DEBUG
			++SceneManager.getSingleton.Drawn;
#endif
		}

		public Display(GraphicsDevice gd) {
			ShowAxis = true;
			m_WorldMatrix = Matrix.Identity;
			m_SpriteBatch = new SpriteBatch(gd);
			initialize(gd);
		}

		/*public Display(Matrix p_World, Matrix p_View, Matrix p_Projection)
		{
			//_effectName = "basic";
			//_textureName = "point_text";
			ShowAxis = true;
			//m_cartoonEffect.Alpha = 1.0f;
			//m_cartoonEffect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
			//m_cartoonEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
			//m_cartoonEffect.SpecularPower = 5.0f;
			//m_cartoonEffect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

			//m_cartoonEffect.DirectionalLight0.Enabled = true;
			//m_cartoonEffect.DirectionalLight0.DiffuseColor = Vector3.One;
			//m_cartoonEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
			//m_cartoonEffect.DirectionalLight0.SpecularColor = Vector3.One;

			//m_cartoonEffect.LightingEnabled = true;
			//m_cartoonEffect.TextureEnabled = true;

			//drawable_List_Level = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());
			//drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());

			//m_cartoonEffect.Parameters["World"].SetValue(p_World);
			m_WorldMatrix = p_World;

			//m_cartoonEffect.Parameters["View"].SetValue(p_View);
			//m_cartoonEffect.Parameters["Projection"].SetValue(p_Projection);

            initialize();
		}*/

		/*public Display(Matrix p_World, string effectName, string p_WorldParameterName,
			string p_TextureParameterName, string p_TechniqueName)
		{
			//drawable_List_Level = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());
			//drawable_List_Drawn = new SortedList<TextureInfo, List<Drawable>>(new TextureInfoComparer());

			_effectName = effectName;
			//_textureName = "point_text";

			ShowAxis = true;

			m_WorldMatrix = p_World;
			m_WorldParameterName = p_WorldParameterName;
			m_TextureParameterName = p_TextureParameterName;

			m_TechniqueName = p_TechniqueName;

            initialize();
		}*/

		public void initialize(GraphicsDevice gd) {
			//initialize draw list, each texture gets a list
			m_DrawList = new List<List<Drawable>>();
			for (int i = 0; i < TextureManager.TextureList.Count; ++i) {
				m_DrawList.Add(new List<Drawable>());
			}

			//*hardcode*
			this.EffectName = "cartoonEffect";
			this.WorldParameterName = "World";
			this.TextureParameterName = "Texture";
			this.TechniqueName = "Lambert";
			this.TextureName = "point_text";
			//*end hardcode*

			createRenderTargets(gd);
		}

		private void createRenderTargets(GraphicsDevice gd) {
			this.Distort = EffectManager.getSingleton.GetEffect("distort");
			this.Distorter = EffectManager.getSingleton.GetEffect("distorter");
			this.CartoonEffect = EffectManager.getSingleton.GetEffect("cartoonEffect");
			this.PostProcessEffect = EffectManager.getSingleton.GetEffect("postprocessEffect");

			PresentationParameters pp = gd.PresentationParameters;

			this.m_SceneRenderTarget = new RenderTarget2D(gd,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			this.m_NormalDepthRenderTarget = new RenderTarget2D(gd,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			this.m_distortionMap = new RenderTarget2D(gd,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			this.m_tempTarget = new ResolveTexture2D(gd, pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat);

			this.m_DepthMapRenderTarget = new RenderTarget2D(gd,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);
		}

		public void Draw(GraphicsDevice graphicsDevice) {
			Draw(graphicsDevice, null);
		}

		public void Draw(GraphicsDevice graphicsDevice, Blob theBlob) {
			RenderState renderState = graphicsDevice.RenderState;

			renderState.AlphaBlendEnable = false;
			renderState.AlphaTestEnable = false;
			renderState.DepthBufferEnable = true;

			if (m_DrawList.Count > 0) {
				if (m_TechniqueName != null)
					try {
						m_cartoonEffect.CurrentTechnique = m_cartoonEffect.Techniques[m_TechniqueName];
					} catch (Exception e) {
						Log.Out.WriteLine(e);
					}
				m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(TextureManager.GetTexture(0));

				if (ShowAxis) {
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
					graphicsDevice.Textures[0] = TextureManager.GetTexture(TextureName);
					m_cartoonEffect.Begin();

					foreach (EffectPass pass in m_cartoonEffect.CurrentTechnique.Passes) {
						pass.Begin();
						graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, 6);
						pass.End();
					}
					m_cartoonEffect.End();
					graphicsDevice.Textures[0] = temp;
					//graphicsDevice.Vertices[0].SetSource(tempBuffer.VertexBuffer,0,tempBuffer.VertexStride);
				}
				if (DEBUG_WireframeMode) {
					renderState.CullMode = CullMode.None;
					renderState.FillMode = FillMode.WireFrame;
					renderState.DepthBufferEnable = false;
				}

				graphicsDevice.Vertices[0].SetSource(null, 0, 0);
				for (int i = 0; i < m_DrawList.Count; ++i) {
					foreach (Drawable d in m_DrawList[i]) {
						if (d is StaticModel) {
							((StaticModel)d).updateVertexBuffer();
						}
					}
				}

				if (m_NormalDepthRenderTarget != null) {
					graphicsDevice.SetRenderTarget(0, m_NormalDepthRenderTarget);
					graphicsDevice.RenderState.DepthBufferWriteEnable = true;
					m_cartoonEffect.CurrentTechnique = m_cartoonEffect.Techniques["NormalDepth"];
					/*foreach (TextureInfo ti in drawable_List_Drawn.Keys)
					{
						if (!ti.TextureName.Equals("event"))
						{
							if (ti.SortNumber != currentTextureNumber)
							{
								graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(ti.TextureName);
								if (m_TextureParameterName != "NONE")
									m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(TextureManager.getSingleton.GetTexture(ti.TextureName));

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
					}*/

					for (int i = 0; i < m_DrawList.Count; ++i) {
						Texture2D t = TextureManager.TextureList[i];
						//temporary check
						//if (!t.Name.Equals("event"))
						//{
						//if (m_TextureParameterName != "NONE")
						m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(t);

						foreach (Drawable d in m_DrawList[i]) {
							if (d is StaticModel) {
								DrawModel(m_WorldMatrix, (StaticModel)d, graphicsDevice);
							} else {
								DrawPrimitives(d, graphicsDevice);
							}
						}
						//}
					}


					if (theBlob != null) {
						//m_cartoonEffect.Begin();
						//foreach (EffectPass pass in EffectManager.getSingleton.GetEffect("cartoonEffect").CurrentTechnique.Passes)
						//{
						//    pass.Begin();
						//    theBlob.DrawMe();
						//    pass.End();
						//}
						//EffectManager.getSingleton.GetEffect("cartoonEffect").End();
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

				if (m_SceneRenderTarget != null && !DEBUG_WireframeMode)
					graphicsDevice.SetRenderTarget(0, m_SceneRenderTarget);
				else
					graphicsDevice.SetRenderTarget(0, null);

				//if(m_DepthMapRenderTarget != null)
				//EffectManager.getSingleton.GetEffect("cartoonEffect").Parameters["ShadowMap"].SetValue(m_DepthMapRenderTarget.GetTexture());

				graphicsDevice.Clear(Color.CornflowerBlue);

				// draw skybox
				if (m_skyBox != null) {
					m_cartoonEffect.CurrentTechnique = m_cartoonEffect.Techniques["SkyBox"];
					m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(TextureManager.GetTexture(m_skyBox.GetTextureID()));
					DrawModel(m_WorldMatrix, m_skyBox, graphicsDevice);
				}

				//if (_effectName == "cartoonEffect")
				m_cartoonEffect.CurrentTechnique = m_cartoonEffect.Techniques["Lambert"];

				/*foreach (TextureInfo ti in drawable_List_Drawn.Keys)
				{
                    if (!ti.TextureName.Equals("event"))
                    {
                        if (ti.SortNumber != currentTextureNumber)
                        {
                            Texture2D tempText = TextureManager.getSingleton.GetTexture(ti.TextureName);
                            m_cartoonEffect.Parameters["Texture"].SetValue(tempText);
                            graphicsDevice.Textures[0] = tempText;
                            if (m_TextureParameterName != "NONE" && _effectName == "m_cartoonEffect")
                                m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(tempText);

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
				}*/

				for (int i = 0; i < m_DrawList.Count; ++i) {
					Texture2D t = TextureManager.TextureList[i];
					//temporary check
					//if (!t.Name.Equals("event"))
					//{
					//if (m_TextureParameterName != "NONE")
					m_cartoonEffect.Parameters["Texture"].SetValue(t);

					//if (m_TextureParameterName != "NONE" && _effectName == "m_cartoonEffect")
					//	m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(t);

					foreach (Drawable d in m_DrawList[i]) {
						if (d is StaticModel) {
							DrawModel(m_WorldMatrix, (StaticModel)d, graphicsDevice);
						} else {
							DrawPrimitives(d, graphicsDevice);
						}
					}
					//}
				}


				if (!DEBUG_WireframeMode)
					graphicsDevice.SetRenderTarget(0, m_distortionMap);
				graphicsDevice.RenderState.DepthBufferEnable = true;
				graphicsDevice.RenderState.DepthBufferWriteEnable = true;
				graphicsDevice.Clear(Color.Black);

				/*foreach (TextureInfo ti in drawable_List_Drawn.Keys)
				{
					if (!ti.TextureName.Equals("event"))
					{
						if (ti.SortNumber != currentTextureNumber)
						{
							graphicsDevice.Textures[0] = TextureManager.getSingleton.GetTexture(ti.TextureName);
							if (m_TextureParameterName != "NONE" && _effectName == "cartoonEffect")
								m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(TextureManager.getSingleton.GetTexture(ti.TextureName));

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
				}*/

				for (int i = 0; i < m_DrawList.Count; ++i) {
					Texture2D t = TextureManager.TextureList[i];
					//temporary check
					//if (!t.Name.Equals("event"))
					//{
					//if (m_TextureParameterName != "NONE")
					//	m_cartoonEffect.Parameters["Texture"].SetValue(t);

					//if (m_TextureParameterName != "NONE" && _effectName == "m_cartoonEffect")
					m_cartoonEffect.Parameters[m_TextureParameterName].SetValue(t);

					foreach (Drawable d in m_DrawList[i]) {
						if (d is StaticModel) {
							DrawModel(m_WorldMatrix, (StaticModel)d, graphicsDevice);
						} else {
							DrawPrimitives(d, graphicsDevice);
						}
					}
					//}
				}


				if (theBlob != null && !DEBUG_WireframeMode) {
					//graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;

					if (m_distorter != null && m_distortionMap != null) {

						m_distorter.CurrentTechnique =
								m_distorter.Techniques["PullIn"];
						m_distorter.Parameters["DistortionScale"].SetValue(0.1f);
						Random r = new Random();
						m_distorter.Parameters["Time"].SetValue(r.Next() / 10.0f);
						//EffectManager.getSingleton.GetEffect("Distorter").Parameters["Time"].SetValue(r.NextDouble() * 30.0);

						//graphicsDevice.SetRenderTarget(0, m_distortionMap);
						graphicsDevice.Clear(ClearOptions.Target, Color.Black, 0.0f, 0);

						//graphicsDevice.Textures[0] = theBlob.DisplacementText;

						m_distorter.Begin();
						foreach (EffectPass pass in m_distorter.CurrentTechnique.Passes) {
							pass.Begin();
							theBlob.DrawMe();
							pass.End();
						}
						m_distorter.End();
					}
				}


				if (m_SceneRenderTarget != null && !DEBUG_WireframeMode)
					ApplyPostProcessing(graphicsDevice, theBlob != null);

				graphicsDevice.RenderState.AlphaBlendEnable = true;

				if (theBlob != null && DEBUG_WireframeMode) {
					m_cartoonEffect.CurrentTechnique = m_cartoonEffect.Techniques["LambertOnBlob"];
					m_cartoonEffect.Parameters["Texture"].SetValue(theBlob.text);
					m_cartoonEffect.Begin();
					foreach (EffectPass pass in m_cartoonEffect.CurrentTechnique.Passes) {
						pass.Begin();
						theBlob.DrawMe();
						pass.End();
					}
					m_cartoonEffect.End();
				}

				if (DEBUG_WireframeMode) {
					renderState.CullMode = CullMode.CullClockwiseFace;
					renderState.FillMode = FillMode.Solid;
					renderState.DepthBufferEnable = true;
				}
			}
		}

		public void DrawPrimitives(Drawable d, GraphicsDevice graphicsDevice) {
			graphicsDevice.Vertices[0].SetSource(d.getVertexBuffer(), 0, d.getVertexStride());

			m_cartoonEffect.Begin();
			foreach (EffectPass pass in m_cartoonEffect.CurrentTechnique.Passes) {
				pass.Begin();
				d.DrawMe();
				pass.End();
			}
			m_cartoonEffect.End();
		}

		public void DrawModel(Matrix p_CurrentWorld, StaticModel d, GraphicsDevice graphicsDevice) {
			Stack<Matrix> drawStack = new Stack<Matrix>();
			Matrix currentWorld = p_CurrentWorld;

			if (d.Name == CurrentlySelected) {
				d.ShowVertices = true;
			} else {
				d.ShowVertices = false;
			}

			d.DrawMe(graphicsDevice, m_cartoonEffect, m_GameMode);
			m_cartoonEffect.Parameters[m_WorldParameterName].SetValue(currentWorld);
		}

		public void ApplyPostProcessing(GraphicsDevice graphicsDevice, bool blob) {
			graphicsDevice.SetRenderTarget(0, null);

			if (!blob) {
				m_cartoonEffect.Begin();
				m_cartoonEffect.CurrentTechnique.Passes[0].Begin();

				m_SpriteBatch.Begin(SpriteBlendMode.None,
							  SpriteSortMode.Immediate,
							  SaveStateMode.None);


				m_SpriteBatch.Draw(m_SceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

				m_SpriteBatch.End();

				m_cartoonEffect.CurrentTechnique.Passes[0].End();
				m_cartoonEffect.End();
			}

			if (m_postProcessEffect != null && m_tempTarget != null) {

				graphicsDevice.SetRenderTarget(0, null);

				// Crash: Invalid Operation Exception: The render target must not be set on the device when calling GetTexture.
				normalDepthTexture = m_NormalDepthRenderTarget.GetTexture();

				m_postProcessEffect.Parameters["EdgeWidth"].SetValue(1.0f);
				m_postProcessEffect.Parameters["EdgeIntensity"].SetValue(1.0f);
				m_postProcessEffect.Parameters["ScreenResolution"].SetValue(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
				m_postProcessEffect.Parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

				// Activate the appropriate effect technique.
				m_postProcessEffect.CurrentTechnique = m_postProcessEffect.Techniques["EdgeDetect"];


				m_SpriteBatch.Begin(SpriteBlendMode.None,
								  SpriteSortMode.Immediate,
								  SaveStateMode.None);

				m_postProcessEffect.Begin();
				m_postProcessEffect.CurrentTechnique.Passes[0].Begin();

				m_SpriteBatch.Draw(m_SceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

				m_SpriteBatch.End();

				m_postProcessEffect.CurrentTechnique.Passes[0].End();
				m_postProcessEffect.End();
			}

			if (m_distortionMap != null && blob) {
				graphicsDevice.ResolveBackBuffer(m_tempTarget);
				graphicsDevice.Textures[1] = m_distortionMap.GetTexture();
				//graphicsDevice.Textures[0] = m_tempTarget;
				m_distort.CurrentTechnique =
					m_distort.Techniques["DistortBlur"];

				m_SpriteBatch.Begin(SpriteBlendMode.None,
							  SpriteSortMode.Immediate,
							  SaveStateMode.None);

				m_distort.Begin();
				m_distort.CurrentTechnique.Passes[0].Begin();

				m_SpriteBatch.Draw(m_tempTarget, Vector2.Zero, Color.White);

				m_SpriteBatch.End();
				m_distort.CurrentTechnique.Passes[0].End();
				m_distort.End();
			}

		}

		#region Blur Calculation

		// Look up the sample weight and offset effect parameters.
		EffectParameter weightsParameter, offsetsParameter;

		int sampleCount;

		float[] sampleWeights;
		Vector2[] sampleOffsets;

		float totalWeights;

		float weight;

		float sampleOffset;

		Vector2 delta;

		/// <summary>
		/// Computes sample weightings and texture coordinate offsets
		/// for one pass of a separable gaussian blur filter.
		/// </summary>
		/// <remarks>
		/// This function was originally provided in the BloomComponent class in the 
		/// Bloom Postprocess sample.
		/// </remarks>
		public void SetBlurEffectParameters(float dx, float dy) {
			weightsParameter = m_distort.Parameters["SampleWeights"];
			offsetsParameter = m_distort.Parameters["SampleOffsets"];

			// Look up how many samples our gaussian blur effect supports.
			sampleCount = weightsParameter.Elements.Count;

			// Create temporary arrays for computing our filter settings.
			sampleWeights = new float[sampleCount];
			sampleOffsets = new Vector2[sampleCount];

			// The first sample always has a zero offset.
			sampleWeights[0] = ComputeGaussian(0);
			sampleOffsets[0] = new Vector2(0);

			// Maintain a sum of all the weighting values.
			totalWeights = sampleWeights[0];

			// Add pairs of additional sample taps, positioned
			// along a line in both directions from the center.
			for (int i = 0; i < sampleCount / 2; ++i) {
				// Store weights for the positive and negative taps.
				weight = ComputeGaussian(i + 1);

				sampleWeights[i * 2 + 1] = weight;
				sampleWeights[i * 2 + 2] = weight;

				totalWeights += weight * 2;

				// To get the maximum amount of blurring from a limited number of
				// pixel shader samples, we take advantage of the bilinear filtering
				// hardware inside the texture fetch unit. If we position our texture
				// coordinates exactly halfway between two texels, the filtering unit
				// will average them for us, giving two samples for the price of one.
				// This allows us to step in units of two texels per sample, rather
				// than just one at a time. The 1.5 offset kicks things off by
				// positioning us nicely in between two texels.
				sampleOffset = i * 2 + 1.5f;

				delta = new Vector2(dx, dy) * sampleOffset;

				// Store texture coordinate offsets for the positive and negative taps.
				sampleOffsets[i * 2 + 1] = delta;
				sampleOffsets[i * 2 + 2] = -delta;
			}

			// Normalize the list of sample weightings, so they will always sum to one.
			for (int i = 0; i < sampleWeights.Length; ++i) {
				sampleWeights[i] /= totalWeights;
			}

			// Tell the effect about our new filter settings.
			weightsParameter.SetValue(sampleWeights);
			offsetsParameter.SetValue(sampleOffsets);
		}

		/// <summary>
		/// Evaluates a single point on the gaussian falloff curve.
		/// Used for setting up the blur filter weightings.
		/// </summary>
		/// <remarks>
		/// This function was originally provided in the BloomComponent class in the 
		/// Bloom Postprocess sample.
		/// </remarks>
		private static float ComputeGaussian(float n) {
			return (float)((1.0 / Math.Sqrt(2 * Math.PI * blurAmount)) *
						   Math.Exp(-(n * n) / (2 * blurAmount * blurAmount)));
		}
		#endregion
	}
}
