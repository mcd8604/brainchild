using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Body : Actor
	{

		public List<Point> points = new List<Point>();
		public List<Collidable> collidables = new List<Collidable>();
		public List<Spring> springs = new List<Spring>();
		public List<Task> tasks = new List<Task>();

		public Body parentBody = null;
		public List<Body> childBodies = null;

		public AxisAlignedBoundingBox boundingBox = new AxisAlignedBoundingBox();

		public Vector3 center;

		public Body() { }

		public Body(Body ParentBody)
		{
			ParentBody.addChild(this);
		}

		public void addChild(Body childBody)
		{
			if (childBody.parentBody != null)
			{
				throw new Exception();
			}
			if (childBodies == null)
			{
				childBodies = new List<Body>();
			}
			childBody.parentBody = this;
			childBodies.Add(childBody);
		}

		public virtual AxisAlignedBoundingBox getBoundingBox()
		{
			return boundingBox;
		}

		public virtual Vector3 getCenter()
		{
			return center;
		}

		public virtual Vector3 getpotentialCenter()
		{
			return center;
		}

		public virtual IEnumerable<Collidable> getCollidables()
		{
			return collidables;
		}

		public virtual IEnumerable<Point> getPoints()
		{
			return points;
		}

		public virtual IEnumerable<Spring> getSprings()
		{
			return springs;
		}

		public virtual void update(float TotalElapsedSeconds)
		{

			foreach (Body b in childBodies)
			{
				b.update(TotalElapsedSeconds);
			}

			// Predict potential position
			foreach (Point p in points)
			{
				p.PotentialPosition = p.CurrentPosition + (p.CurrentVelocity * TotalElapsedSeconds);
			}

			// Springs
			foreach (Spring s in springs)
			{
				s.update();
			}

			//Tasks
			foreach (Task t in tasks)
			{
				t.update(this);
			}

		}

		public virtual void updatePosition()
		{

			boundingBox.clear();
			foreach (Body child in childBodies)
			{
				child.updatePosition();
				boundingBox.expandToInclude(child.getBoundingBox());
			}

			center = Vector3.Zero;
			foreach (Point p in points)
			{
				p.updatePosition();
				boundingBox.expandToInclude(p.CurrentPosition);
				center += p.CurrentPosition;
			}
			center /= points.Count;

		}

        internal void SolveForNextPosition(float TotalElapsedSeconds)
        {

            foreach (Body child in childBodies)
            {
                SolveForNextPosition(TotalElapsedSeconds);
            }

            foreach (Point p in points)
            {
                p.PotentialAcceleration = p.CurrentAcceleration + (p.ForceThisFrame / p.Mass);
                p.PotentialVelocity = p.CurrentVelocity + (p.PotentialAcceleration * TotalElapsedSeconds);

                if (p.PotentialVelocity != Vector3.Zero)
                {
                    //Vector3 DragForce = p.PotentialVelocity * airfriction;
                    Vector3 DragForce = p.PotentialVelocity * 1;
                    Vector3 AccelerationDrag = (DragForce / p.Mass);
                    Vector3 VelocityDrag = (AccelerationDrag * TotalElapsedSeconds);

                    if ((p.PotentialVelocity.X > 0 && p.PotentialVelocity.X - VelocityDrag.X <= 0) ||
                        (p.PotentialVelocity.X < 0 && p.PotentialVelocity.X - VelocityDrag.X >= 0))
                    {
                        p.PotentialVelocity.X = 0;
                        VelocityDrag.X = 0;
                    }
                    if ((p.PotentialVelocity.Y > 0 && p.PotentialVelocity.Y - VelocityDrag.Y <= 0) ||
                        (p.PotentialVelocity.Y < 0 && p.PotentialVelocity.Y - VelocityDrag.Y >= 0))
                    {
                        p.PotentialVelocity.Y = 0;
                        VelocityDrag.Y = 0;
                    }
                    if ((p.PotentialVelocity.Z > 0 && p.PotentialVelocity.Z - VelocityDrag.Z <= 0) ||
                        (p.PotentialVelocity.Z < 0 && p.PotentialVelocity.Z - VelocityDrag.Z >= 0))
                    {
                        p.PotentialVelocity.Z = 0;
                        VelocityDrag.Z = 0;
                    }
                    p.PotentialVelocity -= VelocityDrag;
                }

                p.PotentialPosition = p.CurrentPosition + (p.PotentialVelocity * TotalElapsedSeconds);
            }

        }

	}
}
