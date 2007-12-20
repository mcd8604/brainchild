using System;
using System.Collections.Generic;
using System.Text;

//This class contains the z-Buffering definitions for the sprites.  It is broken up into 9 layers.
namespace project_hook
{
    namespace Depth
    {
        public class BackGround
        {
            public static float Bottom = 0.0f;
            public static float Mid = 0.15f;
            public static float Top = 0.3f;
        }

        public class MidGround
        {
            public static float Bottom = 0.31f;
            public static float Mid = 0.46f;
            public static float Top = 0.61f;

        }

        public class ForeGround
        {
            public static float Bottom = 0.62f;
            public static float Mid = 0.77f;
            public static float Top = 0.92f;

        }

    }
}
