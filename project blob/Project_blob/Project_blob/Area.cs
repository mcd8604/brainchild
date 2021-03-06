using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Collections;
using Physics2;
using Audio;

namespace Project_blob {

	[Serializable]
	public class Area {
		[NonSerialized]
		private Display _display;

		private Dictionary<string, Drawable> _drawables;

		private List<AmbientSoundInfo> _ambientSounds = new List<AmbientSoundInfo>();
		public List<AmbientSoundInfo> AmbientSounds {
			get {
				return _ambientSounds;
			}
			set {
				_ambientSounds = value;
			}
		}

		private string _musicName = string.Empty;
		[TypeConverter(typeof(TypeConverterAudio))]
		public string MusicName
		{
			get { return _musicName; }
			set { _musicName = value; }
		}

		private float m_TimeLimit = -1f;
		public float TimeLimit {
			get { return m_TimeLimit; }
			set { m_TimeLimit = value; }
		}

		private Vector3 m_CameraSpawnPosition = new Vector3();
		public Vector3 CameraSpawnPosition {
			get { return m_CameraSpawnPosition; }
			set { m_CameraSpawnPosition = value; }
		}

		private List<Portal> _portals = new List<Portal>();
		public List<Portal> Portals {
			get {
				return _portals;
			}
			set {
				_portals = value;
			}
		}

		private bool m_ShowTime = true;
		public bool ShowTime {
			get { return m_ShowTime; }
			set { m_ShowTime = value; }
		}

		public Display Display {
			get { return _display; }
			set { _display = value; }
		}

		public Dictionary<string, Drawable> Drawables {
			get { return _drawables; }
			set { _drawables = value; }
		}

		private Vector3 m_StartPosition;
		public Vector3 StartPosition {
			get {
				return m_StartPosition;
			}
			set {
				m_StartPosition = value;
			}
		}

		private string m_SkyTexture;
		public string SkyTexture {
			get {
				return m_SkyTexture;
			}
			set {
				m_SkyTexture = value;
			}
		}

		private SceneGraphType _cullingStructure;
		public SceneGraphType CullingStructure {
			get { return _cullingStructure; }
			set { _cullingStructure = value; }
		}

		/// <summary>
		/// The Air Friction in this area.
		/// null = Don't change
		/// </summary>
		private float? m_AirFriction = null;
		public float? AirFriction
		{
			get
			{
				return m_AirFriction;
			}
			set
			{
				m_AirFriction = value;
			}
		}

		public Area(Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix) {

			//_display = new Display(worldMatrix, viewMatrix, projectionMatrix);
			_drawables = new Dictionary<string, Drawable>();
			_portals = new List<Portal>();
		}

		public Area(Matrix worldMatrix, string effectName, string worldParameterName, string textureParameterName, string techniqueName) {
			//_display = new Display(worldMatrix, effectName, worldParameterName, textureParameterName, techniqueName);
			_drawables = new Dictionary<string, Drawable>();
			_portals = new List<Portal>();
		}

		public Drawable GetDrawable(string drawableName) {
			if (_drawables.ContainsKey(drawableName)) {
				return _drawables[drawableName];
			}
			return null;
		}

		public List<Drawable> getDrawableList() {
			List<Drawable> drawableList = new List<Drawable>();
			/*IEnumerator drawablesEnum = this.Drawables.GetEnumerator();
			while (drawablesEnum.MoveNext())
			{
				KeyValuePair<string, Drawable> kvp = (KeyValuePair<string, Drawable>)drawablesEnum.Current;
				drawableList.Add((Drawable)kvp.Value);
			}*/
			foreach (Drawable d in this.Drawables.Values) {
				if (d is StaticModel) {
					if (((StaticModel)d).Visible) {
						drawableList.Add(d);
					}
				}
			}
			return drawableList;
		}

		public void RemoveDrawable(string drawableName) {
			if (_drawables.ContainsKey(drawableName)) {
				Drawable tempDrawable;
				tempDrawable = _drawables[drawableName];
				_drawables.Remove(drawableName);
				/*foreach (TextureInfo textureInfo in _display.DrawnList.Keys)
				{
					if (_display.DrawnList[textureInfo].Contains(tempDrawable))
					{
						_display.DrawnList[textureInfo].Remove(tempDrawable);
						return;
					}
                }*/
				foreach (List<Drawable> drawables in _display.DrawnList) {
					if (drawables.Contains(tempDrawable)) {
						drawables.Remove(tempDrawable);
						return;
					}
                }
                foreach (List<Drawable> drawables in _display.AlphaDrawnList)
                {
                    if (drawables.Contains(tempDrawable))
                    {
                        drawables.Remove(tempDrawable);
                        return;
                    }
                }
                foreach (List<Drawable> drawables in _display.AdditiveDrawnList)
                {
                    if (drawables.Contains(tempDrawable))
                    {
                        drawables.Remove(tempDrawable);
                        return;
                    }
                }
			}
		}

