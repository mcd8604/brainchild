//This class contains the z-Buffering definitions for the sprites.  It is broken up into 9 layers.
namespace project_hook
{
	namespace Depth
	{
		internal static class MenuLayer
		{
			internal static float Cursor = 0.0f;
			internal static float Highlight = 0.025f;
			internal static float Text = 0.05f;
			internal static float Background = 0.075f;
		}
		internal static class HUDLayer
		{
			internal static float Foreground = 0.1f;
			internal static float Midground = 0.133f;
			internal static float Background = 0.166f;
		}
		internal static class GameLayer
		{
			internal static float Cursor = 0.3f;
			internal static float Explosion = 0.4f;
			internal static float Shot = 0.45f;
			internal static float Shields = 0.475f;
			internal static float PlayerShip = 0.5f;
			internal static float Tail = 0.55f;
			internal static float TailBody = 0.6f;
			internal static float Turrets = 0.65f;
			internal static float Ships = 0.7f;
			internal static float Environment = 0.8f;
			internal static float Gate = 0.825f;
			internal static float Trigger = 0.85f;
		}
		internal static class BackGroundLayer
		{
			internal static float Blood = 0.9f;
			internal static float Background = 1.0f;
		}
	}
}
