using System.Collections.Generic;
namespace Physics2
{
	public class BodyRigid : Body
	{
		public BodyRigid() { }

        public BodyRigid(Body parentBody, string p_collisionSound) : base(parentBody, p_collisionSound) { }

        public BodyRigid(Body ParentBody, IList<PhysicsPoint> p_points, IList<Collidable> p_collidables, IList<Spring> p_springs, IList<Task> p_tasks, string p_collisionSound)
            : base(ParentBody, p_points, p_collidables, p_springs, p_tasks, p_collisionSound) { }
	}
}
