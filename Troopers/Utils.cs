#nullable enable
using System;
using System.Linq;
using System.Threading;

namespace Troopers
{
    public class Utils
    {
        private Utils()
        {
        }

        public static Utils Instance { get; } = new Utils();

        /**
         * Generate String
         * Generate random string
         * @var chars: Characters for randomising.
         * @var length: Length of generated string.
         */
        public string GenerateString(string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", int length = 12)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void Log(string? message, bool newLine = true)
        {
            if (newLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
        }

        public static void Slog(string? message, int ms = 500)
        {
            Console.WriteLine(message);
            Thread.Sleep(ms);
        }

        public static string? Read()
        {
            return Console.ReadLine();
        }
    }
}