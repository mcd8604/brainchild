using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class BodyStatic : Body
	{

        public new List<CollidableStatic> collidables = new List<CollidableStatic>();

        public new List<BodyStatic> childBodies = null;

        public new AxisAlignedBoundingBox boundingBox = new AxisAlignedBoundingBox();

        public BodyStatic() { }

        public BodyStatic(Body ParentBody)
            :base(ParentBody)
        {}

        public override void update(float TotalElapsedSeconds)
        {}
        public override void updatePosition()
        { }

	}
}
