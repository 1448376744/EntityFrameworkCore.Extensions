using EntityFrameworkCore.Extensions.Metadata;
using EntityFrameworkCore.Extensions.Query;

namespace Microsoft.EntityFrameworkCore
{
    public static class EntityFrameworkDbContextExtensions
    {
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
    }
}
