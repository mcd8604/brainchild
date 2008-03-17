using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicsDemo5
{
    class World : Physics.Body
    {

        public List<Physics.Point> points = new List<Physics.Point>();
        public List<Physics.Spring> springs = new List<Physics.Spring>();
        public List<T> collidables = new List<T>();

        public override IEnumerable<Physics.Point> getPoints()
        {
            return points;
        }

        public override IEnumerable<Physics.Collidable> getCollidables()
        {
            List<Physics.Collidable> temp = new List<Physics.Collidable>();
            foreach (T t in collidables)
            {
                temp.Add(t as Physics.Collidable);
            }
            return temp;
        }

        public IEnumerable<Drawable> getDrawables()
        {
            List<Drawable> temp = new List<Drawable>();
            foreach (T t in collidables)
            {
                temp.Add(t as Drawable);
            }
            return temp;
        }

        public override IEnumerable<Physics.Spring> getSprings()
        {
            return springs;
        }

        public override float getVolume()
        {
            // TODO
            return 1;
        }

    }
}
