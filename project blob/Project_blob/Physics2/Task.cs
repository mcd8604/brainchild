using System;
namespace Physics2
{
	[Serializable]
	public abstract class Task
	{

		public abstract void update(Body b, float time);

		protected bool active = false;
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

	}
}
