using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	public class Body
	{
		protected internal IList<PhysicsPoint> points = new List<PhysicsPoint>();
		protected internal IList<Collidable> collidables = new List<Collidable>();
		protected internal IList<Spring> springs = new List<Spring>();
		protected internal IList<Task> tasks = new List<Task>();

		internal Body parentBody = null;
		internal IList<Body> childBodies = new List<Body>();

		internal AxisAlignedBoundingBox boundingBox = null;

		internal Vector3 center;
		internal Vector3 potentialCenter;

		protected Material material = Material.getDefaultMaterial();

		//temp:
		public Audio.Sound collisionSound;

		protected internal Body()
		{

		}

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
			boundingBox = new AxisAlignedBoundingBox();
			foreach (Collidable c in collidables)
			{
				if (c.parent != null && c.parent != this)
				{
					throw new Exception();
				}
				c.parent = this;
				boundingBox.expandToInclude(c.getBoundingBox());
			}
			center = Vector3.Zero;
			foreach (PhysicsPoint p in points)
			{
				boundingBox.expandToInclude(p.CurrentPosition);
				center += p.CurrentPosition;
			}
			center /= points.Count;
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

		public void addTask(Task t)
		{
			tasks.Add(t);
		}

		/// <summary>
		/// Can this body collide with other bodies?
		/// </summary>
		/// <returns></returns>
		public virtual bool canCollide()
		{
			return true;
		}

		public virtual bool isStatic()
		{
			return false;
		}

		/// <summary>
		/// Does this body block the movement of another body on collision?
		/// </summary>
		/// <returns></returns>
		public virtual bool isSolid()
		{
			return true;
		}

		public virtual AxisAlignedBoundingBox getBoundingBox()
		{
			return boundingBox;
		}

		public virtual Vector3 getAverageVelocity()
		{
			if (points.Count == 0)
			{
				return Vector3.Zero;
			}

			Vector3 AvgVel = Vector3.Zero;
			foreach (PhysicsPoint px in points)
			{
				AvgVel += px.NextVelocity;
			}
			return AvgVel / points.Count;
		}

		public virtual Vector3 getCenter()
		{
			return center;
		}

		public virtual Vector3 getPotentialCenter()
		{
			return potentialCenter;
		}

		public virtual IList<Collidable> getCollidables()
		{
			return collidables;
		}

		public virtual IList<PhysicsPoint> getPoints()
		{
			return points;
		}

		public virtual IList<Spring> getSprings()
		{
			return springs;
		}

		public virtual Vector3 getRelativeVelocity(CollisionEvent e)
		{
			return getAverageVelocity();
		}

		public virtual Material getMaterial()
		{
			return material;
		}

		public virtual void setMaterial(Material m)
		{
			material = m;
		}

		public virtual void setCenter(Vector3 newCenter)
		{
			potentialCenter = newCenter;
			Vector3 diff = potentialCenter - center;
			foreach (PhysicsPoint p in points)
			{
				p.PotentialPosition = p.CurrentPosition + diff;
			}
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

				// this is /probably/ not necessary
				//boundingBox.expandToInclude(p.PotentialPosition);

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

		protected internal virtual void SolveForNextPosition(float TotalElapsedSeconds)
		{

			foreach (Body child in childBodies)
			{
				child.SolveForNextPosition(TotalElapsedSeconds);
			}

			foreach (PhysicsPoint p in points)
			{
				if (p.ForceThisFrame != Vector3.Zero || p.AccelerationThisFrame != Vector3.Zero)
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
						Vector3 hit;
						float u = x.didIntersect(p.CurrentPosition, p.PotentialPosition, out hit);
						if (u > 0 && u < 1)
						{
							events.Add(new CollisionEvent(p, x, u, hit));
						}
					}
				}
			}

			return events;

		}
		public virtual void onCollision(CollisionEvent e)
		{
			if (collisionSound != null)
			{
				collisionSound.play(e.collisionPoint, Engine.CameraManager.getSingleton.ActiveCamera.Listener, e.impact);
			}
		}

		public virtual Vector3 getVelocity()
		{
			// TODO - for task velocity
			return Vector3.Zero;
		}

	}
}
