using System;
using System.Collections.Generic;

namespace sudo.util
{
    public static class DictUtil
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> defaultFactory)
        {
            TValue value;
            if (!dict.TryGetValue(key, out value))
                value = dict[key] = defaultFactory();

            return value;
        }
    }
}
