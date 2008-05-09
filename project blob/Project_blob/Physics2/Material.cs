namespace Physics2
{
	public class Material
	{

		private float Cling = 1;
		private float Friction = 1;

		private static Material defaultMaterial = new Material();

		public static Material getDefaultMaterial()
		{
			return defaultMaterial;
		}

		public Material() { }

		public Material(float cling, float friction)
		{
			Cling = cling;
			Friction = friction;
		}

		public float getCling()
		{
			return Cling;
		}

		public float getFriction()
		{
			return Friction;
		}

	}
}
