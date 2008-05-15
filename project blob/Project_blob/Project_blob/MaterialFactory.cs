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
        Sticky
    }

    public struct MaterialInfo
    {
        public float Cling;
        public float Friction;
    }

    public static class MaterialFactory
    {
        public const float CLING_SLICK = 0.1f;
        public const float FRICTION_SLICK = 0.1f;

        public const float CLING_STICKY = 2.0f;
        public const float FRICTION_STICKY = 2.0f;

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
                material = new Material( CLING_SLICK, FRICTION_SLICK );
            }
            else if ( m == MaterialType.Sticky )
            {
                material = new Material( CLING_STICKY, FRICTION_STICKY );
            }

            m_Materials.Add( m, material );

            return material;
        }
    }
}
