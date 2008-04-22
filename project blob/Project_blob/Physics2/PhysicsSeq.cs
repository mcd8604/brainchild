using System;
using System.Collections.Generic;
using System.Text;

namespace Physics2
{
	class PhysicsSeq : PhysicsManager
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

		private int DEBUG_NumCollidables = 0;
		public override int DEBUG_GetNumCollidables()
		{
			return DEBUG_NumCollidables;
		}

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
			foreach (Body b in bodies)
			{
				b.update(TotalElapsedSeconds);
			}

			// Solve for the 'Actual' potential positions based on force and acceleration
            foreach (Body b in bodies)
            {
                b.SolveForNextPosition(TotalElapsedSeconds);
            }

			// Check BoundingBoxes, Find collisions, add to collision list

			// Evaluate collsion list, call onCollsion, set NextPosition

		}

	}
}
