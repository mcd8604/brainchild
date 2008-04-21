using Microsoft.Xna.Framework;

namespace Physics2
{
	public class GravityVector : Task
	{

		private Vector3 Gravity;

		public GravityVector()
		{
			Gravity = new Vector3(0f, -9.8f, 0f);
		}
		public GravityVector(float p_Magnitude)
		{
			Gravity = Vector3.Down * p_Magnitude;
		}
		public GravityVector(float p_Magnitude, Vector3 p_Direction)
		{
			Gravity = p_Direction * p_Magnitude;
		}

		public override void update(Body b)
		{
			foreach (Point p in b.getPoints())
			{
				p.AccelerationThisFrame += Gravity;
			}
		}

	}
}
