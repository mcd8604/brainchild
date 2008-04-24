using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Physics;

namespace Project_blob
{
    [Serializable]
    public class Area
    {
        private Display _display;

        private Dictionary<String, Drawable> _drawables;

        private Dictionary<String, EventTrigger> _events;

        //private Dictionary<String, Collidable> _collidables;

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

        public Dictionary<String, EventTrigger> Events {
            get { return _events; }
            set { _events = value; }
        }

        //public Dictionary<String, Collidable> Collidables
        //{
        //    get { return _collidables; }
        //    set { _collidables = value; }
        //}

        public Area(Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            _display = new Display(worldMatrix, viewMatrix, projectionMatrix);
            _drawables = new Dictionary<String, Drawable>();
            _events = new Dictionary<String, EventTrigger>();

            //_collidables = new Dictionary<String, Collidable>();
        }

        public Area(Matrix worldMatrix, String effectName, String worldParameterName, String textureParameterName, String techniqueName)
        {
            _display = new Display(worldMatrix, effectName, worldParameterName, textureParameterName, techniqueName);
            _drawables = new Dictionary<String, Drawable>();
            _events = new Dictionary<String, EventTrigger>();

            //_collidables = new Dictionary<String, Collidable>();
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

        public EventTrigger GetEvent(String eventName) {
            if(_events.ContainsKey(eventName)) {
                return _events[eventName];
            }
            return null;
        }

        public void RemoveEvent(String eventName) {
            if(_drawables.ContainsKey(eventName)) {
                EventTrigger tempEvent;
                tempEvent = _events[eventName];
                _events.Remove(eventName);
            }
        }

        public void AddEvent(String eventName, EventTrigger eventTrigger) {
            if(_events == null) {
                _events = new Dictionary<string, EventTrigger>();
            }
            if(!_events.ContainsKey(eventName)) {
                _events.Add(eventName, eventTrigger);
            }
        }

        //public Collidable GetCollidable(String collidableName)
        //{
        //    if (_collidables.ContainsKey(collidableName))
        //    {
        //        return _collidables[collidableName];
        //    }
        //    return null;
        //}

        //public void RemoveCollidable(String collidableName)
        //{
        //    if (_collidables.ContainsKey(collidableName))
        //    {
        //        _collidables.Remove(collidableName);
        //    }
        //}

        //public void AddCollidable(String collidableName, Collidable collidable)
        //{
        //    _collidables.Add(collidableName, collidable);
        //}

        [NonSerialized]
        private List<Physics.Collidable> _collidables = new List<Physics.Collidable>();
        public List<Physics.Collidable> getCollidables()
        {
            return this._collidables;
        }

        public void LoadAreaGameplay(Game game)
        {
            this._display.ShowAxis = false;
            this._display.GameMode = true;

            //Give the SceneManager a reference to the display
            SceneManager.getSingleton.Display = this._display;

            this._collidables = new List<Physics.Collidable>();
            
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
                    //TextureManager.getSingleton.AddTexture(dm.TextureName, Content.Load<Texture2D>(@"Textures\\" + dm.TextureName));
                    //textureInfos.Add(new TextureInfo(dm.TextureName, i++));
                    //Collidables

                    //physics.AddCollidableBox(dm.GetBoundingBox(), dm.createCollidables(model));
                    foreach (TextureInfo info in this.Display.DrawnList.Keys)
                    {
                        if (this.Display.DrawnList[info].Contains(dm))
                        {
                            dm.TextureKey = info;

                            //Texture Coord Stuff
                            //if (dm.ModelName.Equals("plane"))
                            //{
                                Texture2D texture = TextureManager.getSingleton.GetTexture(info.TextureName);

                                Vector3 scaleVector = Vector3.Zero;
                                Quaternion rotVector = Quaternion.Identity;
                                Vector3 transVector = Vector3.Zero;
                                dm.Scale.Decompose(out scaleVector, out rotVector, out transVector);

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

                    List<Physics.Collidable> colls = dm.createCollidables(this);

                    this._collidables.AddRange(colls);

                    //temporary material stuff
                    foreach (Collidable c in colls)
                    {
                        Physics.Material m;
                        if (dm.TextureKey.TextureName.Equals("sticky"))
                        {
                            m = new Physics.MaterialCustom(50f, 5f);
                        }
                        else if (dm.TextureKey.TextureName.Equals("slick"))
                        {
                            m = new Physics.MaterialCustom(0f);
                        }
                        else
                        {
                            m = Physics.Material.getDefaultMaterial();
                        }
                        if (c is CollidableTri)
                        {
                            ((CollidableTri)c).setMaterial(m);
                        }
                    }
                }
            }
        }
    }
}
