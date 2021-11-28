using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.Query
{
    public interface IDapperQueryable<T>
    {
        IDapperQueryable<T> Skip(int count, bool condition = true);
        IDapperQueryable<T> Take(int count, bool condition = true);
        IDapperQueryable<T> Where(Expression<Func<T, bool>> expression, bool condition = true);
        IDapperQueryable<T> Having(Expression<Func<T, bool>> expression, bool condition = true);
        IDapperQueryable<T> OrderBy<TGroup>(Expression<Func<T, TGroup>> expression, bool condition = true);
        IDapperQueryable<T> OrderByDescending<TGroup>(Expression<Func<T, TGroup>> expression, bool condition = true);
        IDapperQueryable<T> GroupBy<TGroup>(Expression<Func<T, TGroup>> expression);
        int Count(int? commandTimeout = null);
        Task<int> CountAsync(int? commandTimeout = null);
        TResult Sum<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        List<T> Select(int? commandTimeout = null);
        List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
        Task<List<T>> SelectAsync(int? commandTimeout = null);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null);
    }
}
