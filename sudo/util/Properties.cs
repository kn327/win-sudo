using System;
using System.Collections.Generic;

namespace sudo.util
{
    public static class Properties
    {
        private static Dictionary<string, string> _properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static HashSet<string> _isDefault = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public const string STDOUT = "-stdout";
        public const string STDERR = "-stderr";

        public static int Count => _properties.Count;
        public static IEnumerable<KeyValuePair<string, string>> Values => _properties;

        public static void Add(string key, string value) => _properties.Add(key, value);

        public static bool IsDefault(string key) => _isDefault.Contains(key);

        public static string Get(string key, string defaultValue) => Get(key, () => defaultValue);
        public static string Get(string key, Func<string> defaultValueFactory)
        {
            string value;
            if (!_properties.TryGetValue(key, out value))
            {
                value = _properties[key] = defaultValueFactory();
                _isDefault.Add(key);
            }

            return value;
        }
    }
}
