namespace Physics2
{
    public class Material
    {

		private float cling = 1;
		private float friction = 1;

		private static Material defaultMaterial = new Material();

		public static Material getDefaultMaterial()
		{
			return defaultMaterial;
		}

		public Material() { }

        public float getCling()
        {
			return cling;
        }

		public float getFriction()
		{
			return friction;
		}

    }
}
