using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
	public abstract class PhysicsManager
	{

		public static bool enableParallel = true;

		public static PhysicsManager getInstance()
		{
            int i = System.Environment.ProcessorCount;
            if (i > 1 && enableParallel)
            {
                Console.WriteLine("More than one processor core; There are " + i + "; Parallelize!");
				return new PhysicsParallel();
            }
			return new PhysicsSeq();
		}

        public abstract int DEBUG_GetNumCollidables();

        public abstract float AirFriction
        {
            get;
            set;
        }

        public abstract Player Player
        {
            get;
        }

        public abstract void AddGravity(Gravity g);

        public abstract void AddCollidable(Collidable c);
        public abstract void AddCollidables(IEnumerable<Collidable> c);

        public abstract void AddPoint(Point p);
        public abstract void AddPoints(IEnumerable<Point> p);

        public abstract void AddSpring(Spring s);
        public abstract void AddSprings(IEnumerable<Spring> s);

        public abstract void AddBody(Body b);
        public abstract void AddBodys(IEnumerable<Body> b);

        public abstract void update(float TotalElapsedSeconds);

		public virtual void stop() { }

	}
}
