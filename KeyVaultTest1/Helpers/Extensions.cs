using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace KeyVaultTest1.Helpers
{
    public static class Extensions
    {
        public static void CheckNotNull<T>(this T container) where T : class
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            NullChecker<T>.Check(container);
        }

        private static class NullChecker<T> where T : class
        {
            private static readonly List<Func<T, bool>> checkers;
            private static readonly List<string> names;

            static NullChecker()
            {
                checkers = new List<Func<T, bool>>();
                names = new List<string>();

                foreach (string name in typeof(T).GetConstructors()[0]
                                                 .GetParameters()
                                                 .Select(p => p.Name))
                {
                    names.Add(name);
                    PropertyInfo property = typeof(T).GetProperty(name);

                    if (property.PropertyType.IsValueType)
                    {
                        throw new ArgumentException
                            ("Property " + property + " is a value type");
                    }
                    ParameterExpression param = Expression.Parameter(typeof(T), "container");
                    Expression propertyAccess = Expression.Property(param, property);
                    Expression nullValue = Expression.Constant(null, property.PropertyType);
                    Expression equality = Expression.Equal(propertyAccess, nullValue);
                    var lambda = Expression.Lambda<Func<T, bool>>(equality, param);
                    checkers.Add(lambda.Compile());
                }
            }

            internal static void Check(T item)
            {
                for (int i = 0; i < checkers.Count; i++)
                {
                    if (checkers[i](item))
                    {
                        throw new ArgumentNullException(names[i]);
                    }
                }
            }
        }
    }
}
