using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics
{
	public static class Physics
	{

		static float friction = 42.0f;
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

        static List<Body> bodys = new List<Body>();

        public static void AddBody(Body b)
        {
            bodys.Add(b);
        }
        public static void AddBodys(IEnumerable<Body> b)
        {
            bodys.AddRange(b);
        }


        //static List<Collidable> collision = new List<Collidable>();
        //static List<Point> points = new List<Point>();
        //static List<Spring> springs = new List<Spring>();

        //public static void AddCollidable(Collidable c)
        //{
        //    collision.Add(c);
        //}
        //public static void AddCollidables(IEnumerable<Collidable> c)
        //{
        //    collision.AddRange(c);
        //}
        //public static void AddPoint(Point p)
        //{
        //    points.Add(p);
        //}
        //public static void AddPoints(IEnumerable<Point> p)
        //{
        //    points.AddRange(p);
        //}
        //public static void AddSpring(Spring s)
        //{
        //    springs.Add(s);
        //}
        //public static void AddSprings(IEnumerable<Spring> s)
        //{
        //    springs.AddRange(s);
        //}

		public static void Clear()
		{
			//collision.Clear();
			//points.Clear();
			//springs.Clear();
            bodys.Clear();
		}

		static float gravityForce = 9.8f;
		static bool gravityMode = true;

		public static void setGravityMode(bool point)
		{
			gravityMode = point;
		}

		static Vector3 getGravity(Vector3 from)
		{
			if (gravityMode)
			{
				return Vector3.Normalize(gravityOrigin - from) * gravityForce;
			}
			else
			{
				return new Vector3(0f, -gravityForce, 0f);
			}
			//return Vector3.Normalize(gravityOrigin - from) * ((9.8f * 25f) / Vector3.DistanceSquared(from, gravityOrigin) );
		}
		public static Vector3 getUp(Vector3 from)
		{
			bool Collision = false;
			Collidable CollisionTri = null;
			float CollisionU = 1;
            foreach (Body b in bodys)
            {
                foreach (Collidable c in b.getCollidables())
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
            }

			if (Collision)
			{
				return CollisionTri.Normal();
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

            foreach (Body b in bodys)
            {
                b.sumSpringForces();
            }

			doSomePhysics(TotalElapsedSeconds);

            foreach (Body b in bodys)
            {

                foreach (Point p in b.getPoints())
                {
                    p.updatePosition();
                }

            }
		}

		private static List<Collidable> CollisionChain = new List<Collidable>();
		private static void doSomePhysics(float TotalElapsedSeconds)
		{

            foreach (Body b in bodys)
            {

                foreach (Point p in b.getPoints())
                {
                    CollisionChain.Clear();
                    // Apply Freefall Forces
                    Vector3 Force = p.Force;
                    // forces
                    Force += getGravity(p.Position) * p.mass;
                    //foreach (Spring s in b.getSprings())
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
                    Force += Vector3.Negate(p.Velocity) * airfriction;

                    // Calc Acceleration
                    Vector3 Acceleration = p.Acceleration;
                    Acceleration = Force / p.mass;

                    // Calc Velocity
                    Vector3 Velocity = p.Velocity;
                    Velocity += Acceleration * TotalElapsedSeconds;

                    // Calc New Position
                    Vector3 originalPosition = p.Position;
                    Vector3 finalPosition = p.Position;
                    finalPosition += Velocity * TotalElapsedSeconds;


                    // Check for Collision
                    bool Collision = false;
                    Collidable CollisionTri = null;
                    float CollisionU = float.MaxValue;
                    foreach (Body b2 in bodys)
                    {
                        if (b2 != b)
                        {
                            foreach (Collidable c in b2.getCollidables())
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

                    }

                    if (Collision)
                    {
                        // freefallPhysics first half
                        freefallPhysics(b, p, TotalElapsedSeconds * CollisionU);

                        // sliding physics second half
                        slidingPhysics(b, p, TotalElapsedSeconds * (1 - CollisionU), CollisionTri);
                    }

                    // If No Collision, apply values Calc'd Above
                    if (!Collision)
                    {
                        p.Force = Vector3.Zero;
                        p.Acceleration = Vector3.Zero;
                        p.Velocity = Velocity;
                        p.NextPosition = finalPosition;
                    }

                }
            }

		}
		private static void freefallPhysics(Body b, Point p, float time)
		{

			// forces
			p.Force += getGravity(p.Position) * p.mass;
                //foreach (Spring s in b.getSprings())
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
			p.Force += Vector3.Negate(p.Velocity) * airfriction;

			// acceleration
			p.Acceleration = p.Force / p.mass;

			// velocity - by euler-cromer
			p.Velocity += p.Acceleration * time;

			// position
			p.NextPosition += p.Velocity * time;


			//done
			p.Force = Vector3.Zero;
			p.Acceleration = Vector3.Zero;

		}
		private static void slidingPhysics(Body b, Point p, float time, Collidable s)
		{

			// nudge it out just enough to be above the plane to keep floating point error from falling through
			while (s.DotNormal(p.NextPosition) <= 0)
			{
				p.NextPosition += (s.Normal() * 0.001f);
			}


			// Stop Velocity in direction of the wall
			Vector3 NormalEffect = (s.Normal() * (p.Velocity.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(p.Velocity, s.Normal()).Length(), Vector3.Dot(p.Velocity, s.Normal())))));
			p.Velocity = (p.Velocity - NormalEffect);

			impact += NormalEffect.Length() * 0.1f;

			// forces
			Vector3 Force = p.Force;
			Force += getGravity(p.NextPosition) * p.mass;
                //foreach (Spring sp in b.getSprings())
                //{
                //    if (p == sp.A)
                //    {
                //        p.Force += sp.getForceVectorOnA();
                //    }
                //    else if (p == sp.B)
                //    {
                //        p.Force += sp.getForceVectorOnB();
                //    }
                //}

			// normal force
			Vector3 NormalForce = (s.Normal() * (Force.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(Force, s.Normal()).Length(), Vector3.Dot(Force, s.Normal())))));
			Force = (Force - NormalForce);

			// air friction
			Force += Vector3.Negate(p.Velocity) * airfriction;

			// surface friction !
			Force += Vector3.Negate(p.Velocity) * friction;


			// acceleration
			Vector3 Acceleration = p.Acceleration;
			Acceleration = Force / p.mass;

			// velocity
			Vector3 Velocity = p.Velocity;
			Velocity += Acceleration * time;

			// position
			Vector3 originalPosition = p.NextPosition;
			Vector3 finalPosition = p.NextPosition;
			finalPosition += Velocity * time;


			bool Collision = false;
			Collidable CollisionTri = null;
			float CollisionU = float.MaxValue;
            foreach (Body b2 in bodys)
            {
                if (b2 != b)
                {
                    foreach (Collidable c in b2.getCollidables())
                    {
                        float u = c.didIntersect(originalPosition, finalPosition);
                        // If Collision ( u < 1 ) - Split Time and redo
                        if (u < 1)
                        {

                            if (s == c)
                            {
                                Console.WriteLine("Duplicate Collision!");
                                //throw new Exception();
                                continue;
                            }

                            if (CollisionChain.Contains(s))
                            {
                                Console.WriteLine("Duplicate Sliding Collision! Ignoring - This probably means a point is going to fall through the world.");
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
            }

			if (Collision)
			{
				CollisionChain.Add(s);

				// freefallPhysics first half
				slidingPhysics(b, p, time * CollisionU, s);

				// sliding physics second half
				slidingPhysics(b, p, time * (1 - CollisionU), CollisionTri);
			}

			// If No Collision, apply values Calc'd Above
			if (!Collision)
			{
				p.Force = Vector3.Zero;
				p.Acceleration = Vector3.Zero;
				p.Velocity = Velocity;
				p.NextPosition = finalPosition;
			}



			// done
			p.Force = Vector3.Zero;
			p.Acceleration = Vector3.Zero;

		}


	}
}
