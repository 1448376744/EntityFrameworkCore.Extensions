using EntityFrameworkCore.Extensions.Metadata;
using EntityFrameworkCore.Extensions.Query;
using System.Data;

namespace Microsoft.EntityFrameworkCore
{
    public static class EntityFrameworkDbContextExtensions
    {
        #region queryable
        public static IDapperQueryable<T> Queryable<T>(this DbContext context, DbSet<T>? set = default)
            where T : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T>(provider, model);
        }

        public static IDapperQueryable<T1, T2> Queryable<T1, T2>(this DbContext context, DbSet<T1>? set1 = default, DbSet<T2>? set2 = default)
            where T1 : class
            where T2 : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T1, T2>(provider, model);
        }

        public static IDapperQueryable<T1, T2, T3> Queryable<T1, T2, T3>(this DbContext context, DbSet<T1>? set1 = default, DbSet<T2>? set2 = default, DbSet<T3>? set3 = default)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T1, T2, T3>(provider, model);
        }

        public static IDapperQueryable<T1, T2, T3, T4> Queryable<T1, T2, T3, T4>(this DbContext context, DbSet<T1>? set1 = default, DbSet<T2>? set2 = default, DbSet<T3>? set3 = default, DbSet<T4>? set4 = default)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T1, T2, T3, T4>(provider, model);
        }
        #endregion

        #region dapper
        public static int Execute(this DbContext context, string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.Execute(sql, param, commandTimeout, commandType);
        }
        public static Task<int> ExecuteAsync(this DbContext context, string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.ExecuteAsync(sql, param, commandTimeout, commandType);
        }
        public static T ExecuteScalar<T>(this DbContext context, string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.ExecuteScalar<T>(sql, param, commandTimeout, commandType);
        }
        public static Task<T> ExecuteScalarAsync<T>(this DbContext context, string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.ExecuteScalarAsync<T>(sql, param, commandTimeout, commandType);
        }
        public static IEnumerable<dynamic> Query(this DbContext context, string sql, object? param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.Query(sql, param, buffered, commandTimeout, commandType);
        }
        public static Task<IEnumerable<dynamic>> QueryAsync(this DbContext context, string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.QueryAsync(sql, param, commandTimeout, commandType);
        }
        public static IEnumerable<T> Query<T>(this DbContext context, string sql, object? param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.Query<T>(sql, param, buffered, commandTimeout, commandType);
        }
        public static Task<IEnumerable<T>> QueryAsync<T>(this DbContext context, string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var provider = new DapperQueryProvider(context.Database);
            return provider.QueryAsync<T>(sql, param, commandTimeout, commandType);
        }
        #endregion
    }
}
