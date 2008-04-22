using Microsoft.Xna.Framework;

namespace Physics2
{
	class GravityPoint : Task
	{

		private float Magnitude = 9.8f;
		private Vector3 Origin = Vector3.Zero;

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

		public override void update(Body b)
		{
			foreach (Point p in b.getPoints())
			{
				p.AccelerationThisFrame += Vector3.Normalize(Origin - p.CurrentPosition) * Magnitude;
			}
		}

	}
}