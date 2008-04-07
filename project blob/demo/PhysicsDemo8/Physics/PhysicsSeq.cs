using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Physics
{
    public class PhysicsSeq : PhysicsManager
    {

        private Player player = new Player();
        public Player Player
        {
            get
            {
                return player;
            }
        }

        float airfriction = 1f;
        public float AirFriction
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

        private List<Gravity> gravity = new List<Gravity>();
        public void AddGravity(Gravity g)
        {
            gravity.Add(g);
        }

        List<Collidable> collision = new List<Collidable>();
        public void AddCollidable(Collidable c)
        {
            collision.Add(c);
        }
        public void AddCollidables(IEnumerable<Collidable> c)
        {
            collision.AddRange(c);
        }

        List<Point> points = new List<Point>();
        public void AddPoint(Point p)
        {
            points.Add(p);
        }
        public void AddPoints(IEnumerable<Point> p)
        {
            points.AddRange(p);
        }

        List<Spring> springs = new List<Spring>();
        public void AddSpring(Spring s)
        {
            springs.Add(s);
        }
        public void AddSprings(IEnumerable<Spring> s)
        {
            springs.AddRange(s);
        }

        List<Body> bodys = new List<Body>();
        public void AddBody(Body b)
        {
            bodys.Add(b);
        }
        public void AddBodys(IEnumerable<Body> b)
        {
            bodys.AddRange(b);
        }

        public void update(float TotalElapsedSeconds)
        {

            player.update(TotalElapsedSeconds);

			// Predict potiential position
			foreach (Point p in points)
			{
				p.PotientialPosition = p.CurrentPosition + (p.CurrentVelocity * TotalElapsedSeconds);
			}

            // Springs
            foreach (Spring s in springs)
            {
                s.ApplyForces();
            }

            // Volumes
            foreach (Body b in bodys)
            {
                PressureBody pb = b as PressureBody;
                if (pb != null)
                {
                    Vector3 CurrentCenter = pb.getCenter();
					Vector3 NextCenter = pb.getNextCenter();
                    float CurrentVolume = pb.getVolume();
					float NextVolume = pb.getNextVolume();
                    float IdealVolume = pb.getIdealVolume();
                    foreach (Physics.Point p in pb.getPoints())
                    {
						p.ForceThisFrame += (Vector3.Normalize(CurrentCenter - p.CurrentPosition) * (CurrentVolume - IdealVolume)) + (Vector3.Normalize(CurrentCenter - p.PotientialPosition) * (NextVolume - IdealVolume));
                    }
                }
            }

            // Gravity
            foreach (Gravity g in gravity)
            {
                foreach (Point p in points)
                {
                    p.ForceThisFrame += g.getForceOn(p);
                }
            }

            foreach (Point p in points)
            {
                fall(p, TotalElapsedSeconds);
            }

            // Collisions


            //temp
            foreach (Point p in points)
            {
                checkCollisions2(p, TotalElapsedSeconds);
            }

            foreach (Point p in points)
            {
                p.updatePosition();
            }

        }

        private void fall(Point p, float time)
        {
			/*
            p.PotientialAcceleration = p.CurrentAcceleration + (p.ForceThisFrame / p.mass);
            p.PotientialVelocity = p.CurrentVelocity + (p.PotientialAcceleration * time);

            bool changed = false;
            if ((p.PotientialVelocity.X > 0 && p.CurrentVelocity.X < 0) ||
                (p.PotientialVelocity.X < 0 && p.CurrentVelocity.X > 0))
            {
                changed = true;
            }

            if ((p.PotientialVelocity.Y > 0 && p.CurrentVelocity.Y < 0) ||
                (p.PotientialVelocity.Y < 0 && p.CurrentVelocity.Y > 0))
            {
                changed = true;
            }

            if ((p.PotientialVelocity.Z > 0 && p.CurrentVelocity.Z < 0) ||
                (p.PotientialVelocity.Z < 0 && p.CurrentVelocity.Z > 0))
            {
                changed = true;
            }

            if (!changed)
            {

                Vector3 EffectiveForce = p.ForceThisFrame + Vector3.Negate(p.CurrentVelocity) * airfriction;
                Vector3 AccelerationWithDrag = p.CurrentAcceleration + (EffectiveForce / p.mass);
                Vector3 VelocityWithDrag = p.CurrentVelocity + (AccelerationWithDrag * time);

                if ((p.PotientialAcceleration.X >= 0 && AccelerationWithDrag.X <= 0) ||
                    (p.PotientialAcceleration.X <= 0 && AccelerationWithDrag.X >= 0))
                {
                    AccelerationWithDrag.X = 0;
                }

                if ((p.PotientialAcceleration.Y >= 0 && AccelerationWithDrag.Y <= 0) ||
                    (p.PotientialAcceleration.Y <= 0 && AccelerationWithDrag.Y >= 0))
                {
                    AccelerationWithDrag.Y = 0;
                }

                if ((p.PotientialAcceleration.Z >= 0 && AccelerationWithDrag.Z <= 0) ||
                    (p.PotientialAcceleration.Z <= 0 && AccelerationWithDrag.Z >= 0))
                {
                    AccelerationWithDrag.Z = 0;
                }

                if ((p.PotientialVelocity.X >= 0 && VelocityWithDrag.X <= 0) ||
                    (p.PotientialVelocity.X <= 0 && VelocityWithDrag.X >= 0))
                {
                    VelocityWithDrag.X = 0;
                }

                if ((p.PotientialVelocity.Y >= 0 && VelocityWithDrag.Y <= 0) ||
                    (p.PotientialVelocity.Y <= 0 && VelocityWithDrag.Y >= 0))
                {
                    VelocityWithDrag.Y = 0;
                }

                if ((p.PotientialVelocity.Z >= 0 && VelocityWithDrag.Z <= 0) ||
                    (p.PotientialVelocity.Z <= 0 && VelocityWithDrag.Z >= 0))
                {
                    VelocityWithDrag.Z = 0;
                }

                p.PotientialAcceleration = AccelerationWithDrag;
                p.PotientialVelocity = VelocityWithDrag;

            }

            p.PotientialPosition = p.CurrentPosition + (p.PotientialVelocity * time);

			 */


			p.PotientialAcceleration = p.CurrentAcceleration + (p.ForceThisFrame / p.mass);
			p.PotientialVelocity = p.CurrentVelocity + (p.PotientialAcceleration * time);

			if (p.PotientialVelocity != Vector3.Zero)
			{
				Vector3 FrictionForce = p.PotientialVelocity * airfriction;
				Vector3 AccelerationDrag = (FrictionForce / p.mass);
				Vector3 VelocityDrag = (AccelerationDrag * time);

				if ((p.PotientialVelocity.X > 0 && p.PotientialVelocity.X - VelocityDrag.X <= 0) ||
					(p.PotientialVelocity.X < 0 && p.PotientialVelocity.X - VelocityDrag.X >= 0))
				{
					p.PotientialVelocity.X = 0;
					VelocityDrag.X = 0;
				}
				if ((p.PotientialVelocity.Y > 0 && p.PotientialVelocity.Y - VelocityDrag.Y <= 0) ||
					(p.PotientialVelocity.Y < 0 && p.PotientialVelocity.Y - VelocityDrag.Y >= 0))
				{
					p.PotientialVelocity.Y = 0;
					VelocityDrag.Y = 0;
				}
				if ((p.PotientialVelocity.Z > 0 && p.PotientialVelocity.Z - VelocityDrag.Z <= 0) ||
					(p.PotientialVelocity.Z < 0 && p.PotientialVelocity.Z - VelocityDrag.Z >= 0))
				{
					p.PotientialVelocity.Z = 0;
					VelocityDrag.Z = 0;
				}
				p.PotientialVelocity -= VelocityDrag;
			}

			p.PotientialPosition = p.CurrentPosition + (p.PotientialVelocity * time);

        }

		private void checkCollisions2(Point p, float time)
		{
			bool Collision = false;
			Collidable Collidable = null;
			float CollisionU = float.MaxValue;
			foreach (Collidable c in collision)
			{
				if (c.couldIntersect(p))
				{
					//Vector3[] lx = c.getNextCollisionVerticies();
					//Vector3 li;
					//float lu = CollisionMath.LineStaticTriangleIntersect(p.CurrentPosition, p.PotientialPosition, lx[0], lx[1], lx[2], out li);

					c.test(p);

					Vector3[] x = c.getNextCollisionVerticies();
					Vector3 i;
					float u = CollisionMath.LineStaticTriangleIntersect(p.CurrentPosition, p.PotientialPosition, x[0], x[1], x[2], out i);
					// If Collision ( u < 1 ) - Split Time and redo
					if (u > 0 && u < 1 /* && c.inBoundingBox(i)*/)
					{

						// should physics interact
						if (c.shouldPhysicsBlock(p))
						{

							if (!Collision)
							{
								Collidable = c;
								CollisionU = u;
								Collision = true;
							}
							else
							{
								if (u < CollisionU)
								{
									Collidable = c;
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
				fall(p, time * CollisionU);

				// sliding physics second half
				slide(p, time * (1 - CollisionU), Collidable);
			}
			else
			{

				p.NextAcceleration = p.PotientialAcceleration;
				p.NextPosition = p.PotientialPosition;
				p.NextVelocity = p.PotientialVelocity;

			}

		}

        private List<Collidable> CollisionChain = new List<Collidable>();
        private void checkCollisions(Point p, float time)
        {
            CollisionChain.Clear();
            bool Collision = false;
            Collidable Collidable = null;
            float CollisionU = float.MaxValue;

            foreach (Collidable c in collision)
            {
                if (c.couldIntersect(p))
                {
                    float u = c.didIntersect(p.CurrentPosition, p.PotientialPosition);
                    // If Collision ( u < 1 ) - Split Time and redo
                    if (u < 1)
                    {

                        // should physics interact
                        if (c.shouldPhysicsBlock(p))
                        {

							if (!Collision)
							{
								Collidable = c;
								CollisionU = u;
								Collision = true;
							}
							else
							{
								if (u < CollisionU)
								{
									Collidable = c;
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
                fall(p, time * CollisionU);

                // sliding physics second half
                slide(p, time * (1 - CollisionU), Collidable);
            }
            else
            {

                p.NextAcceleration = p.PotientialAcceleration;
                p.NextPosition = p.PotientialPosition;
                p.NextVelocity = p.PotientialVelocity;

            }

        }

            private void slide(Point p, float time, Collidable s)
		{
			Vector3 collisionNormal = s.Normal();

			while (s.DotNormal(p.NextPosition) <= 0)
			{
				p.NextPosition += (collisionNormal * 0.001f);
				//++DEBUG_BumpLoops;
			}

			// Stop Velocity in direction of the wall
			Vector3 NormalEffect = (collisionNormal * (p.NextVelocity.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(p.NextVelocity, collisionNormal).Length(), Vector3.Dot(p.NextVelocity, collisionNormal)))));
			p.NextVelocity = (p.NextVelocity - NormalEffect);
			s.ImpartVelocity(p.NextPosition, NormalEffect);

			//impact += NormalEffect.Length() * 0.1f;

			// forces
			Vector3 Force = p.ForceThisFrame;

			// normal force
			Vector3 NormalForce = (collisionNormal * (Force.Length() * (float)Math.Cos(Math.Atan2(Vector3.Cross(Force, collisionNormal).Length(), Vector3.Dot(Force, collisionNormal)))));
			Force = (Force - NormalForce);
			s.ApplyForce(p.NextPosition, NormalForce);

			// air friction
			if (p.NextVelocity.LengthSquared() > 0)
			{
				Vector3 Drag = Vector3.Negate(p.NextVelocity) * airfriction;
				Force += Drag;

				// surface friction !  F = uN

				//Force += Vector3.Negate(p.NextVelocity) * friction;

				Vector3 FrictionForce = Vector3.Normalize(Vector3.Negate(p.NextVelocity)) * (NormalForce.Length() * player.Traction.value);

				Vector3 MaxFriction = Vector3.Negate((p.NextVelocity / time) * p.mass);

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

			}


			// acceleration
			Vector3 Acceleration = p.NextAcceleration;
			Acceleration = Force / p.mass;

			// velocity
			Vector3 Velocity = p.NextVelocity;
			Velocity += Acceleration * time;

			// surface friction?

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

				slide(p, time * CollisionU, s);

				slide(p, time * (1 - CollisionU), CollisionTri);
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
					//++DEBUG_BumpLoops;
				}
			}
		}

        

    }
}
