//This class contains the z-Buffering definitions for the sprites.
namespace project_hook
{
	namespace Depth
	{
		internal static class MenuLayer
		{
			internal const float Cursor = 0.0f;
			internal const float Highlight = 0.025f;
			internal const float Text = 0.05f;
			internal const float Background = 0.075f;
		}
		internal static class HUDLayer
		{
			internal const float Bar = 0.1f;
			internal const float BarBackground = 0.133f;
			internal const float BarBackIcon = 0.15f;
			internal const float Text = 0.166f;
		}
		internal static class GameLayer
		{
			internal const float Cursor = 0.3f;
			internal const float Explosion = 0.4f;
			internal const float Shot = 0.45f;
			internal const float Shields = 0.475f;
			internal const float PlayerShip = 0.5f;
			internal const float Tail = 0.55f;
			internal const float TailBody = 0.6f;
			internal const float Turrets = 0.65f;
			internal const float Ships = 0.7f;
			internal const float Environment = 0.8f;
			internal const float Gate = 0.825f;
			internal const float Trigger = 0.85f;
		}
		internal static class BackGroundLayer
		{
			internal const float Blood = 0.9f;
			internal const float Background = 1.0f;
		}
	}
}
