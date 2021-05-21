using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudo.util
{
    public static class ArrayUtil
    {
        public static void ShrinkRight<T>(ref T[] input, int count) {
            if (count > input.Length)
                throw new ArgumentException($"new size is bigger than the original arr.");

            if (count <= 0)
                throw new ArgumentException($"Start index must be greater than or equal to 1.");

            for (int i = 0; i < input.Length - count; i++)
                input[i] = input[i + count];

            Array.Resize(ref input, input.Length - count);
        }
    }
}
