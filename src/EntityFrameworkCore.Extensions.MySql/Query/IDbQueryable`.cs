using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.Query
{
    public interface IDapperQueryable<T>
    {
        IDapperQueryable<T> Skip(int count);
        IDapperQueryable<T> Take(int count);
        IDapperQueryable<T> Where(Expression<Func<T, bool>> expression);
        IDapperQueryable<T> Having(Expression<Func<T, bool>> expression);
        IDapperQueryable<T> OrderBy<TGroup>(Expression<Func<T, TGroup>> expression);
        IDapperQueryable<T> OrderByDescending<TGroup>(Expression<Func<T, TGroup>> expression);
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
