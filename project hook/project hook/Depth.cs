
//This class contains the z-Buffering definitions for the sprites.  It is broken up into 9 layers.
namespace project_hook
{
	namespace Depth
	{

		public static class MenuLayer
		{

			public static float Cursor = 0.0f;
			public static float Highlight = 0.025f;
			public static float Text = 0.05f;
			public static float Background = 0.075f;

		}

		public static class HUDLayer
		{

			public static float Foreground = 0.1f;
			public static float Midground = 0.133f;
			public static float Background = 0.166f;

		}

		public static class GameLayer
		{
			public static float Cursor = 0.3f;

			public static float Explosion = 0.4f;

			public static float Shot = 0.45f;

			public static float PlayerShip = 0.5f;

			public static float Tail = 0.55f;

			public static float Ships = 0.6f;

			public static float TailBody = 0.65f;

			public static float Shields = 0.7f;

			public static float Environment = 0.8f;

			public static float Gate = 0.825f;

			public static float Trigger = 0.85f;
		}

		public static class BackGroundLayer
		{
			public static float Upper = 0.9f;
			public static float Background = 1.0f;

		}

	}
}
