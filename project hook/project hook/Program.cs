namespace project_hook
{
	static class Program
	{

#if FINAL
		private const string outfilename = "err.log";
		private static System.IO.TextWriter writer;
#else
		private static System.IO.TextWriter writer = System.Console.Out;
#endif
		public static System.IO.TextWriter Out
		{
			get
			{
				return writer;
			}
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
#if FINAL
			using (writer = new System.IO.StreamWriter(outfilename))
			{
#endif
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
					Out.WriteLine("Exception occured: " + e);
					Out.WriteLine("Process Terminated.");
					System.Windows.Forms.MessageBox.Show("Sorry, A Fatal Exeception has occurred in Cell.  Please send the file err.log in the program directory to the developers for assistance.", "Fatal Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
#endif
#if FINAL
				Out.Flush();
			}
#endif
		}
	}
}
