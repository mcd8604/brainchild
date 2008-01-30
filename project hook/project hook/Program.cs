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

			//DialogResult result = MessageBox.Show("Test(Yes) or Development (No)?", "Choose a version", MessageBoxButtons.YesNoCancel);
			using (Game game = new Game())
			{
				game.Run();
			}
			/*
			if (result == DialogResult.Yes)
			{
				using (Game1 game = new Game1())
				{
					game.Run();
				}
			}
			else if (result == DialogResult.No)
			{
				
			}
			 * */
		}
	}
}
