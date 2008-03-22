using System;
using System.Collections.Generic;
using System.Text;

namespace WorldMakerDemo.Level
{
    public class Area
    {
        private Dictionary<String, DrawableModel> _drawables;

        //private Dictionary<String, Collidable> _collidables;

        public Dictionary<String, DrawableModel> Drawables
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
            _drawables = new Dictionary<String, DrawableModel>();

            //_collidables = new Dictionary<String, Collidable>();
        }

        public DrawableModel GetDrawable(String drawableName)
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
                _drawables.Remove(drawableName);
            }
        }

        public void AddDrawable(String drawableName, DrawableModel drawable)
        {
            _drawables.Add(drawableName, drawable);
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
