﻿using EntityFrameworkCore.Extensions.Metadata;
using EntityFrameworkCore.Extensions.Query;
using Microsoft.EntityFrameworkCore.Infrastructure;
using EntityFrameworkCore.Extensions.SqlExpressions;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

        public static IDapperQueryable<T> Queryable<T>(this DbContext context, DbSet<T> set)
            where T : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T>(provider, model);
        }

        public static IDapperQueryable<T1, T2> Queryable<T1, T2>(this DbContext context, DbSet<T1> set1, DbSet<T2> set2)
            where T1 : class
            where T2 : class
        {
            var model = new EFCoreModel(context.Model);
            var provider = new DapperQueryProvider(context.Database);
            return new DapperQueryableImpl<T1,T2>(provider, model);
        }
    }
}
