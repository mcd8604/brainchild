
#define TIMED

using System;
using System.Collections.Generic;
using System.Text;

namespace Physics2
{
	internal class PhysicsParallel : PhysicsManager
	{

		private PhysicsSeq physicsMain;

		private System.Threading.Thread WorkerThread;

		private float runForTime = 0f;

		private bool run = true;

#if DEBUG
		public override int DEBUG_GetNumCollidables()
		{
			return physicsMain.DEBUG_GetNumCollidables();
		}

#if TIMED
		private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
		private float waitTimeMsec = 0;
		private float physicsTimeMsec = 0;

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
#endif
#endif

		public PhysicsParallel()
		{
			physicsMain = new PhysicsSeq();

			WorkerThread = new System.Threading.Thread(delegate()
			{
#if DEBUG && TIMED
				timer.Start();
#endif
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
#if DEBUG && TIMED
					timer.Stop();
					waitTimeMsec += (float)timer.Elapsed.TotalMilliseconds;
					timer.Reset();
					timer.Start();
#endif
					try
					{
						physicsMain.doPhysics(runForTime * physicsMultiplier);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Internal Physics Exception:");
						Console.WriteLine(ex);
						Console.WriteLine("-> Someone broke physics <-  See exception above:");
						break;
					}
#if DEBUG && TIMED
					timer.Stop();
					physicsTimeMsec += (float)timer.Elapsed.TotalMilliseconds;
					timer.Reset();
					timer.Start();
#endif
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

			if (TotalElapsedSeconds == 0f)
			{
				return;
			}

			while (runForTime != 0f)
			{
				lock (this)
				{
					System.Threading.Monitor.Pulse(this);
					System.Threading.Monitor.Wait(this);
				}
			}

			foreach (Body b in physicsMain.bodies)
			{
				b.updatePosition();
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

		public override float Time
		{
			get
			{
				return physicsMain.Time;
			}
		}
	}
}
