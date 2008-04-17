using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
	public abstract class PhysicsManager
	{

		public enum ParallelSetting { Always, Automatic, Never };

		public static ParallelSetting enableParallel = ParallelSetting.Automatic;

		public static PhysicsManager getInstance()
		{
			switch (enableParallel)
			{
				case ParallelSetting.Always:
					return new PhysicsParallel();
				case ParallelSetting.Automatic:
					if (System.Environment.ProcessorCount > 1)
					{
						return new PhysicsParallel();
					}
					else
					{
						return new PhysicsSeq();
					}
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

		public virtual float PWR
		{
			get
			{
				return 1f;
			}
		}

	}
}
