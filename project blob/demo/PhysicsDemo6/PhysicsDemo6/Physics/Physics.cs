using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics
{
	public static class Physics
	{

		static float friction = 1f;
		public static float TEMP_SurfaceFriction
		{
			get
			{
				return friction;
			}
			set
			{
				friction = value;
			}
		}
		static float airfriction = 1f;
		public static float AirFriction
		{
			get
			{
				return airfriction;
			}
			set
			{
				airfriction = value;
			}
		}

		static Vector3 gravityOrigin = Vector3.Zero;

		static List<Collidable> collision = new List<Collidable>();
		static List<Point> points = new List<Point>();
		static List<Spring> springs = new List<Spring>();

		public static void AddCollidable(Collidable c)
		{
			collision.Add(c);
		}
		public static void AddPoint(Point p)
		{
			points.Add(p);
		}
		public static void AddPoints(IEnumerable<Point> p)
		{
			points.AddRange(p);
		}
		public static void AddSpring(Spring s)
		{
			springs.Add(s);
		}
		public static void AddSprings(IEnumerable<Spring> s)
		{
			springs.AddRange(s);
		}

		public static void Clear()
		{
			collision.Clear();
			points.Clear();
			springs.Clear();
		}

		private static Gravity gravity;
		public static Gravity Gravity
		{
			get
			{
				return gravity;
			}
			set
			{
				gravity = value;
			}
		}

		public static int DEBUG_BumpLoops = 0;

		public static Vector3 getUp(Vector3 from)
		{
			bool Collision = false;
			Collidable CollisionTri = null;
			float CollisionU = 1;
			foreach (Collidable c in collision)
			{

				float u = c.didIntersect(from, gravityOrigin);
				if (u < 1)
				{
					if (!Collision)
					{
						CollisionTri = c;
						CollisionU = u;
						Collision = true;
					}
					else
					{
						if (u < CollisionU)
						{
							CollisionTri = c;
							CollisionU = u;
						}
					}


				}

			}

			if (Collision)
			{
				return CollisionTri.getPlane().Normal;
			}
			else
			{
				return Vector3.Up;
			}
		}

		static float impact = 0;
		public static float ImpactThisFrame
		{
			get
			{
				return impact;
			}
		}

		public static void update(float TotalElapsedSeconds)
		{
			impact = 0;

			foreach (Spring s in springs)
			{
				s.ApplyForces();
			}
			foreach (Point p in points)
			{
				p.CurrentForce += gravity.getForceOn(p);
			}

			doSomePhysics(TotalElapsedSeconds);

			foreach (Point p in points)
			{
				p.updatePosition();
			}
		}

		private static List<Collidable> CollisionChain = new List<Collidable>();
		private static void doSomePhysics(float TotalElapsedSeconds)
		{

			foreach (Point p in points)
			{
				CollisionChain.Clear();
				// Apply Freefall Forces
				Vector3 Force = p.CurrentForce;
				// forces
				//Force += getGravity(p.Position) * p.mass;
				//foreach (Spring s in springs)
				//{
				//    if (p == s.A)
				//    {
				//        Force += s.getForceVectorOnA();
				//    }
				//    else if (p == s.B)
				//    {
				//        Force += s.getForceVectorOnB();
				//    }
				//}

				// Air Friction
				Force += Vector3.Negate(p.NextVelocity) * airfriction;

				// Calc Acceleration
				Vector3 Acceleration = p.NextAcceleration;
				Acceleration = Force / p.mass;

				// Calc Velocity
				Vector3 Velocity = p.NextVelocity;
				Velocity += Acceleration * TotalElapsedSeconds;

				// Calc New Position
				Vector3 originalPosition = p.NextPosition;
				Vector3 finalPosition = p.NextPosition;
				finalPosition += Velocity * TotalElapsedSeconds;


				// Check for Collision
				bool Collision = false;
				Collidable CollisionTri = null;
				float CollisionU = float.MaxValue;
				foreach (Collidable c in collision)
				{
					if (c.couldIntersect(p))
					{
						float u = c.didIntersect(originalPosition, finalPosition);
						// If Collision ( u < 1 ) - Split Time and redo
						if (u < 1)
						{

							if (Collision)
							{

								//Console.WriteLine("Secondary Collision!");

							}


							if (!Collision)
							{
								CollisionTri = c;
								CollisionU = u;
								Collision = true;
							}
							else
							{
								if (u < CollisionU)
								{
									CollisionTri = c;
									CollisionU = u;
								}
							}


						}

					}
				}

				if (Collision)
				{
					// freefallPhysics first half
					freefallPhysics(p, TotalElapsedSeconds * CollisionU);

					// sliding physics second half
					slidingPhysics(p, TotalElapsedSeconds * (1 - CollisionU), CollisionTri);
				} else 
				// If No Collision, apply values Calc'd Above
				{
					//p.CurrentForce = Vector3.Zero;
					//p.CurrentAcceleration = Vector3.Zero;
                    p.NextAcceleration = Acceleration;
					p.NextVelocity = Velocity;
					p.NextPosition = finalPosition;
					p.LastCollision = null;
				}

			}

		}
		private static void freefallPhysics(Point p, float time)
		{

			// forces
			Vector3 Force = p.CurrentForce;
			//Force += getGravity(p.Position) * p.mass;
			//foreach (Spring s in springs)
			//{
			//    if (p == s.A)
			//    {
			//        p.Force += s.getForceVectorOnA();
			//    }
			//    else if (p == s.B)
			//    {
			//        p.Force += s.getForceVectorOnB();
			//    }
			//}

			// air friction
			Force += Vector3.Negate(p.NextVelocity) * airfriction;

			// acceleration
			//Vector3 Acceleration = p.NextAcceleration;
            p.NextAcceleration = Force / p.mass;

			// velocity - by euler-cromer
			//Vector3 Velocity = p.NextVelocity;
            p.NextVelocity += p.NextAcceleration * time;

			// position
			p.NextPosition += p.NextVelocity * time;


			//done
			//Force = Vector3.Zero;
			//Acceleration = Vector3.Zero;
            

		}
		private static void slidingPhysics(Point p, float time, Collidable s)
		{

			Vector3 collisionNormal = s.getPlane().Normal;

			// nudge it out just enough to be above the plane to keep floating point error from falling through
			while (s.DotNormal(p.NextPosition) <= 0)
			//while ( Vector3.Dot( collisionNormal, p.NextPosition) <= 0)
			{
				p.NextPosition += (collisionNormal * 0.001f);
				++DEBUG_BumpLoops;
			}


			// Stop Velocity in direction of the wall
			Vector3 NormalEffect = (collisionNormal * (p.NextVelocity.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(p.NextVelocity, collisionNormal).Length(), Vector3.Dot(p.NextVelocity, collisionNormal)))));
			p.NextVelocity = (p.NextVelocity - NormalEffect);
			s.ImpartVelocity(p.NextPosition, NormalEffect);

			impact += NormalEffect.Length() * 0.1f;

			// forces
			Vector3 Force = p.CurrentForce;
			//Force += getGravity(p.NextPosition) * p.mass;
			//foreach (Spring sp in springs)
			//{
			//    if (p == sp.A)
			//    {
			//        Force += sp.getForceVectorOnA();
			//    }
			//    else if (p == sp.B)
			//    {
			//        Force += sp.getForceVectorOnB();
			//    }
			//}

			// normal force
			Vector3 NormalForce = (collisionNormal * (Force.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(Force, collisionNormal).Length(), Vector3.Dot(Force, collisionNormal)))));
			Force = (Force - NormalForce);
			s.ApplyForce(p.NextPosition, NormalForce);

			// air friction
			Force += Vector3.Negate(p.NextVelocity) * airfriction;

			// surface friction !  F = uN

			//Force += Vector3.Negate(p.NextVelocity) * friction;

			Vector3 FrictionForce = Vector3.Normalize( Vector3.Negate(p.NextVelocity) ) * ( NormalForce.Length() *  friction);

			Vector3 MaxFriction = Vector3.Negate( (p.NextVelocity / time) * p.mass );

			if (FrictionForce.LengthSquared() > MaxFriction.LengthSquared())
			{
				//Console.WriteLine("Maxed out friction: " + (FrictionForce.Length() / MaxFriction.Length()));
				Force += MaxFriction;
			}
			else
			{
				//Console.WriteLine("Didn't: " + (FrictionForce.Length() / MaxFriction.Length()));
				Force += FrictionForce;
			}


			//if ( FrictionForce.LengthSquared() > Force.LengthSquared() ){
			//    Force = Vector3.Zero;
			//} else {
			//    Force += FrictionForce;
			//}


			// acceleration
			Vector3 Acceleration = p.NextAcceleration;
			Acceleration = Force / p.mass;

			// velocity
			Vector3 Velocity = p.NextVelocity;
			Velocity += Acceleration * time;

			// position
			Vector3 originalPosition = p.NextPosition;
			Vector3 finalPosition = p.NextPosition;
			finalPosition += Velocity * time;


			bool Collision = false;
			Collidable CollisionTri = null;
			float CollisionU = float.MaxValue;
			foreach (Collidable c in collision)
			{
				if (c.couldIntersect(p))
				{
					float u = c.didIntersect(originalPosition, finalPosition);
					// If Collision ( u < 1 ) - Split Time and redo
					if (u < 1)
					{

						if (s == c)
						{
							//Console.WriteLine("Duplicate Collision!");
							//throw new Exception();
							continue;
						}

						if (CollisionChain.Contains(s))
						{
							//Console.WriteLine("Duplicate Sliding Collision! Ignoring - This probably means a point is going to fall through the world.");
							//throw new Exception();
							continue;
						}

						//Console.WriteLine("Sliding Collision!");

						if (Collision)
						{

							//Console.WriteLine("Secondary Sliding Collision!");

						}


						if (!Collision)
						{
							CollisionTri = c;
							CollisionU = u;
							Collision = true;
						}
						else
						{
							if (u < CollisionU)
							{
								CollisionTri = c;
								CollisionU = u;
							}
						}

					}
				}
			}

			if (Collision)
			{
				CollisionChain.Add(s);

				// freefallPhysics first half
				slidingPhysics(p, time * CollisionU, s);

				// sliding physics second half
				slidingPhysics(p, time * (1 - CollisionU), CollisionTri);
			}
			else
			{ // If No Collision, apply values Calc'd Above

				//p.CurrentForce = Vector3.Zero;
				//p.CurrentAcceleration = Vector3.Zero;
                p.NextAcceleration = Acceleration;
				p.NextVelocity = Velocity;
				p.NextPosition = finalPosition;
				p.LastCollision = s;
			}

			if (s.DotNormal(p.NextPosition) <= 0)
			{
				// point just fell through
				// this really shouldn't happen, floating point error?
				// so for now, I'll just bump it up some more
				while (s.DotNormal(p.NextPosition) <= 0)
				{
					p.NextPosition += (collisionNormal * 0.001f);
					++DEBUG_BumpLoops;
				}
			}

			// done
			//p.CurrentForce = Vector3.Zero;
			//p.CurrentAcceleration = Vector3.Zero;

		}


	}
}
