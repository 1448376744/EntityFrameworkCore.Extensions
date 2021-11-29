using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.Query
{
    public interface IDapperQueryable<T1, T2>
    {
        IDapperQueryable<T1, T2> Skip(int count);
        IDapperQueryable<T1, T2> Take(int count);
        IDapperQueryable<T1, T2> Where(Expression<Func<T1, T2, bool>> expression);
        IDapperQueryable<T1, T2> Having(Expression<Func<T1, T2, bool>> expression);
        IDapperQueryable<T1, T2> OrderBy<TGroup>(Expression<Func<T1, T2, TGroup>> expression);
        IDapperQueryable<T1, T2> OrderByDescending<TGroup>(Expression<Func<T1, T2, TGroup>> expression);
        IDapperQueryable<T1, T2> GroupBy<TGroup>(Expression<Func<T1, T2, TGroup>> expression);
        IDapperQueryable<T1, T2> On(Expression<Func<T1, T2, JoinArray>> expression);
        int Count(int? commandTimeout = null);
        Task<int> CountAsync(int? commandTimeout = null);
        TResult Sum<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null);
        Task<TResult> SumAsync<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null);
        List<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null);
    }
}
