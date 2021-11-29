using EntityFrameworkCore.Extensions.Query;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
    public static class IDbQueryableExtensions
    {
        #region one
        public static TResult First<TResult>(this IDapperQueryable<TResult> queryable)
        {
            return queryable.Take(1).ToList().First();
        }
        public static async Task<TResult> FirstAsync<TResult>(this IDapperQueryable<TResult> queryable)
        {
            var list = await queryable.Take(1).ToListAsync();
            return list.First();
        }
        public static TResult? FirstOrDefault<TResult>(this IDapperQueryable<TResult> queryable)
        {
            return queryable.Take(1).ToList().FirstOrDefault();
        }
        public static async Task<TResult?> FirstOrDefaultAsync<TResult>(this IDapperQueryable<TResult> queryable)
        {
            var list = await queryable.Take(1).ToListAsync();
            return list.FirstOrDefault();
        }
        public static TResult Single<TResult>(this IDapperQueryable<TResult> queryable)
        {
            return queryable.Take(1).ToList().Single();
        }
        public static async Task<TResult> SingleAsync<TResult>(this IDapperQueryable<TResult> queryable)
        {
            var list = await queryable.Take(1).ToListAsync();
            return list.First();
        }
        public static TResult? SingleOrDefault<TResult>(this IDapperQueryable<TResult> queryable)
        {
            return queryable.Take(1).ToList().SingleOrDefault();
        }
        public static async Task<TResult?> SingleOrDefaultAsync<TResult>(this IDapperQueryable<TResult> queryable)
        {
            var list = await queryable.Take(1).ToListAsync();
            return list.FirstOrDefault();
        }
        public static List<TResult> ToList<TResult>(this IDapperQueryable<TResult> queryable)
        {
            return queryable.Select().ToList();
        }

        public static List<TResult> ToList<T, TResult>(this IDapperQueryable<T> queryable, Expression<Func<T, TResult>> expression)
        {
            return queryable.Select(expression).ToList();
        }
        public static async Task<List<TResult>> ToListAsync<TResult>(this IDapperQueryable<TResult> queryable)
        {
            var list = await queryable.SelectAsync();
            return list;
        }
        public static async Task<List<TResult>> ToListAsync<T, TResult>(this IDapperQueryable<T> queryable, Expression<Func<T, TResult>> expression)
        {
            var list = await queryable.SelectAsync(expression);
            return list;
        }
        #endregion
    }
}
