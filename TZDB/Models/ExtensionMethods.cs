using System.Collections.Concurrent;
using System.Windows.Controls;
using System.Windows.Threading;

namespace NUIClockUpdater.Models
{
    static class ExtensionMethods
    {
        /// <summary>
        /// Checks if the provided string is empty or whitespace.
        /// </summary>
        /// <param name="str">The string to check for emptiness and whitespace.</param>
        public static bool IsEmptyOrWhiteSpace(this string value)
        {
            if (!value.Equals(string.Empty))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Partitioner<T> GetConsumingPartitioner<T>(this BlockingCollection<T> collection)
        {
            return new BlockingCollectionPartitioner<T>(collection);
        }
    }
}