		public void AddDrawable(string drawableName, int textureID, Drawable drawable) {
			if (!_drawables.ContainsKey(drawableName)) {
				_drawables.Add(drawableName, drawable);
				/*if (!_display.DrawnList.ContainsKey(textureInfo))
				{
					_display.DrawnList.Add(textureInfo, new List<Drawable>());
				}*/
                AddDrawableToDisplay(drawable);
			}
		}

        private void AddDrawableToDisplay(Drawable d)
        {
            switch (d.BlendMode)
            {
                case BlendModes.Additive:
                    this.Display.AdditiveDrawnList[d.GetTextureID()].Add(d);
                    break;
                case BlendModes.Alpha:
                    this.Display.AlphaDrawnList[d.GetTextureID()].Add(d);
                    break;
                case BlendModes.None:
                    this.Display.DrawnList[d.GetTextureID()].Add(d);
                    break;
            }
        }

		[NonSerialized]
		private List<Physics2.Body> m_Bodies = new List<Physics2.Body>();
		public List<Physics2.Body> getBodies() {
			return this.m_Bodies;
		}

		public void LoadAreaWorldMaker(Game game) {
			//create new display 
			this.Display = new Display(game.GraphicsDevice);

			this.Display.TextureName = "point_text";
			this.Display.ShowAxis = true;
			this._display.GameMode = false;

			//initialize drawables and populate draw list
			foreach (Drawable d in this.Drawables.Values) {
				if (d is StaticModel) {
					((StaticModel)d).initialize();
					//((StaticModel)d).Visible = true;
                }
                d.Drawn = true;
                AddDrawableToDisplay(d);
            }

            // load skybox
            if (this.m_SkyTexture != null && this.m_SkyTexture.Length > 0)
            {
                Texture2D skyTex = game.Content.Load<Texture2D>(@"Textures\\" + this.m_SkyTexture);
                skyTex.Name = this.m_SkyTexture;
                TextureManager.AddTexture(skyTex);
                ModelManager.getSingleton.AddModel("skyBox", game.Content.Load<Model>(@"Models\\skySphere"));
                this._display.SkyBox = new StaticModel("sky", "skyBox", "none", this.m_SkyTexture, new List<short>());
                this._display.SkyBox.Scale = Matrix.CreateScale(750f);
                this._display.SkyBox.initialize();
            }



			//move events into their respective models
			/*IEnumerator eventsEnum = this._events.GetEnumerator();
			while (eventsEnum.MoveNext())
			{
				//KeyValuePair<string, Drawable> kvp = (KeyValuePair<string, Drawable>)eventsEnum.Current;
				drawableList.Add((Drawable)kvp.Value);
					((StaticModel)d).Visible = true;

			}*/

			/*foreach (KeyValuePair<string, EventTrigger> kvp in this._events)
			{
				if (this._drawables[kvp.Key] is StaticModel)
				{
					//((StaticModel)this._drawables[kvp.Key]).Visible = false;
					//((StaticModel)this._drawables[kvp.Key]).Event = kvp.Value;
				}
			}*/
			//this._events = null;
		}

