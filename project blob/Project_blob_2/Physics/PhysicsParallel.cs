using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
	class PhysicsParallel : PhysicsManager
	{

		private PhysicsSeq physicsMain;

		private System.Threading.Thread WorkerThread;

		private float runForTime = 0f;

		private bool run = true;

		private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
		private float waitTimeMsec = 0;
		private float physicsTimeMsec = 0;

		private const int physicsDivisor = 1;
		//public float physicsMultiplier = 1f;

		public override float PWR
		{
			get
			{
				if ((waitTimeMsec + physicsTimeMsec) == 0)
				{
					return 0;
				}
				else
				{
					float pwr = physicsTimeMsec / (waitTimeMsec + physicsTimeMsec);
					//if (physicsTimeMsec > 1000 || waitTimeMsec > 1000)
					//{
					//	physicsTimeMsec = 0;
					//	waitTimeMsec = 0;
					//}
					physicsTimeMsec *= 0.5f;
					waitTimeMsec *= 0.5f;
					return pwr;
				}
			}
		}

		public PhysicsParallel()
		{

			physicsMain = new PhysicsSeq();

			WorkerThread = new System.Threading.Thread(delegate()
			{
				timer.Start();
				do
				{
					do
					{
						lock (this)
						{
							System.Threading.Monitor.Pulse(this);
							System.Threading.Monitor.Wait(this);
						}
						if (!run)
						{
							return;
						}
					} while (runForTime == 0f);
					timer.Stop();
					waitTimeMsec += (float)timer.Elapsed.TotalMilliseconds;
					timer.Reset();
					timer.Start();
					try
					{
						for (int i = physicsDivisor; i > 0; --i)
						{
							physicsMain.doPhysics(runForTime * (physicsMultiplier / (float)physicsDivisor));
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex);
						break;
					}
					timer.Stop();
					physicsTimeMsec += (float)timer.Elapsed.TotalMilliseconds;
					timer.Reset();
					timer.Start();
					runForTime = 0f;
					lock (this) System.Threading.Monitor.Pulse(this);
				} while (run);

			});
			WorkerThread.IsBackground = true;
			WorkerThread.Name = "Physics Thread";
			WorkerThread.Priority = System.Threading.ThreadPriority.AboveNormal;
			WorkerThread.Start();

		}

		public override void update(float TotalElapsedSeconds)
		{
			while (runForTime != 0f)
			{
				lock (this)
				{
					System.Threading.Monitor.Pulse(this);
					System.Threading.Monitor.Wait(this);
				}
			}
			foreach (Collidable c in physicsMain._eventsToTrigger)
			{
				c.TriggerEvents();
			}
			if (physicsMain == null)
			{
				return;
			}
			physicsMain._eventsToTrigger.Clear();

			// Don't call physics after you've stopped it!
			foreach (Point p in physicsMain.points)
			{
				p.updatePosition();
			}

			runForTime = TotalElapsedSeconds;
			lock (this) System.Threading.Monitor.Pulse(this);
		}

		public override void stop()
		{

			run = false;
			physicsMain = null;
			lock (this) System.Threading.Monitor.Pulse(this);

		}

		public override int DEBUG_GetNumCollidables()
		{
			return physicsMain.DEBUG_GetNumCollidables();
		}
		public override void AddBody(Body b)
		{
			physicsMain.AddBody(b);
		}
		public override void AddBodys(IEnumerable<Body> b)
		{
			physicsMain.AddBodys(b);
		}
		public override void AddCollidable(Collidable c)
		{
			physicsMain.AddCollidable(c);
		}
		public override void AddCollidables(IEnumerable<Collidable> c)
		{
			physicsMain.AddCollidables(c);
		}
		public override void AddGravity(Gravity g)
		{
			physicsMain.AddGravity(g);
		}
		public override void AddPoint(Point p)
		{
			physicsMain.AddPoint(p);
		}
		public override void AddPoints(IEnumerable<Point> p)
		{
			physicsMain.AddPoints(p);
		}
		public override void AddSpring(Spring s)
		{
			physicsMain.AddSpring(s);
		}
		public override void AddSprings(IEnumerable<Spring> s)
		{
			physicsMain.AddSprings(s);
		}
		public override float AirFriction
		{
			get
			{
				return physicsMain.AirFriction;
			}
			set
			{
				physicsMain.AirFriction = value;
			}
		}
		public override Player Player
		{
			get
			{
				// Don't call physics after you've stopped it!
				return physicsMain.Player;
			}
		}

	}
}
