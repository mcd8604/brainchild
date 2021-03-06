using System.Collections.Generic;

namespace Physics2
{

	public abstract class PhysicsManager
	{
		public enum ParallelSetting { Always, Automatic, Never };

		public static ParallelSetting enableParallel = ParallelSetting.Always;

		/// <summary>
		/// A multiplier for physics time.
		/// Think of it as changing the speed of a film on a projector.
		/// Note that due to rounding errors, physics becomes more erratic at high speeds.
		/// Setting to 0 effectively stops all physics from happening.
		/// Negative numbers are allowed, and physics will behave as if time was running backwards.
		/// This feature is not supported, or recommended.
		/// </summary>
		public float physicsMultiplier = 1f;

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

		public static float AirFriction = 1f;

		public abstract Player Player
		{
			get;
		}

		public abstract void AddBody(Body b);
		public abstract void AddBodys(IEnumerable<Body> b);

		public abstract float Time
		{
			get;
		}

		/// <summary>
		/// Request that physics move all the objects ahead by some increment of time.
		/// </summary>
		/// <param name="TotalElapsedSeconds">The amount of time.</param>
		public abstract void update(float TotalElapsedSeconds);

		public abstract void stop();

#if DEBUG
		public abstract int DEBUG_GetNumCollidables();

		public abstract int DEBUG_GetNumPoints();

		public abstract float PWR
		{
			get;
		}
#endif

	}
}