		public void LoadAreaGameplay(Game game) {
			//List<TextureInfo> textureInfos = new List<TextureInfo>();

			//load level textures
			//TextureManager.getSingleton.AddTexture("cloudsky", game.Content.Load<Texture2D>(@"Textures\\cloudsky"));

			/*foreach (TextureInfo ti in this.Display.DrawnList.Keys)
			{
				TextureManager.getSingleton.AddTexture(ti.TextureName, game.Content.Load<Texture2D>(@"Textures\\" + ti.TextureName));
			}*/

			//temp code - adds SwitchEvent to button1
			/*StaticModel button1 = (StaticModel)this.Drawables["button1"];
			SwitchEvent se = new SwitchEvent();
			se.Models = new List<DynamicModel>();
			DynamicModel cauldron2 = (DynamicModel)this.Drawables["cauldron2"];
			cauldron2.Tasks[0].run = false;
			se.Models.Add(cauldron2);

			((StaticModel)button1).Event = se;*/

			this.m_Bodies = new List<Physics2.Body>();

			List<TriggerBody> switches = new List<TriggerBody>();

			//load level models
			foreach (Drawable d in this.Drawables.Values) {
				if (d is StaticModel) {
					StaticModel dm = (StaticModel)d;
					Model model = game.Content.Load<Model>(@"Models\\" + dm.ModelName);
					ModelManager.getSingleton.AddModel(dm.ModelName, model);
					Texture2D t = game.Content.Load<Texture2D>(@"Textures\\" + dm.TextureName);
					t.Name = dm.TextureName;
					TextureManager.AddTexture(t);
					dm.initialize();

					/*foreach (TextureInfo info in this.Display.DrawnList.Keys)
					{
						if (this.Display.DrawnList[info].Contains(dm))
						{
							dm.TextureKey = info;
						}
					}*/

					// generate collidables & physics body

					//dm.getVertexBuffer().GraphicsDevice.Vertices[0].SetSource(null, 0, 0);

					VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[dm.NumVertices];
					dm.getVertexBuffer().GetData<VertexPositionNormalTexture>(vertices);

					// Indices
					int[] indices;
					if (dm.getIndexBuffer().IndexElementSize == IndexElementSize.SixteenBits) {
						indices = new int[(dm.getIndexBuffer().SizeInBytes) >> 1];
						short[] temp = new short[(dm.getIndexBuffer().SizeInBytes) >> 1];
						dm.getIndexBuffer().GetData<short>(temp);
						for (int i = 0; i < temp.Length; ++i)
							indices[i] = temp[i];
					} else {
						indices = new int[(dm.getIndexBuffer().SizeInBytes) >> 2];
						dm.getIndexBuffer().GetData<int>(indices);
					}

					Physics2.Body body = null;

					BoundingBox boundingBox = new BoundingBox(Vector3.Zero, Vector3.Zero);

					if (!(dm is DynamicModel)) {

						List<Physics2.CollidableStatic> collidables = new List<Physics2.CollidableStatic>();

						if (dm.CollisionType == CollisionTypes.Body) {
							int numCol = 0;
							for (int i = 0; i < indices.Length; i += 3) {
								if (vertices[indices[i]].Position != vertices[indices[i + 1]].Position &&
									vertices[indices[i + 2]].Position != vertices[indices[i]].Position &&
									vertices[indices[i + 1]].Position != vertices[indices[i + 2]].Position) {
									collidables.Add(new Physics2.CollidableStaticTri(vertices[indices[i + 2]].Position,
										vertices[indices[i + 1]].Position, vertices[indices[i]].Position));
									++numCol;
								}
							}
						} else if (dm.CollisionType == CollisionTypes.BoundingBox) {
							AxisAlignedBoundingBox b = new AxisAlignedBoundingBox();
							foreach (VertexPositionNormalTexture v in vertices) {
								b.expandToInclude(v.Position);
							}
							//TODO: create 12 collidables from b 
						} else if (dm.CollisionType == CollisionTypes.None) {
							AxisAlignedBoundingBox b = new AxisAlignedBoundingBox();
							foreach (VertexPositionNormalTexture v in vertices) {
								b.expandToInclude(v.Position);
							}
							boundingBox = b.GetXNABoundingBox();
						}

						if (dm.Event != null) {
							body = new TriggerBody(collidables, null, dm.Event);
							if (((TriggerBody)body).TriggeredEvent is SwitchEvent) {
								switches.Add((TriggerBody)body);
							}
							body.collisionSound = Audio.AudioManager.getSound(dm.AudioName);
						} else if (dm is ConveyerBeltStatic) {
							body = new BodyStaticConveyerBelt(collidables, null, dm as ConveyerBeltStatic);
							body.collisionSound = Audio.AudioManager.getSound(dm.AudioName);
						} else if (dm is StaticModelSpeed) {
							body = new SpeedStatic(collidables, null, ((StaticModelSpeed)dm).Speed);
							body.collisionSound = Audio.AudioManager.getSound(dm.AudioName);
						} else {
							body = new Body(collidables, null);
							body.collisionSound = Audio.AudioManager.getSound(dm.AudioName);
						}
					} else {
						DynamicModel dynModel = (DynamicModel)dm;

						List<PhysicsPoint> tempList = new List<PhysicsPoint>();
						Dictionary<int, PhysicsPoint> pointMap = new Dictionary<int, PhysicsPoint>();
						for (int i = 0; i < vertices.Length; ++i) {
							PhysicsPoint newPoint = new PhysicsPoint(vertices[i].Position);
							PhysicsPoint mapPoint = null;
							bool exists = false;
							foreach (PhysicsPoint p in tempList) {
								if (p.ExternalPosition == newPoint.ExternalPosition) {
									exists = true;
									mapPoint = p;
									break;
								}
							}
							if (exists) {
								pointMap[i] = mapPoint;
							} else {
								tempList.Add(newPoint);
								pointMap[i] = newPoint;
							}
						}

						List<Spring> springs = new List<Spring>();
						List<PhysicsPoint> points = new List<PhysicsPoint>();

						if (dynModel.HasSprings) {
							foreach (PhysicsPoint tPoint in tempList) {
								foreach (PhysicsPoint p in points) {
									float springDist = Vector3.Distance(tPoint.ExternalPosition, p.ExternalPosition);
									if (springDist > 0) {
										springs.Add(new Spring(tPoint, p, Vector3.Distance(tPoint.ExternalPosition, p.ExternalPosition), 1f));
									}
								}
								points.Add(tPoint);
							}
						} else {
							points = new List<PhysicsPoint>(tempList);
						}

						List<Physics2.Collidable> collidables = new List<Physics2.Collidable>();
						int numCol = 0;
						for (int i = 0; i < indices.Length; i += 3) {
							collidables.Add(new Physics2.CollidableTri(pointMap[indices[i + 2]], pointMap[indices[i + 1]], pointMap[indices[i]]));
							++numCol;
						}

						if (dm.Event != null) {
							body = new TriggerBody(null, points, collidables, new List<Spring>(), dynModel.Tasks, dynModel.Event);
							if (((TriggerBody)body).TriggeredEvent is SwitchEvent) {
								switches.Add((TriggerBody)body);
							}
							body.collisionSound = Audio.AudioManager.getSound(dynModel.AudioName);
						} else if (dm is ConveyerBeltDynamic) {
							body = new BodyDynamicConveyorBelt(null, points, collidables, new List<Spring>(), dynModel.Tasks, dm as ConveyerBeltDynamic, pointMap);
							body.collisionSound = Audio.AudioManager.getSound(dm.AudioName);
						} else {
							body = new DrawableBody(null, points, collidables, springs, dynModel.Tasks, dynModel, pointMap);
							body.collisionSound = Audio.AudioManager.getSound(dynModel.AudioName);
						}
					}

					if (boundingBox.Min == Vector3.Zero && boundingBox.Max == Vector3.Zero) {
						dm.SetBoundingBox(body.getBoundingBox().GetXNABoundingBox());
					} else {
						dm.SetBoundingBox(boundingBox);
					}

					body.setMaterial(MaterialFactory.GetPhysicsMaterial(dm.MyMaterialType));
					this.m_Bodies.Add(body);
                }
            }
            //create new display 
            this._display = new Display(game.GraphicsDevice);
            this._display.ShowAxis = false;
            this._display.GameMode = true;

            // load skybox
            if (this.m_SkyTexture != null && this.m_SkyTexture.Length > 0)
            {
                Texture2D skyTex = game.Content.Load<Texture2D>(@"Textures\\" + this.m_SkyTexture);
                skyTex.Name = this.m_SkyTexture;
                TextureManager.AddTexture(skyTex);
                ModelManager.getSingleton.AddModel("skyBox", game.Content.Load<Model>(@"Models\\skySphere"));
                this._display.SkyBox = new StaticModel("sky", "skyBox", "none", this.m_SkyTexture, new List<short>());
                this._display.SkyBox.Scale = Matrix.CreateScale(750f);
                this._display.SkyBox.initialize();
            }

            foreach (Drawable d in this._drawables.Values)
            {
                if (!(d is StaticModel))
                {
                    AddDrawableToDisplay(d);
                }
                if (d is StaticModel && ((StaticModel)d).Visible)
                {
                    AddDrawableToDisplay(d);
                }
            }

			// These loops associate bodies with the SwitchEvent 
			// The bodies are needed to run their tasks when 
			// the switch is triggered.
			//
			// (I don't think we have enough loops) /sarcasm
			foreach (TriggerBody t in switches) {
				((SwitchEvent)t.TriggeredEvent).Bodies = new List<Body>();
				foreach (DynamicModel d in ((SwitchEvent)t.TriggeredEvent).Models) {
					foreach (Body b in m_Bodies) {
						if (b is DrawableBody) {
							if (((DrawableBody)b).getModel().Equals(d)) {
								((SwitchEvent)t.TriggeredEvent).Bodies.Add(b);
							}
						}
					}
				}
			}
		}
	}
}



