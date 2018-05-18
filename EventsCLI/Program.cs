using System;

namespace EventsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"Events on {(args.Length > 0 ? args[0] : "localhost")}, press q to quit, x to toggle printing xml";
            ConsoleKeyInfo key;
            var watcher = args.Length > 0 ? new Watcher("*[System[Provider[@Name='ASP.NET 4.0.30319.0']]]", args[0]) : new Watcher();
            watcher.StartWatch();
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.X)
                    watcher.ToggleXML();
                if (key.Key == ConsoleKey.Q || key.Key == ConsoleKey.Escape || (key.Key==ConsoleKey.F4 && key.Modifiers==ConsoleModifiers.Alt))
                    break;
            }
        }
    }
}
