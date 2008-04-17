using System;
using System.Collections.Generic;
using System.Text;

namespace Physics2
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

		public override float PWR
		{
			get
			{
				float pwr = physicsTimeMsec / (waitTimeMsec + physicsTimeMsec);
				if (physicsTimeMsec > 1000 || waitTimeMsec > 1000)
				{
					physicsTimeMsec = 0;
					waitTimeMsec = 0;
				}
				return pwr;
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
						lock (this) System.Threading.Monitor.Wait(this);
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
						physicsMain.doPhysics(runForTime);
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
				} while (run);

			});
			WorkerThread.IsBackground = true;
			WorkerThread.Name = "Physics Thread";
			WorkerThread.Priority = System.Threading.ThreadPriority.AboveNormal;
			WorkerThread.Start();

		}

		public override void update(float TotalElapsedSeconds)
		{

			foreach (Body b in physicsMain.bodies)
			{
				b.update();
			}

			runForTime = TotalElapsedSeconds;
			lock (this) System.Threading.Monitor.Pulse(this);
		}

		public override void stop() {

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
			get { return physicsMain.Player; }
		}
		
	}
}
