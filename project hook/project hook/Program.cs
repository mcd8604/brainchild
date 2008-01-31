using System;

namespace project_hook
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
#if DEBUG
			Collision.SelfTest();
#endif
			using (Game game = new Game())
			{
				game.Run();
			}
		}
	}
}
