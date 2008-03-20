using System;
using System.Collections.Generic;
using System.Text;

namespace WorldMakerDemo.Level
{
    public class Area
    {
        private List<DrawableModel> _drawables;

        //private List<Collidable> _collidables;

        public List<DrawableModel> Drawables
        {
            get { return _drawables; }
            set { _drawables = value; }
        }

        //public List<Collidable> Collidables
        //{
        //    get { return _collidables; }
        //    set { _collidables = value; }
        //}

        public Area()
        {
            _drawables = new List<DrawableModel>();

            //_collidables = new List<Collidable>();
        }
    }
}
