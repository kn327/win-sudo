using System;
using System.Collections.Generic;

namespace sudo.util
{
    public static class Flags
    {
        private static HashSet<string> _flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public const string DEBUG = "--debug";
        public const string SAVE_TEMP = "--save-temp";
        public const string SHOW_WINDOW = "--show-window";

        public static int Count => _flags.Count;
        public static IEnumerable<string> Values => _flags;
        public static bool Add(string key) => _flags.Add(key);
        public static bool Contains(string key) => _flags.Contains(key);
    }
}