//Texture Coord Stuff
//if (dm.ModelName.Equals("plane"))
//{
//	Texture2D texture = TextureManager.getSingleton.GetTexture(info.TextureName);

//	Vector3 scaleVector = Vector3.Zero;
//	Quaternion rotVector = Quaternion.Identity;
//	Vector3 transVector = Vector3.Zero;
//	dm.Scale.Decompose(out scaleVector, out rotVector, out transVector);

//    foreach (ModelMesh mesh in model.Meshes)
//    {
//        int numVertices = 0;
//        foreach (ModelMeshPart part in mesh.MeshParts)
//        {
//            numVertices += part.NumVertices;
//        }
//        VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[numVertices];
//        mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

//        /*int[] indices;
//        if (mesh.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
//        {
//            indices = new int[(mesh.IndexBuffer.SizeInBytes) >> 1];
//            short[] temp = new short[(mesh.IndexBuffer.SizeInBytes) >> 1];
//            mesh.IndexBuffer.GetData<short>(temp);
//            for (int i = 0; i < temp.Length; ++i)
//                indices[i] = temp[i];
//        }
//        else
//        {
//            indices = new int[(mesh.IndexBuffer.SizeInBytes) >> 2];
//            mesh.IndexBuffer.GetData<int>(indices);
//        }*/


