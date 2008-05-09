using System.Collections.Generic;
namespace Physics2
{
	public class BodyRigid : Body
	{
		public BodyRigid() { }

		public BodyRigid(Body parentBody) : base(parentBody) { }

		public BodyRigid(Body ParentBody, IList<PhysicsPoint> p_points, IList<Collidable> p_collidables, IList<Spring> p_springs, IList<Task> p_tasks)
			: base(ParentBody, p_points, p_collidables, p_springs, p_tasks) { }
	}
}
