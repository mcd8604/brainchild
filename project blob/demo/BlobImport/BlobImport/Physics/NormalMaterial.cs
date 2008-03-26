using System;
using System.Collections.Generic;
using System.Text;

namespace Physics
{
    class NormalMaterial : Material
    {

        public float getFriction()
        {
            return 1;
        }

        public float getBounce()
        {
            return 10;
        }

    }
}
