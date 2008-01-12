
//This class contains the z-Buffering definitions for the sprites.  It is broken up into 9 layers.
namespace project_hook
{
    namespace Depth
    {
        public class ForeGround
        {
            public static float Top = 0.0f;
            public static float Mid = 0.15f;
            public static float Bottom = 0.3f;
        }

        public class MidGround
        {
            public static float Top = 0.31f;
            public static float Mid = 0.46f;
            public static float Bottom = 0.61f;

        }

        public class BackGround
        {
            public static float Top = 0.62f;
            public static float Mid = 0.77f;
            public static float Bottom = 0.92f;

        }

    }
}
