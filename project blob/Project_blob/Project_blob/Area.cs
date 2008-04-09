using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace Project_blob
{
    [Serializable]
    public class Area
    {
        private Display _display;

        private Dictionary<String, Drawable> _drawables;

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

        //public Dictionary<String, Collidable> Collidables
        //{
        //    get { return _collidables; }
        //    set { _collidables = value; }
        //}

        public Area(Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            _display = new Display(worldMatrix, viewMatrix, projectionMatrix);
            _drawables = new Dictionary<String, Drawable>();

            //_collidables = new Dictionary<String, Collidable>();
        }

        public Area(Matrix worldMatrix, String effectName, String worldParameterName, String textureParameterName, String techniqueName)
        {
            _display = new Display(worldMatrix, effectName, worldParameterName, textureParameterName, techniqueName);
            _drawables = new Dictionary<String, Drawable>();

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

            //load level models and textures
            IEnumerator drawablesEnum = this.Drawables.GetEnumerator();
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
                        }
                    }

                    List<Physics.Collidable> colls = dm.createCollidables(model);

                    this._collidables.AddRange(colls);

                    //temporary material stuff
                    foreach (CollidableTri c in colls)
                    {
                        if (dm.TextureKey.TextureName.Equals("sticky"))
                        {
                            c.setMaterial(new Physics.MaterialCustom(50f, 5f));
                        }
                    }
                }
            }

            //change to level list, rather than drawn
            foreach (TextureInfo ti in this.Display.DrawnList.Keys)
            {
                TextureManager.getSingleton.AddTexture(ti.TextureName, game.Content.Load<Texture2D>(@"Textures\\" + ti.TextureName));
            }
        }
    }
}
