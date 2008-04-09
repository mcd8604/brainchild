namespace Physics
{
    public abstract class Material
    {

		private static Material DefaultMaterial = new MaterialBasic();

		public static Material getDefaultMaterial()
		{
			return DefaultMaterial;
		}

        public abstract float getFriction();

    }
}
