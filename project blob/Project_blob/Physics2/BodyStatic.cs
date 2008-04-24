using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class BodyStatic : Body
	{

		private new List<CollidableStatic> collidables = new List<CollidableStatic>();

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

		public override void addChild(Body childBody)
		{
			if (!(childBody is BodyStatic))
			{
				throw new Exception("BodyStatic child must also be static");
			}
			base.addChild(childBody);
		}

		public override bool isStatic()
		{
			return true;
		}

		public override Vector3 getRelativeVelocity(Point p)
		{
			return base.getRelativeVelocity(p);
		}

		public override void update(float TotalElapsedSeconds)
		{ }
		public override void updatePosition()
		{ }
		internal override void SolveForNextPosition(float TotalElapsedSeconds) { }

	}
}
