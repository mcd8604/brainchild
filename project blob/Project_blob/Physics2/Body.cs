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

		private readonly bool Static = false;

		//temp:
		public Audio.Sound collisionSound;

		private static System.Converter<CollidableStatic, Collidable> converter = new System.Converter<CollidableStatic, Collidable>(delegate(CollidableStatic c) { return (Collidable)c; });

		protected internal Body() { }

		public Body(Body ParentBody)
		{
			if (ParentBody != null)
			{
				ParentBody.addChild(this);
			}
		}

		public Body(Body ParentBody, bool isStatic)
		{
			if (ParentBody != null)
			{
				ParentBody.addChild(this);
			}
			Static = isStatic;
		}

		/// <summary>
		/// Replaces the old BodyStatic constructor
		/// </summary>
		/// <param name="p_collidables"></param>
		/// <param name="ParentBody"></param>
		public Body(List<CollidableStatic> p_collidables, Body ParentBody)
		{
			if (ParentBody != null)
			{
				ParentBody.addChild(this);
			}
			collidables = p_collidables.ConvertAll<Collidable>(converter);
			Static = true;
			initializeStatic();
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

		public Body(Body ParentBody, IList<PhysicsPoint> p_points, IList<Collidable> p_collidables, IList<Spring> p_springs, IList<Task> p_tasks, bool isStatic)
		{
			if (ParentBody != null)
			{
				ParentBody.addChild(this);
			}
			Static = isStatic;
			if (!Static)
			{
				points = p_points;
				springs = p_springs;
				collidables = p_collidables;
				tasks = p_tasks;
				initialize();
			}
			else
			{
				collidables = p_collidables;
				initializeStatic();
			}
		}

		public virtual void initialize()
		{
#if DEBUG
			if (boundingBox != null)
			{
				throw new Exception("A Body should not be initiallized twice.");
			}
#endif
			boundingBox = new AxisAlignedBoundingBox();
			foreach (Collidable c in collidables)
			{
#if DEBUG
				if (c.parent != null && c.parent != this)
				{
					throw new Exception("A collidable cannot belong to two bodies.");
				}
#endif
				c.parent = this;
				boundingBox.expandToInclude(c.getBoundingBox());
			}
			center = Util.Zero;
			foreach (PhysicsPoint p in points)
			{
				boundingBox.expandToInclude(p.CurrentPosition);
				center += p.CurrentPosition;
			}
			center /= points.Count;
		}

		public virtual void initializeStatic()
		{
#if DEBUG
			foreach (Collidable c in collidables)
			{
				if (!(c is CollidableStatic))
				{
					throw new Exception("A Static Body must have only Static Collidables.");
				}
			}
#endif
#if DEBUG
			if (boundingBox != null)
			{
				throw new Exception("A Body should not be initiallized twice.");
			}
#endif
			boundingBox = new AxisAlignedBoundingBox();
			foreach (Collidable c in collidables)
			{
#if DEBUG
				if (c.parent != null && c.parent != this)
				{
					throw new Exception("A collidable cannot belong to two bodies.");
				}
#endif
				c.parent = this;
				boundingBox.expandToInclude(c.getBoundingBox());
			}
		}

		public virtual void addChild(Body childBody)
		{
#if DEBUG
			if (Static && !(childBody.Static))
			{
				throw new Exception("BodyStatic child must also be static");
			}
			if (childBody.parentBody != null)
			{
				throw new Exception("A Body cannot have more than one parent.");
			}
#endif
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
			return !Static;
		}

		public virtual bool isStatic()
		{
			return Static;
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
				return Util.Zero;
			}

			Vector3 AvgVel = Util.Zero;
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

		public virtual void setCenter(Vector3 nCenter)
		{
			potentialCenter = nCenter;
			Vector3 diff = potentialCenter - center;
			foreach (PhysicsPoint p in points)
			{
				p.PotentialPosition = p.CurrentPosition + diff;
			}
		}

		public virtual void update(float TotalElapsedSeconds)
		{
			if (Static)
			{
				return;
			}

			foreach (Body b in childBodies)
			{
				b.update(TotalElapsedSeconds);
			}

			// Predict potential position
			potentialCenter = Util.Zero;
			foreach (PhysicsPoint p in points)
			{
				p.PotentialPosition.X = p.CurrentPosition.X + (p.CurrentVelocity.X * TotalElapsedSeconds);
				p.PotentialPosition.Y = p.CurrentPosition.Y + (p.CurrentVelocity.Y * TotalElapsedSeconds);
				p.PotentialPosition.Z = p.CurrentPosition.Z + (p.CurrentVelocity.Z * TotalElapsedSeconds);

				potentialCenter.X += p.PotentialPosition.X;
				potentialCenter.Y += p.PotentialPosition.Y;
				potentialCenter.Z += p.PotentialPosition.Z;
			}
			potentialCenter.X /= points.Count;
			potentialCenter.Y /= points.Count;
			potentialCenter.Z /= points.Count;

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
			if (Static)
			{
				return;
			}

			boundingBox.clear();
			foreach (Body child in childBodies)
			{
				child.updatePosition();
				boundingBox.expandToInclude(child.getBoundingBox());
			}

			center = Util.Zero;
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
			if (Static)
			{
				return;
			}

			foreach (Body child in childBodies)
			{
				child.SolveForNextPosition(TotalElapsedSeconds);
			}

			foreach (PhysicsPoint p in points)
			{
				if (p.ForceThisFrame != Util.Zero || p.AccelerationThisFrame != Util.Zero)
				{
					Vector3 Acceleration = p.AccelerationThisFrame + (p.ForceThisFrame / p.Mass);
					p.PotentialVelocity = p.CurrentVelocity + (Acceleration * TotalElapsedSeconds);

					if (p.PotentialVelocity != Util.Zero)
					{
						Vector3 DragForce = p.PotentialVelocity * PhysicsManager.AirFriction;
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

		internal void findCollisions(Body c, ref List<CollisionEvent> events)
		{
			foreach (Body child in childBodies)
			{
				if (child.getBoundingBox().intersects(c.getBoundingBox()))
				{
					child.findCollisions(c, ref events);
				}
			}

			if (points.Count > 0)
			{
				c.findCollisionsWith(this, ref events);

			}
		}

		private void findCollisionsWith(Body b, ref List<CollisionEvent> events)
		{
			foreach (Body child in childBodies)
			{
				if (child.getBoundingBox().intersects(b.getBoundingBox()))
				{
					child.findCollisionsWith(b, ref events);
				}
			}

			if (collidables.Count > 0)
			{
				foreach (PhysicsPoint p in b.points)
				{
					foreach (Collidable x in collidables)
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
			}
		}

		public virtual void onCollision(CollisionEvent e)
		{
			if (collisionSound != null)
			{
				collisionSound.play(e.collisionPoint, e.impact);
			}
		}

		public virtual Vector3 getVelocity()
		{
			// TODO - for task velocity
			return getAverageVelocity();
		}

		public IList<Task> getTasks()
		{
			return tasks;
		}

	}
}
