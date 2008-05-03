using System;
using System.Collections.Generic;
using System.Text;
using Physics2;

namespace Project_blob
{
    public abstract class DrawableBody : Physics2.Body
    {

        private StaticModel m_DrawableModel;

        public DrawableBody(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks, StaticModel p_Model)
			:base(ParentBody,p_points,p_collidables,p_springs,p_tasks)
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
