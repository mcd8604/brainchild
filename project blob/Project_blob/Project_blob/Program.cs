using System;

namespace Project_blob
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ScreenManager game = new ScreenManager())
            {
                game.Run();
            }
        }
    }
}

