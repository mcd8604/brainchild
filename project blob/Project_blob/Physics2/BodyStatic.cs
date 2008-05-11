using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class BodyStatic : Body
	{

		private IList<CollidableStatic> staticCollidables = new List<CollidableStatic>();

		public BodyStatic(IList<CollidableStatic> Collidables, Body ParentBody, string p_collisionSound)
			: base(p_collisionSound)
		{
			if (ParentBody != null)
			{
				ParentBody.addChild(this);
			}
			staticCollidables = Collidables;

			boundingBox = new AxisAlignedBoundingBox();
			foreach (Collidable c in staticCollidables)
			{
				collidables.Add(c);
				if (c.parent != null && c.parent != this)
				{
					throw new Exception();
				}
				c.parent = this;
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

        public override void update(float TotalElapsedSeconds) { base.update(TotalElapsedSeconds); }
		public override void updatePosition() { }
		protected internal override void SolveForNextPosition(float TotalElapsedSeconds) { }

	}
}
