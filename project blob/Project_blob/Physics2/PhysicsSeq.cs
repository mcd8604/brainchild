using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics2
{
	internal sealed class PhysicsSeq : PhysicsManager
	{

		float time;

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

		public override float PWR
		{
			get { return 1f; }
		}
#endif

		private Player player = new Player();
		public override Player Player
		{
			get { return player; }
		}

		public override void stop()
		{ }

		public override float Time
		{
			get
			{
				return time;
			}
		}

		public override void update(float TotalElapsedSeconds)
		{

			if (TotalElapsedSeconds == 0f)
			{
				return;
			}

			doPhysics(TotalElapsedSeconds * physicsMultiplier);

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
				if (b.canCollide())
				{
					AxisAlignedBoundingBox box = b.getBoundingBox();
					foreach (Body c in bodies)
					{
						if (b != c)
						{
							if (box.intersects(c.getBoundingBox()))
							{
								b.findCollisions(c, ref events);
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

			List<PhysicsPoint> check = new List<PhysicsPoint>();

			foreach (CollisionEvent e in events)
			{

				if (check.Contains(e.point) || !e.isBlocking())
				{
					continue;
				}
				check.Add(e.point);

				// handle collision, sliding;
				Vector3 newPosition = e.collisionPoint;

#if DEBUG
				// Check!!
				if (newPosition != e.point.CurrentPosition + ((e.point.PotentialPosition - e.point.CurrentPosition) * e.when))
				{
					// if this is ever spamming, let me know! - Adam
					Log.Out.WriteLine("mismatch: " + newPosition + " : " + e.collisionPoint);
				}
#endif

				// bump?
				//while (s.DotNormal(p.NextPosition) <= 0)
				//{
				//    int i = 0;
				//    //p.NextPosition += (collisionNormal * 0.001f);
				//    //++DEBUG_BumpLoops;
				//}
				//Plane p = e.collidable.Plane;
				//float d = Vector3.Dot(e.collidable.Normal, newPosition);


				//newPosition += Vector3.Normalize(e.collidable.Normal) * 0.01f;


				//Plane p2 = e.collidable.Plane;
				//float d2 = Vector3.Dot(e.collidable.Normal, newPosition);


				//if ((d > 0 && d2 < 0) || (d < 0 && d2 > 0))
				//{
				//    int i = 0;
				//}

				//bump (this may or may not be neccessary)
				float d = e.collidable.Plane.D;
				float d2 = Vector3.Dot(e.collidable.Normal, newPosition);
				while ((d + d2) <= 0)
				{
					newPosition += Vector3.Normalize(e.collidable.Normal) * 0.001f;
					d2 = Vector3.Dot(e.collidable.Normal, newPosition);
				}


				// stop point velocity in the direction of the collidable
				Vector3 CollidableNormal = Vector3.Normalize(e.collidable.Normal);

				Vector3 VelocityTransfer = Util.Zero;
				Vector3 newVelocity = Util.Zero;

				if (Vector3.Dot(e.point.PotentialVelocity, CollidableNormal) < 0)
				{
					Vector3 CollidableBodyVelocity = Vector3.Dot(e.collidable.getVelocity(), CollidableNormal) * CollidableNormal;
					newVelocity = (Vector3.Cross(CollidableNormal, Vector3.Cross(e.point.PotentialVelocity, CollidableNormal))) + CollidableBodyVelocity;

					// apply velocity to collidable
					VelocityTransfer = (Vector3.Dot(e.point.PotentialVelocity, CollidableNormal) * CollidableNormal) - CollidableBodyVelocity;
					e.collidable.ImpartVelocity(newPosition, VelocityTransfer);
				}

				// normal force
				Vector3 NormalForce = Util.Zero;
				Vector3 newForce = Util.Zero;

				if (Vector3.Dot(e.point.ForceThisFrame, CollidableNormal) < 0)
				{
					NormalForce = (Vector3.Dot(e.point.ForceThisFrame, CollidableNormal) * CollidableNormal);

					newForce = (Vector3.Cross(CollidableNormal, Vector3.Cross(e.point.ForceThisFrame, CollidableNormal)));

					e.collidable.ApplyForce(newPosition, NormalForce);

				}

				Vector3 TotalNormalForce = (VelocityTransfer / (TotalElapsedSeconds * (1 - e.when)) * e.point.Mass) + NormalForce;

				e.impact = TotalNormalForce.Length();

				// relative velocity
				Vector3 relativeVelocity = e.collidable.getRelativeVelocity(e) - newVelocity;

				// fix relative velocity to be along surface
				relativeVelocity = (Vector3.Cross(CollidableNormal, Vector3.Cross(relativeVelocity, CollidableNormal)));
				// give it a little bump to keep from falling through
				relativeVelocity += CollidableNormal;

				// surface friction !  F = uN
				if (relativeVelocity.LengthSquared() > 0)
				{
					Vector3 FrictionForce = Vector3.Normalize(relativeVelocity) * (TotalNormalForce.Length() * (player.Traction.value * e.collidable.getMaterial().Friction));

					// This is the maximum amount of force to stop the point
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

				//bump (this may or may not be neccessary)
				float d22 = Vector3.Dot(e.collidable.Normal, Position);
				while ((d + d22) <= 0)
				{
					Position += Vector3.Normalize(e.collidable.Normal) * 0.001f;
					d22 = Vector3.Dot(e.collidable.Normal, Position);
				}

				e.point.NextVelocity = Velocity;
				e.point.NextPosition = Position;
				e.point.LastCollision = e.collidable;
			}

			// check check?

			foreach (CollisionEvent e in events)
			{
				e.trigger();
			}

			time += TotalElapsedSeconds;

		}

	}
}
