using System;
using System.Windows.Forms;

namespace project_hook
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{          

             DialogResult result = MessageBox.Show("Test(Yes) or Development (No)?", "Choose a version", MessageBoxButtons.YesNoCancel);

             if (result == DialogResult.Yes)
             {
                 using (Game1 game = new Game1())
                 {
                     game.Run();
                 }
             }
             else if (result == DialogResult.No)
             {
                 using (Game game = new Game())
                 {
                     game.Run();
                 }
             }
		}
	}
}
