using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    public static class IEnumerableExtension
    {
        public static string Join(this IEnumerable<string> source, string separator)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }
            return source.Aggregate((string x, string y) => x + separator + y);
        }

        public static string Join<TSource>(this IEnumerable<TSource> soucre, string separator, Func<TSource, string> map)
        {
            if (soucre == null)
            {
                throw new ArgumentNullException("source");
            }
            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }
            TSource[] source = (soucre as TSource[]) ?? soucre.ToArray();
            return Join(source.Select(map), separator);
        }

        public static IEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> sources, params KeyValuePair<bool, Func<TSource, object>>[] keySelector)
        {
            if (sources == null)
            {
                throw new ArgumentNullException("sources");
            }
            IOrderedEnumerable<TSource> orderedEnumerable = null;
            int num = 0;
            for (int i = 0; i < keySelector.Length; i++)
            {
                KeyValuePair<bool, Func<TSource, object>> keyValuePair = keySelector[i];
                if (num == 0)
                {
                    orderedEnumerable = (keyValuePair.Key ? sources.OrderBy(keyValuePair.Value) : sources.OrderByDescending(keyValuePair.Value));
                }
                else if (orderedEnumerable != null)
                {
                    orderedEnumerable = (keyValuePair.Key ? orderedEnumerable.ThenBy(keyValuePair.Value) : orderedEnumerable.ThenByDescending(keyValuePair.Value));
                }
                num++;
            }
            return orderedEnumerable;
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> sources, Func<TSource, bool> predicate, Func<TSource, TKey> keySelector)
        {
            return sources.Where(predicate).OrderBy(keySelector);
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> sources, Func<TSource, bool> predicate, Func<TSource, TKey> keySelector)
        {
            return sources.Where(predicate).OrderByDescending(keySelector);
        }

        public static TElement MaxElement<TElement, TData>(this IEnumerable<TElement> source, Func<TElement, TData> selector) where TData : IComparable<TData>
        {
            return ComparableElement(source, selector, true);
        }

        public static TElement MinElement<TElement, TData>(this IEnumerable<TElement> source, Func<TElement, TData> selector) where TData : IComparable<TData>
        {
            return ComparableElement(source, selector, false);
        }

        public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Max();
        }

        public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Min();
        }

        public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum((decimal? result) => result.GetValueOrDefault());
        }

        public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Where(predicate).Select(selector).Sum();
        }

        public static bool Repeat<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Count(predicate) > 1;
        }

        public static bool Comparer<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return (from source in first
                from source1 in second
                where comparer.Equals(source, source1)
                select source).Any();
        }

        private static TElement ComparableElement<TElement, TData>(IEnumerable<TElement> source, Func<TElement, TData> selector, bool isMax) where TData : IComparable<TData>
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            bool flag = true;
            TElement result = default(TElement);
            TData other = default(TData);
            foreach (TElement item in source)
            {
                TData val = selector(item);
                if (flag || ((!isMax || val.CompareTo(other) > 0) && (isMax || val.CompareTo(other) <= 0)))
                {
                    flag = false;
                    other = val;
                    result = item;
                }
            }
            return result;
        }
    }

}
