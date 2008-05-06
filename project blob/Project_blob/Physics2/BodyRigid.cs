using System.Collections.Generic;
namespace Physics2
{
	public class BodyRigid : Body
	{
        public BodyRigid() { }

        public BodyRigid(Body parentBody) : base(parentBody) { }

        public BodyRigid(Body ParentBody, List<PhysicsPoint> p_points, List<Collidable> p_collidables, List<Spring> p_springs, List<Task> p_tasks)
            : base(ParentBody, p_points, p_collidables, p_springs, p_tasks) { }
	}
}
