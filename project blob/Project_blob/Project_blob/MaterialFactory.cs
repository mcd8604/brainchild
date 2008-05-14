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

    public class MaterialFactory
    {
        private const float CLING_SLICK = 0.1f;
        private const float FRICTION_SLICK = 0.1f;

        private const float CLING_STICKY = 2.0f;
        private const float FRICTION_STICKY = 2.0f;

        protected MaterialFactory() { }

        public static Material GetPhysicsMaterial(MaterialType m)
        {
            Material material = Material.getDefaultMaterial();

            if ( m == MaterialType.Slick )
            {
                material = new Material( CLING_SLICK, FRICTION_SLICK );
            }
            else if ( m == MaterialType.Sticky )
            {
                material = new Material( CLING_STICKY, FRICTION_STICKY );
            }

            return material;
        }
    }
}
