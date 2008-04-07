using Microsoft.Xna.Framework;

namespace Physics
{
	class GravityPoint : Gravity
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

		public Vector3 getForceOn(Point p)
		{
			return Vector3.Normalize(Origin - p.CurrentPosition) * (Magnitude * p.mass);
		}

	}
}
