using System;
using System.Collections.Generic;
using System.Text;

namespace WorldMakerDemo.Level
{
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

        public Area()
        {
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

        public void RemoveDrawable(String drawableName, TextureInfo textureInfo)
        {
            if (_drawables.ContainsKey(drawableName))
            {
                Drawable tempDrawable;
                tempDrawable = _drawables[drawableName];
                _drawables.Remove(drawableName);
                _display.DrawnList[textureInfo].Remove(tempDrawable); 
            }
        }

        public void AddDrawable(String drawableName, TextureInfo textureInfo, Drawable drawable)
        {
            _drawables.Add(drawableName, drawable);
            _display.DrawnList[textureInfo].Add(drawable); 
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
