using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Prototype.Providers
{
    public static class LambdaExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static List<T> CloneList<T>(this IEnumerable<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static ListSortable<T> ToListSortable<T>(this IEnumerable<T> TSource)
        {
            var _TTarget = new ListSortable<T>();
            _TTarget.AddRange(TSource);

            return _TTarget;
        }

        public static string GetName<T>(Expression<Func<T>> _expression)
        {
            var body = _expression.Body as MemberExpression;
            return body.Member.Name;
        }

        public static string GetName<T>(this T item) where T : class
        {
            return Cache<T>.Name;
        }

        private static class Cache<T>
        {
            public static readonly string Name;

            static Cache()
            {
                var properties = typeof(T).GetProperties()[0];
                Name = properties.Name;
            }
        }
    }
}
