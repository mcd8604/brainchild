using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	internal class PhysicsSeq : PhysicsManager
	{

		internal List<Body> bodies = new List<Body>();
		public override void AddBody(Body b)
		{
			bodies.Add(b);
		}
		public override void AddBodys(IEnumerable<Body> b)
		{
			bodies.AddRange(b);
		}

		private float airFriction = 1f;
		public override float AirFriction
		{
			get
			{
				return airFriction;
			}
			set
			{
				airFriction = value;
			}
		}

#if DEBUG
		private int DEBUG_NumCollidables = 0;
		public override int DEBUG_GetNumCollidables()
		{
			return DEBUG_NumCollidables;
		}
#endif

		private Player player = new Player();
		public override Player Player
		{
			get { return player; }
		}

		public override float PWR
		{
			get { return 1f; }
		}

		public override void stop()
		{ }

		public override void update(float TotalElapsedSeconds)
		{
			doPhysics(TotalElapsedSeconds);

			foreach (Body b in bodies)
			{
				b.updatePosition();
			}
		}

		internal void doPhysics(float TotalElapsedSeconds)
		{

			player.update(TotalElapsedSeconds);

			// Predict potential position
#if DEBUG
			int CollidableCount = 0;
#endif
			foreach (Body b in bodies)
			{
#if DEBUG
				CollidableCount += b.collidables.Count;
#endif
				b.update(TotalElapsedSeconds);
			}
#if DEBUG
			DEBUG_NumCollidables = CollidableCount;
#endif

			// Solve for the 'Actual' potential positions based on force and acceleration
			foreach (Body b in bodies)
			{
				b.SolveForNextPosition(TotalElapsedSeconds);
			}

			// Check BoundingBoxes, Find collisions, add to collision list
			List<CollisionEvent> events = new List<CollisionEvent>();

			foreach (Body b in bodies)
			{
				if (!(b is BodyStatic))
				{
					AxisAlignedBoundingBox box = b.getBoundingBox();
					foreach (Body c in bodies)
					{
						if (b != c)
						{
							if (box.intersects(c.getBoundingBox()))
							{
								events.AddRange(b.findCollisions(c));
							}
						}
					}

					foreach (PhysicsPoint p in b.points)
					{
						p.NextVelocity = p.PotentialVelocity;
						p.NextPosition = p.PotentialPosition;
						p.LastCollision = null;
					}

				}
			}



			// Evaluate collsion list, call onCollsion, set NextPosition
			events.Sort(CollisionEvent.CompareEvents);

			foreach (CollisionEvent e in events)
			{

				// handle collision, sliding;
				Vector3 newPosition = e.point.CurrentPosition + ((e.point.PotentialPosition - e.point.CurrentPosition) * e.when);

				// bump?

				// stop point velocity in the direction of the collidable
				Vector3 CollidableNormal = Vector3.Normalize(e.collidable.Normal());

				Vector3 VelocityTransfer = Vector3.Zero;
				Vector3 newVelocity = Vector3.Zero;

				if (Vector3.Dot(e.point.PotentialVelocity, CollidableNormal) > 0)
				{
					Vector3 CollidableBodyVelocity = Vector3.Dot(e.collidable.getVelocity(), CollidableNormal) * CollidableNormal;
					newVelocity = (Vector3.Cross(CollidableNormal, Vector3.Cross(e.point.PotentialVelocity, CollidableNormal))) + CollidableBodyVelocity;

					// apply velocity to collidable
					VelocityTransfer = (Vector3.Dot(e.point.PotentialVelocity, CollidableNormal) * CollidableNormal) - CollidableBodyVelocity;
					e.collidable.ImpartVelocity(newPosition, VelocityTransfer);
				}

				// normal force
				Vector3 NormalForce = Vector3.Zero;
				Vector3 newForce = Vector3.Zero;

				if (Vector3.Dot(e.point.ForceThisFrame, CollidableNormal) > 0)
				{
					NormalForce = (Vector3.Dot(e.point.ForceThisFrame, CollidableNormal) * CollidableNormal);

					newForce = (Vector3.Cross(CollidableNormal, Vector3.Cross(e.point.ForceThisFrame, CollidableNormal)));

					e.collidable.ApplyForce(newPosition, NormalForce);

				}

				Vector3 TotalNormalForce = (VelocityTransfer / (TotalElapsedSeconds * (1 - e.when) ) * e.point.Mass) + NormalForce;

				// relative velocity
				Vector3 relativeVelocity = e.collidable.getRelativeVelocity(e.point) - newVelocity;

				// surface friction !  F = uN
				if (relativeVelocity.LengthSquared() > 0)
				{
					Vector3 FrictionForce = Vector3.Normalize(relativeVelocity) * (TotalNormalForce.Length() * (player.Traction.value * e.collidable.getMaterial().getFriction()));

					// This is the maximum amount of force to stop the point - will need tweaking for conveyor belts
					Vector3 MaxFriction = ((relativeVelocity / (TotalElapsedSeconds * (1 - e.when))) * e.point.Mass) + Vector3.Negate(newForce);

					if (FrictionForce.LengthSquared() > MaxFriction.LengthSquared())
					{
						//Console.WriteLine("Maxed out friction: " + (FrictionForce.Length() / MaxFriction.Length()));
						newForce += MaxFriction;
					}
					else
					{
						//Console.WriteLine("Didn't: " + (FrictionForce.Length() / MaxFriction.Length()));
						newForce += FrictionForce;
					}

				}

				// if static then point is relatively static
				if (e.isStatic())
				{
					e.point.relativelyStatic = true;
				}

				// acceleration
				Vector3 Acceleration = newForce / e.point.Mass;

				// velocity
				Vector3 Velocity = newVelocity + (Acceleration * (TotalElapsedSeconds * (1 - e.when)));

				// position
				Vector3 Position = newPosition + (Velocity * (TotalElapsedSeconds * (1 - e.when)));

				e.point.NextVelocity = Velocity;
				e.point.NextPosition = Position;
				e.point.LastCollision = e.collidable;

				// Bump?

			}

			foreach (CollisionEvent e in events)
			{
				e.trigger();
			}


		}

	}
}
