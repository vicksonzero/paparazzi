using System.Collections.Generic;

namespace DicksonMd.Extensions
{
    public static class ListExtensions
    {
        public static Dictionary<string, int> CountDuplicates(this IEnumerable<string> list)
        {
            var result = new Dictionary<string, int>();

            foreach (var item in list)
            {
                result[item] = result.TryGetValue(item, out var value)
                    ? value + 1
                    : 1;
            }

            return result;
        }
    }
}