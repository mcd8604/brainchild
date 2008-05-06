using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Body
	{

		public IList<PhysicsPoint> points = new List<PhysicsPoint>();
		public IList<Collidable> collidables = new List<Collidable>();
		public IList<Spring> springs = new List<Spring>();
		public IList<Task> tasks = new List<Task>();

		public Body parentBody = null;
		public IList<Body> childBodies = new List<Body>();

		public AxisAlignedBoundingBox boundingBox = new AxisAlignedBoundingBox();

		public Vector3 center;
		public Vector3 potentialCenter;

		protected Material material = Material.getDefaultMaterial();

		internal Body() { }

		public Body(Body ParentBody)
		{
			if (ParentBody != null)
			{
				ParentBody.addChild(this);
			}
		}

		public Body(Body ParentBody, IList<PhysicsPoint> p_points, IList<Collidable> p_collidables, IList<Spring> p_springs, IList<Task> p_tasks)
		{
			if (ParentBody != null)
			{
				ParentBody.addChild(this);
			}
			points = p_points;
			springs = p_springs;
			collidables = p_collidables;
			tasks = p_tasks;

			initialize();
		}

		public virtual void initialize()
		{
			foreach (Collidable c in collidables)
			{
				if (c.parent != null && c.parent != this)
				{
					throw new Exception();
				}
				c.parent = this;
				boundingBox.expandToInclude(c.getBoundingBox());
			}
			foreach (PhysicsPoint p in points)
			{
				boundingBox.expandToInclude(p.CurrentPosition);
			}
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

		public virtual bool isSolid()
		{
			return true;
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
			return potentialCenter;
		}

		public virtual IEnumerable<Collidable> getCollidables()
		{
			return collidables;
		}

		public virtual IEnumerable<PhysicsPoint> getPoints()
		{
			return points;
		}

		public virtual IEnumerable<Spring> getSprings()
		{
			return springs;
		}

		public virtual Vector3 getRelativeVelocity(PhysicsPoint p)
		{
            // fix later
			return Vector3.Zero;
		}

		public virtual Material getMaterial()
		{
			return material;
		}

		public virtual void setMaterial(Material m)
		{
			material = m;
		}

		public virtual void update(float TotalElapsedSeconds)
		{
			foreach (Body b in childBodies)
			{
				b.update(TotalElapsedSeconds);
			}

			// Predict potential position
			potentialCenter = Vector3.Zero;
			foreach (PhysicsPoint p in points)
			{
				p.PotentialPosition = p.CurrentPosition + (p.CurrentVelocity * TotalElapsedSeconds);

                // temp
                boundingBox.expandToInclude(p.PotentialPosition);

				potentialCenter += p.PotentialPosition;
			}
			potentialCenter /= points.Count;

			foreach (Collidable c in collidables)
			{
				c.update();
			}

			// Springs
			foreach (Spring s in springs)
			{
				s.update();
			}

			//Tasks
			foreach (Task t in tasks)
			{
				t.update(this, TotalElapsedSeconds);
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
			foreach (PhysicsPoint p in points)
			{
				p.updatePosition();
				center += p.CurrentPosition;
				boundingBox.expandToInclude(p.CurrentPosition);
			}
			center /= points.Count;

			foreach (Collidable c in collidables)
			{
				c.update();
				boundingBox.expandToInclude(c.getBoundingBox());
			}

		}

		internal virtual void SolveForNextPosition(float TotalElapsedSeconds)
		{

			foreach (Body child in childBodies)
			{
				child.SolveForNextPosition(TotalElapsedSeconds);
			}

			foreach (PhysicsPoint p in points)
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

                // temp again
                boundingBox.expandToInclude(p.PotentialPosition);
			}

		}

		internal virtual List<CollisionEvent> findCollisions(Body c)
		{
            // fix later
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
            // fix later
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
            // fix later
			List<CollisionEvent> events = new List<CollisionEvent>();

			foreach (PhysicsPoint p in points)
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
		public virtual void onCollision(PhysicsPoint p) { }

		public Vector3 getVelocity()
		{
			return Vector3.Zero;
		}

	}
}
