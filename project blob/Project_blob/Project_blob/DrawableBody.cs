using System;
using System.Collections.Generic;
using System.Text;

namespace Project_blob
{
    public abstract class DrawableBody : Physics2.Body
    {

        private StaticModel m_DrawableModel;

        public DrawableBody(StaticModel p_Model)
        {
            m_DrawableModel = p_Model;
        }

        /*public override Vector3 getCenter(){}

        public override Vector3 getNextCenter(){}

        public override IEnumerable<Point> getPoints(){}

        public override IEnumerable<Collidable> getCollidables(){}

        public override IEnumerable<Spring> getSprings(){}*/

    }
}
