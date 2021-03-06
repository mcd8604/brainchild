using System;
using System.Collections.Generic;
using System.Text;
using Physics2;

namespace Project_blob
{
    public enum MaterialType
    {
        Default,
        Slick, 
        Sticky,
        SuperSticky
    }

    public struct MaterialInfo
    {
        public float Cling;
        public float Friction;
    }

    public static class MaterialFactory
    {
        public const float CLING_SLICK = 0f;
        public const float FRICTION_SLICK = 0f;

        public const float CLING_STICKY = 2.0f;
        public const float FRICTION_STICKY = 2.0f;

        public const float CLING_SUPER_STICKY = 7.0f;
        public const float FRICTION_SUPER_STICKY = 7.0f;

        private static Dictionary<MaterialType, Material> m_Materials = new Dictionary<MaterialType, Material>();

        public static Material GetPhysicsMaterial(MaterialType m)
        {
            Material material = Material.getDefaultMaterial();

            if ( m_Materials.ContainsKey( m ) )
            {
                return m_Materials[m];
            }

            if ( m == MaterialType.Slick )
            {
                material = Material.getMaterial( CLING_SLICK, FRICTION_SLICK );
            }
            else if ( m == MaterialType.Sticky )
            {
				material = Material.getMaterial(CLING_STICKY, FRICTION_STICKY);
            }
            else if (m == MaterialType.SuperSticky)
            {
				material = Material.getMaterial(CLING_SUPER_STICKY, FRICTION_SUPER_STICKY);
            }

            m_Materials.Add( m, material );

            return material;
        }

    }
}
