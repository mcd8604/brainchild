namespace project_hook
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
#if DEBUG && TEST
			Collision.SelfTest();
#endif
#if CATCH
			try
			{
#endif
			using (Game game = new Game())
			{
				game.Run();
			}
#if CATCH
			}
			catch (System.Exception e)
			{
				Game.Out.WriteLine("Exception occured: " + e);
				Game.Out.WriteLine("Process Terminated.");
			}
#endif
		}
	}
}
