using System;

namespace Project_blob
{
	internal static class Program
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
#if FINAL
			using ( new Log() )
			{
			try
			{
#endif
			using (ScreenManager game = new ScreenManager())
			{
				game.Run();
			}
#if FINAL
			}
			catch (System.Exception e)
			{
				Log.Out.WriteLine("Exception occured: " + e);
				Log.Out.WriteLine("Process Terminated.");
				System.Windows.Forms.MessageBox.Show("Sorry, A Fatal Exeception has occurred.  Please send the error.log file in the program directory to the developers for assistance.", "Fatal Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
			}
#endif
		}
	}
}

