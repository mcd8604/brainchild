using System.Collections.Generic;

namespace Physics2
{
	public abstract class PhysicsManager
	{

        public enum ParallelSetting { Always, Automatic, Never };

        public static ParallelSetting enableParallel = ParallelSetting.Automatic;

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

        public abstract float PWR
        {
            get;
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

        public abstract void AddBody(Body b);
        public abstract void AddBodys(IEnumerable<Body> b);

        /// <summary>
        /// Request that physics move all the objects ahead by some increment of time.
        /// </summary>
        /// <param name="TotalElapsedSeconds">The amount of time.</param>
        public abstract void update(float TotalElapsedSeconds);

        public abstract void stop();

	}
}
