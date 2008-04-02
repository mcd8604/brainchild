using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    }
}
