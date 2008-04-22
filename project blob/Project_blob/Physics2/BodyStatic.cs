using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class BodyStatic : Body
	{

		private new List<CollidableStatic> collidables = new List<CollidableStatic>();

		private new List<BodyStatic> childBodies = null;

		public BodyStatic() { }

		public BodyStatic(List<CollidableStatic> Collidables, Body ParentBody)
			: base(ParentBody)
		{
			collidables = Collidables;
			foreach (CollidableStatic c in collidables)
			{
				boundingBox.expandToInclude(c.getBoundingBox());
			}
		}

		public override bool isStatic()
		{
			return true;
		}

		public override void update(float TotalElapsedSeconds)
		{ }
		public override void updatePosition()
		{ }
		internal override void SolveForNextPosition(float TotalElapsedSeconds) { }

	}
}
