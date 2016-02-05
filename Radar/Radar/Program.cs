using System;

namespace Radar {
#if WINDOWS || XBOX
    static class Program
    {
        public static string PROCESS_WINDOW_TITLE = "World of Warcraft";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

