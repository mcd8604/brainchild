using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Physics2;

namespace Project_blob
{
	[Serializable]
	public class Area
	{
		private Display _display;

		private Dictionary<String, Drawable> _drawables;

		private Dictionary<String, EventTrigger> _events;

		public Display Display
		{
			get { return _display; }
			set { _display = value; }
		}

		public Dictionary<String, Drawable> Drawables
		{
			get { return _drawables; }
			set { _drawables = value; }
		}

		public Dictionary<String, EventTrigger> Events
		{
			get { return _events; }
			set { _events = value; }
		}

		public Area(Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
		{
			_display = new Display(worldMatrix, viewMatrix, projectionMatrix);
			_drawables = new Dictionary<String, Drawable>();
			_events = new Dictionary<String, EventTrigger>();
		}

		public Area(Matrix worldMatrix, String effectName, String worldParameterName, String textureParameterName, String techniqueName)
		{
			_display = new Display(worldMatrix, effectName, worldParameterName, textureParameterName, techniqueName);
			_drawables = new Dictionary<String, Drawable>();
			_events = new Dictionary<String, EventTrigger>();
		}

		public Drawable GetDrawable(String drawableName)
		{
			if (_drawables.ContainsKey(drawableName))
			{
				return _drawables[drawableName];
			}
			return null;
		}

		public List<Drawable> getDrawableList()
		{
			List<Drawable> drawableList = new List<Drawable>();
			IEnumerator drawablesEnum = this.Drawables.GetEnumerator();
			while (drawablesEnum.MoveNext())
			{
				KeyValuePair<String, Drawable> kvp = (KeyValuePair<String, Drawable>)drawablesEnum.Current;
				drawableList.Add((Drawable)kvp.Value);
			}
			return drawableList;
		}

		public void RemoveDrawable(String drawableName)
		{
			if (_drawables.ContainsKey(drawableName))
			{
				Drawable tempDrawable;
				tempDrawable = _drawables[drawableName];
				_drawables.Remove(drawableName);
				foreach (TextureInfo textureInfo in _display.DrawnList.Keys)
				{
					if (_display.DrawnList[textureInfo].Contains(tempDrawable))
					{
						_display.DrawnList[textureInfo].Remove(tempDrawable);
						return;
					}
				}
			}
		}

		public void AddDrawable(String drawableName, TextureInfo textureInfo, Drawable drawable)
		{
			if (!_drawables.ContainsKey(drawableName))
			{
				_drawables.Add(drawableName, drawable);
				if (!_display.DrawnList.ContainsKey(textureInfo))
				{
					_display.DrawnList.Add(textureInfo, new List<Drawable>());
				}
				_display.DrawnList[textureInfo].Add(drawable);
			}
		}

		public EventTrigger GetEvent(String eventName)
		{
			if (_events.ContainsKey(eventName))
			{
				return _events[eventName];
			}
			return null;
		}

		public void RemoveEvent(String eventName)
		{
			if (_drawables.ContainsKey(eventName))
			{
				EventTrigger tempEvent;
				tempEvent = _events[eventName];
				_events.Remove(eventName);
			}
		}

		public void AddEvent(String eventName, EventTrigger eventTrigger)
		{
			if (_events == null)
			{
				_events = new Dictionary<string, EventTrigger>();
			}
			if (!_events.ContainsKey(eventName))
			{
				_events.Add(eventName, eventTrigger);
			}
		}

		[NonSerialized]
		private List<Physics2.Body> m_Bodies = new List<Physics2.Body>();
		public List<Physics2.Body> getBodies()
		{
			return this.m_Bodies;
		}

		public void LoadAreaGameplay(Game game)
		{
			this._display.ShowAxis = false;
			this._display.GameMode = true;

			//Give the SceneManager a reference to the display
			SceneManager.getSingleton.Display = this._display;

			this.m_Bodies = new List<Physics2.Body>();

			List<TextureInfo> textureInfos = new List<TextureInfo>();

			//load level textures
			//TODO: change to level list, rather than drawn
			TextureManager.getSingleton.AddTexture("cloudsky", game.Content.Load<Texture2D>(@"Textures\\cloudsky"));
			foreach (TextureInfo ti in this.Display.DrawnList.Keys)
			{
				TextureManager.getSingleton.AddTexture(ti.TextureName, game.Content.Load<Texture2D>(@"Textures\\" + ti.TextureName));
			}

			//load level models
			IEnumerator drawablesEnum = this.Drawables.GetEnumerator();
			ModelManager.getSingleton.AddModel("skyBox", game.Content.Load<Model>(@"Models\\skySphere"));
			while (drawablesEnum.MoveNext())
			{
				KeyValuePair<String, Drawable> kvp = (KeyValuePair<String, Drawable>)drawablesEnum.Current;
				Drawable d = (Drawable)kvp.Value;
				if (d is StaticModel)
				{
					StaticModel dm = (StaticModel)d;
					Model model = game.Content.Load<Model>(@"Models\\" + dm.ModelName);
					ModelManager.getSingleton.AddModel(dm.ModelName, model);
					dm.initialize();

					foreach (TextureInfo info in this.Display.DrawnList.Keys)
					{
						if (this.Display.DrawnList[info].Contains(dm))
						{
							dm.TextureKey = info;

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
							//            indices = new int[(mesh.IndexBuffer.SizeInBytes) * 8 / 16];
							//            short[] temp = new short[(mesh.IndexBuffer.SizeInBytes) * 8 / 16];
							//            mesh.IndexBuffer.GetData<short>(temp);
							//            for (int i = 0; i < temp.Length; i++)
							//                indices[i] = temp[i];
							//        }
							//        else
							//        {
							//            indices = new int[(mesh.IndexBuffer.SizeInBytes) * 8 / 32];
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

							//            //vertices[indices[i]].TextureCoordinate.X *= ((distTrans1 / distOriginal1) / (texture.Width / 2f));
							//            //vertices[indices[i]].TextureCoordinate.Y *= ((distTrans1 / distOriginal1) / (texture.Height / 2f));

							//            if(!transTextCord.Contains(indices[i + 1]))
							//            {
							//                vertices[indices[i + 1]].TextureCoordinate.X *= ((distTrans1 / distOriginal1));// / (texture.Width / 2f));
							//                vertices[indices[i + 1]].TextureCoordinate.Y *= ((distTrans1 / distOriginal1));// / (texture.Height / 2f));
							//                transTextCord.Add(indices[i + 1]);
							//            }

							//            if(!transTextCord.Contains(indices[i + 2]))
							//            {
							//                vertices[indices[i + 2]].TextureCoordinate.X *= ((distTrans2 / distOriginal2));// / (texture.Width / 2f));
							//                vertices[indices[i + 2]].TextureCoordinate.Y *= ((distTrans2 / distOriginal2));// / (texture.Height / 2f));
							//                transTextCord.Add(indices[i + 2]);
							//            }
							//        }*/

							//        /*Vector3[] points = new Vector3[numVertices];
							//        for (int i = 0; i < vertices.Length; i++)
							//        {
							//            points[i] = vertices[i].Position;
							//        }
							//        BoundingBox boundingBox = BoundingBox.CreateFromPoints(points);*/

							//        /*for (int i = 0; i < vertices.Length; i++)
							//        {
							//            Vector3 scaleVector = Vector3.Zero;
							//            Quaternion rotVector = Quaternion.Identity;
							//            Vector3 transVector = Vector3.Zero;
							//            dm.Scale.Decompose(out scaleVector, out rotVector, out transVector);
							//            vertices[i].TextureCoordinate.X *= (scaleVector.X / (texture.Width / 2f));
							//            vertices[i].TextureCoordinate.Y *= (scaleVector.Z / (texture.Height / 2f));
							//        }*/
							//        mesh.VertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
							//    //}
							//}
						}
					}

                    // generate collidables & physics body

                    VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[dm.NumVertices];
                    dm.getVertexBuffer().GetData<VertexPositionNormalTexture>(vertices);

                    for (int i = 0; i < vertices.Length; i++)
                    {
                        //transform points to correct position
                        vertices[i].Position = Vector3.Transform(vertices[i].Position, dm.Transform);
                        vertices[i].Normal = Vector3.TransformNormal(vertices[i].Normal, dm.Transform);
                    }

                    // Indices
                    int[] indices;
                    if (dm.getIndexBuffer().IndexElementSize == IndexElementSize.SixteenBits)
                    {
                        indices = new int[(dm.getIndexBuffer().SizeInBytes) * 8 / 16];
                        short[] temp = new short[(dm.getIndexBuffer().SizeInBytes) * 8 / 16];
                        dm.getIndexBuffer().GetData<short>(temp);
                        for (int i = 0; i < temp.Length; i++)
                            indices[i] = temp[i];
                    }
                    else
                    {
                        indices = new int[(dm.getIndexBuffer().SizeInBytes) * 8 / 32];
                        dm.getIndexBuffer().GetData<int>(indices);
                    }

                    Physics2.Body body = null;

                    //If a StaticModel has no tasks, use a BodyStatic
                    //If it has tasks, DrawableBody
                    if (dm.tasks == null || dm.tasks.Count == 0)
                    {
                        List<Physics2.CollidableStatic> collidables = new List<Physics2.CollidableStatic>();
                        int numCol = 0;
                        for (int i = 0; i < indices.Length; i += 3)
                        {
                            if (vertices[indices[i]].Position != vertices[indices[i + 1]].Position && vertices[indices[i + 2]].Position != vertices[indices[i]].Position && vertices[indices[i + 1]].Position != vertices[indices[i + 2]].Position)
                            {
                                if (dm.TextureKey.TextureName.Equals("event"))
                                {
                                    //collidables.Add(new Trigger(vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]], areaRef.Events[Name]));
                                }
                                else
                                {
                                    collidables.Add(new Physics2.CollidableStaticTri(vertices[indices[i + 2]].Position, vertices[indices[i + 1]].Position, vertices[indices[i]].Position));
                                }
                                numCol++;
                            }
                        }

                        body = new BodyStatic(collidables, null);
                    }
                    else
                    {
                        List<PhysicsPoint> points = new List<PhysicsPoint>();
                        for (int i = 0; i < vertices.Length; i++)
                        {
                            points.Add(new PhysicsPoint(vertices[i].Position, null));
                        }

                        List<Physics2.Collidable> collidables = new List<Physics2.Collidable>();
                        int numCol = 0;
                        for (int i = 0; i < indices.Length; i += 3)
                        {
                            if (points[indices[i]].ExternalPosition != points[indices[i + 1]].ExternalPosition && points[indices[i + 2]].ExternalPosition != points[indices[i]].ExternalPosition && points[indices[i + 1]].ExternalPosition != points[indices[i + 2]].ExternalPosition)
                            {
                                collidables.Add(new Physics2.CollidableTri(points[indices[i + 2]], points[indices[i + 1]], points[indices[i]]));
                                numCol++;
                            }
                        }
                        body = new DrawableBody(null, points, collidables, null, dm.tasks, dm);
                    }
                    dm.SetBoundingBox(body.boundingBox.GetXNABoundingBox());

					Material sticky = new Material(10f, 10f);
					Material slick = new Material(0f, 0f);
					Material def = Material.getDefaultMaterial();

					//temporary material stuff
					Material m;
					if (dm.TextureKey.TextureName.Equals("sticky"))
					{
						m = sticky;
					}
					else if (dm.TextureKey.TextureName.Equals("slick"))
					{
						m = slick;
					}
					else
					{
						m = def;
					}

					body.setMaterial(m);
					this.m_Bodies.Add(body); 
				}
			}
		}
	}
}
