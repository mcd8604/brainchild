using Microsoft.Xna.Framework;

namespace Physics2
{
	public class GravityPoint : Task
	{

		private float Magnitude = 9.8f;
		private Vector3 Origin = Util.Zero;

		public GravityPoint() { }
		public GravityPoint(float p_Magnitude)
		{
			Magnitude = p_Magnitude;
		}
		public GravityPoint(float p_Magnitude, Vector3 p_Origin)
		{
			Magnitude = p_Magnitude;
			Origin = p_Origin;
		}

		public override void update(Body b, float time)
		{
			foreach (PhysicsPoint p in b.points)
			{
				p.AccelerationThisFrame += Vector3.Normalize(Origin - p.CurrentPosition) * Magnitude;
			}
		}

	}
}
