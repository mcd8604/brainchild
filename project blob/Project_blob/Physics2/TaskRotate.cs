using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics2
{
	[Serializable]
    class TaskRotate : Task
    {
        private Vector3 axis = Vector3.Up;
        public Vector3 Axis
        {
            get
            {
                return axis;
            }
            set
            {
                axis = value;
            }
        }

        private float angle = MathHelper.ToRadians(1f);
        public float Degrees
        {
            get
            {
				return MathHelper.ToDegrees(angle);
            }
            set
			{
				angle = MathHelper.ToRadians(value);
            }
        }

		private Quaternion rotate;

		public TaskRotate() { }

        public TaskRotate( Vector3 rotateAxis, float rotateDegrees )
        {
			axis = rotateAxis;
            angle = MathHelper.ToRadians(rotateDegrees);
        }

        public override void update( Body b, float time )
        {
			rotate = Quaternion.CreateFromAxisAngle(b.getCenter() + axis, angle * time);
            foreach ( PhysicsPoint p in b.getPoints() )
            {
				p.PotentialPosition = Vector3.Transform(p.CurrentPosition, rotate);
            }
        }
    }
}
