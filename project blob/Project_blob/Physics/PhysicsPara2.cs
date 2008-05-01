using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    class PhysicsPara2 : PhysicsManager
    {
        private PhysicsSeq physicsMain;

        private System.Threading.Thread WorkerThread;

        private float runForTime = 0f;

        private bool run = true;

        private System.Diagnostics.Stopwatch PhysicsTimer = new System.Diagnostics.Stopwatch();
        private System.Diagnostics.Stopwatch WaitTimer = new System.Diagnostics.Stopwatch();
        private float waitTimeMsec = 0;
        private float physicsTimeMsec = 0;

        private const int physicsDivisor = 2;

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

        public PhysicsPara2()
		{

			physicsMain = new PhysicsSeq();

			WorkerThread = new System.Threading.Thread(delegate()
			{
				WaitTimer.Start();
				do
				{
                        float val = 10f - (float)PhysicsTimer.Elapsed.TotalMilliseconds;
                        if ( val > 0 ) {
						System.Threading.Thread.Sleep( (int)val );
                        }
                            
						if (!run)
						{
							return;
						}
					WaitTimer.Stop();
					waitTimeMsec += (float)WaitTimer.Elapsed.TotalMilliseconds;
					WaitTimer.Reset();
                    runForTime = (float)PhysicsTimer.Elapsed.TotalMilliseconds + (float)WaitTimer.Elapsed.TotalMilliseconds;
					PhysicsTimer.Start();
					try
					{
						for (int i = physicsDivisor; i > 0; --i)
						{
							physicsMain.doPhysics(runForTime * ((float)physicsMultiplier / (float)physicsDivisor));
                            //Console.WriteLine(i + ": " + PhysicsTimer.Elapsed.TotalMilliseconds);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex);
						break;
					}
					PhysicsTimer.Stop();
					physicsTimeMsec += (float)PhysicsTimer.Elapsed.TotalMilliseconds;
					PhysicsTimer.Reset();
					WaitTimer.Start();
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