//        //major problem noticed, this algorithm only works for one triangle
//        /*List<int> transTextCord = new List<int>();
//        for (int i = 0; i < indices.Length; i += 3)
//        {
//            //Plane plane = new Plane(vertices[indices[i]].Position, vertices[indices[i + 1]].Position, vertices[indices[i + 2]].Position);
//            //plane.Normal;

//            float distOriginal1 = Vector3.Distance(vertices[indices[i]].Position, vertices[indices[i + 1]].Position);
//            float distOriginal2 = Vector3.Distance(vertices[indices[i]].Position, vertices[indices[i + 2]].Position);
//            //float distOriginal3 = Vector3.Distance(vertices[indices[i + 2]].Position, vertices[indices[i]].Position);

//            float distTrans1 = Vector3.Distance(vertices[indices[i]].Position * scaleVector, vertices[indices[i + 1]].Position * scaleVector);
//            float distTrans2 = Vector3.Distance(vertices[indices[i]].Position * scaleVector, vertices[indices[i + 2]].Position * scaleVector);
//            //float distTrans3 = Vector3.Distance(vertices[indices[i + 2]].Position * scaleVector, vertices[indices[i]].Position * scaleVector);

//            //vertices[indices[i]].TextureCoordinate.X *= ((distTrans1 / distOriginal1) / (texture.Width * 0.5f));
//            //vertices[indices[i]].TextureCoordinate.Y *= ((distTrans1 / distOriginal1) / (texture.Height * 0.5f));

//            if(!transTextCord.Contains(indices[i + 1]))
//            {
//                vertices[indices[i + 1]].TextureCoordinate.X *= ((distTrans1 / distOriginal1));// / (texture.Width * 0.5f));
//                vertices[indices[i + 1]].TextureCoordinate.Y *= ((distTrans1 / distOriginal1));// / (texture.Height * 0.5f));
//                transTextCord.Add(indices[i + 1]);
//            }

//            if(!transTextCord.Contains(indices[i + 2]))
//            {
//                vertices[indices[i + 2]].TextureCoordinate.X *= ((distTrans2 / distOriginal2));// / (texture.Width * 0.5f));
//                vertices[indices[i + 2]].TextureCoordinate.Y *= ((distTrans2 / distOriginal2));// / (texture.Height * 0.5f));
//                transTextCord.Add(indices[i + 2]);
//            }
//        }*/

//        /*Vector3[] points = new Vector3[numVertices];
//        for (int i = 0; i < vertices.Length; ++i)
//        {
//            points[i] = vertices[i].Position;
//        }
//        BoundingBox boundingBox = BoundingBox.CreateFromPoints(points);*/

//        /*for (int i = 0; i < vertices.Length; ++i)
//        {
//            Vector3 scaleVector = Vector3.Zero;
//            Quaternion rotVector = Quaternion.Identity;
//            Vector3 transVector = Vector3.Zero;
//            dm.Scale.Decompose(out scaleVector, out rotVector, out transVector);
//            vertices[i].TextureCoordinate.X *= (scaleVector.X / (texture.Width * 0.5f));
//            vertices[i].TextureCoordinate.Y *= (scaleVector.Z / (texture.Height * 0.5f));
//        }*/
//        mesh.VertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
//    //}
//}
