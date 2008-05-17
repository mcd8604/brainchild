using System;
namespace Physics2
{
	[Serializable]
	public abstract class Task
	{

		public abstract void update(Body b, float time);

		public bool run = true;
		public bool Run
		{
			get { return run; }
			set { run = value; }
		}

	}
}
