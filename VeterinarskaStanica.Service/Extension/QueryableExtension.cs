using System;
using System.Linq;
using System.Linq.Expressions;

namespace VeterinarskaStanica.Service.Extension
{
    public static class QueryableExtension
    {

        /// <summary>
        /// Extension for database order by dynamic
        /// </summary>
        /// <returns>The by dynamic.</returns>
        /// <param name="query">Query.</param>
        /// <param name="orderByMember">Order by member.</param>
        /// <param name="direction">Direction.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, string direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);

            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(typeof(Queryable), direction.Equals("asc") ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), memberAccess.Type }, query.Expression, Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }

        /// <summary>
        /// Order query by dinamic data and get page
        /// </summary>
        /// <returns>The by page.</returns>
        /// <param name="query">Query.</param>
        /// <param name="orderByMember">Order by member.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="page">Page.</param>
        /// <param name="pageSize">Page size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IQueryable<T> OrderByPage<T>(this IQueryable<T> query, string orderByMember, string direction, int page, int pageSize)
        {
            // Calculate skip
            var skip = (page - 1) * pageSize;

            return query.OrderByDynamic(orderByMember, direction).Skip(skip).Take(pageSize);
        }
    }
}
