using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.Query
{
    public interface IDapperQueryable<T1, T2, T3, T4>
    {
        IDapperQueryable<T1, T2, T3, T4> Skip(int count);
        IDapperQueryable<T1, T2, T3, T4> Take(int count);
        IDapperQueryable<T1, T2, T3, T4> On(Expression<Func<T1, T2, T3, T4, JoinArray>> expression);
        IDapperQueryable<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> expression);
        IDapperQueryable<T1, T2, T3, T4> Having(Expression<Func<T1, T2, T3, T4, bool>> expression);
        IDapperQueryable<T1, T2, T3, T4> OrderBy<TGroup>(Expression<Func<T1, T2, T3, T4, TGroup>> expression);
        IDapperQueryable<T1, T2, T3, T4> OrderByDescending<TGroup>(Expression<Func<T1, T2, T3, T4, TGroup>> expression);
        IDapperQueryable<T1, T2, T3, T4> GroupBy<TGroup>(Expression<Func<T1, T2, T3, T4, TGroup>> expression);
        int Count(int? commandTimeout = null);
        Task<int> CountAsync(int? commandTimeout = null);
        TResult Sum<TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression, int? commandTimeout = null);
        Task<TResult> SumAsync<TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression, int? commandTimeout = null);
        List<TResult> Select<TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression, int? commandTimeout = null);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T1, T2, T3, T4, TResult>> expression, int? commandTimeout = null);
    }
}
