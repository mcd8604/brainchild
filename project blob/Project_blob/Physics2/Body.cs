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

		public virtual void addChild(Body childBody)
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

		public virtual bool isStatic()
		{
			return false;
		}

		public virtual AxisAlignedBoundingBox getBoundingBox()
		{
			return boundingBox;
		}

		public virtual Vector3 getCenter()
		{
			return center;
		}

		public virtual Vector3 getPotentialCenter()
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

		public virtual Vector3 getRelativeVelocity(Point p)
		{
			return Vector3.Zero;
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

		internal virtual void SolveForNextPosition(float TotalElapsedSeconds)
		{

			foreach (Body child in childBodies)
			{
				child.SolveForNextPosition(TotalElapsedSeconds);
			}

			foreach (Point p in points)
			{
				Vector3 Acceleration = p.AccelerationThisFrame + (p.ForceThisFrame / p.Mass);
				p.PotentialVelocity = p.CurrentVelocity + (Acceleration * TotalElapsedSeconds);

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

		internal virtual List<CollisionEvent> findCollisions(Body c)
		{
			List<CollisionEvent> events = new List<CollisionEvent>();

			foreach (Body child in childBodies)
			{
				if (child.getBoundingBox().intersects(c.getBoundingBox()))
				{
					events.AddRange(child.findCollisions(c));
				}
			}

			if (points.Count > 0)
			{
				events.AddRange(c.findCollisionsWith(this));

			}

			return events;
		}

		internal virtual List<CollisionEvent> findCollisionsWith(Body b)
		{
			List<CollisionEvent> events = new List<CollisionEvent>();

			foreach (Body child in childBodies)
			{
				if (child.getBoundingBox().intersects(b.getBoundingBox()))
				{
					events.AddRange(child.findCollisionsWith(b));
				}
			}

			if (collidables.Count > 0)
			{
				events.AddRange(b.findPointCollisions(this));
			}

			return events;
		}

		internal virtual List<CollisionEvent> findPointCollisions(Body c)
		{
			List<CollisionEvent> events = new List<CollisionEvent>();

			foreach (Point p in points)
			{
				foreach (Collidable x in c.collidables)
				{
					if (x.couldIntersect(p.CurrentPosition, p.PotentialPosition))
					{
						float u = x.didIntersect(p.CurrentPosition, p.PotentialPosition);
						if (u > 0 && u < 1)
						{
							events.Add(new CollisionEvent(p, x, u));
						}
					}
				}
			}

			return events;

		}
        public virtual void onCollision(Point p)
        {

        }

        public Vector3 getVelocity()
        {
            return Vector3.Zero;
        }
	}
}
