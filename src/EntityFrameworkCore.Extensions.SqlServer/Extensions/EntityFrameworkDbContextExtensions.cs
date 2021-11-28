using EntityFrameworkCore.Extensions.Metadata;
using EntityFrameworkCore.Extensions.Query;
using EntityFrameworkCore.Extensions.SqlExpressions;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
    public static class EntityFrameworkDbContextExtensions
    {
        public static IDapperQueryable<T> CreateSingleQuery<T>(this DbContext context)
          where T : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T>(provider, model);
        }

        public static IDapperQueryable<T> CreateSingleQuery<T>(this DbContext context, DbSet<T> set)
            where T : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T>(provider, model);
        }
   
        public static IDapperQueryable<T1, T2> Join<T1, T2>(this IDapperQueryable<T1> queryable, Expression<Func<T1, T2, bool>> expression)
          where T1 : class
          where T2 : class
        {
            var query = queryable as DapperQueryableImpl<T1>;
            if (query == null)
            {
                throw new NotSupportedException();
            }
            var multiple = new DapperQueryableImpl<T1, T2>(query.Provider, query.Context.Model);
            multiple.Join(expression);
            return multiple;
        }

        public static IDapperQueryable<T1, T2> LeftJoin<T1, T2>(this IDapperQueryable<T1> queryable, Expression<Func<T1, T2, bool>> expression)
         where T1 : class
         where T2 : class
        {
            var query = queryable as DapperQueryableImpl<T1>;
            if (query == null)
            {
                throw new NotSupportedException();
            }
            var multiple = new DapperQueryableImpl<T1, T2>(query.Provider, query.Context.Model);
            multiple.LeftJoin(expression);
            return multiple;
        }

        public static IDapperQueryable<T1, T2> RightJoin<T1, T2>(this IDapperQueryable<T1> queryable, Expression<Func<T1, T2, bool>> expression)
        where T1 : class
        where T2 : class
        {
            var query = queryable as DapperQueryableImpl<T1>;
            if (query == null)
            {
                throw new NotSupportedException();
            }
            var multiple = new DapperQueryableImpl<T1, T2>(query.Provider, query.Context.Model);
            multiple.RightJoin(expression);
            return multiple;
        }

        public static IDapperQueryable<T1, T2> Join<T1, T2>(this IDapperQueryable<T1> queryable, DbSet<T2> table1, Expression<Func<T1, T2, bool>> expression)
           where T1 : class
           where T2 : class
        {
            var query = queryable as DapperQueryableImpl<T1>;
            if (query == null)
            {
                throw new NotSupportedException();
            }
            var multiple = new DapperQueryableImpl<T1, T2>(query.Provider, query.Context.Model);
            multiple.Join(expression);
            return multiple;
        }

        public static IDapperQueryable<T1, T2> LeftJoin<T1, T2>(this IDapperQueryable<T1> queryable, DbSet<T2> table1, Expression<Func<T1, T2, bool>> expression)
          where T1 : class
          where T2 : class
        {
            var query = queryable as DapperQueryableImpl<T1>;
            if (query == null)
            {
                throw new NotSupportedException();
            }
            var multiple = new DapperQueryableImpl<T1, T2>(query.Provider, query.Context.Model);
            multiple.LeftJoin(expression);
            return multiple;
        }

        public static IDapperQueryable<T1, T2> RightJoin<T1, T2>(this IDapperQueryable<T1> queryable, DbSet<T2> table1, Expression<Func<T1, T2, bool>> expression)
         where T1 : class
         where T2 : class
        {
            var query = queryable as DapperQueryableImpl<T1>;
            if (query == null)
            {
                throw new NotSupportedException();
            }
            var multiple = new DapperQueryableImpl<T1, T2>(query.Provider, query.Context.Model);
            multiple.RightJoin(expression);
            return multiple;
        }
    }
}
