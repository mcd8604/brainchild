using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    public abstract class Body
    {

        public void sumSpringForces()
        {
                foreach (Spring s in getSprings())
                {
                    s.A.Force += s.getForceVectorOnA();
                    s.B.Force += s.getForceVectorOnB();
                }
        }

        public abstract IEnumerable<Point> getPoints();

        public abstract IEnumerable<Collidable> getCollidables();

        public abstract IEnumerable<Spring> getSprings();

        public abstract float getVolume();

    }
}