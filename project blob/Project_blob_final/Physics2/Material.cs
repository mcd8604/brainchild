using System.Collections.Generic;

namespace Physics2
{
	public class Material
	{

		private float cling = 1;
		private float friction = 1;

		private static readonly Material defaultMaterial = new Material();

		private static readonly List<Material> materials = new List<Material>();

		public static Material getDefaultMaterial()
		{
			return defaultMaterial;
		}

		public static Material getMaterial(float p_cling, float p_friction)
		{
			foreach (Material m in materials)
			{
				if (m.cling == p_cling && m.friction == p_friction)
				{
					return m;
				}
			}
			Material ret = new Material(p_cling, p_friction);
			materials.Add(ret);
			return ret;
		}

		public float Cling
		{
			get
			{
				return cling;
			}
		}

		public float Friction
		{
			get
			{
				return friction;
			}
		}

		private Material() { }

		private Material(float p_cling, float p_friction)
		{
			cling = p_cling;
			friction = p_friction;
		}

	}
}
