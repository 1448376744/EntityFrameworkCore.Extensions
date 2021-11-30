using EntityFrameworkCore.Extensions.Metadata;
using EntityFrameworkCore.Extensions.SqlExpressions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;

namespace EntityFrameworkCore.Extensions.Query
{
    public class DapperQueryableImpl<T1, T2> : IDapperQueryable<T1, T2>
    {
        private readonly SqlExpressionContext Context;

        private readonly DapperQueryProvider _provider;

        private readonly SqlExpressionCollection _expressions = new();

        private int _takeCount = 0;

        private int _skipCount = 0;

        public DapperQueryableImpl(DapperQueryProvider provider, IModelEx model)
        {
            _provider = provider;
            Context = new SqlExpressionContext(model);
            Context.TableAlias.Add(typeof(T1), "t1");
            Context.TableAlias.Add(typeof(T2), "t2");
        }
        public IDapperQueryable<T1, T2> On(Expression<Func<T1, T2, JoinArray>> expression)
        {
            var join = new JoinSqlExpression(Context, expression);
            _expressions.Add(join);
            return this;
        }
        public IDapperQueryable<T1, T2> Join(Expression<Func<T1, T2, bool>> expression)
        {
            var ex = Expression.Constant(new JoinArray(JoinType.Inner, expression));
            var join = new JoinSqlExpression(Context, ex);
            _expressions.Add(join);
            return this;
        }

        public IDapperQueryable<T1, T2> LeftJoin(Expression<Func<T1, T2, bool>> expression)
        {
            var ex = Expression.Constant(new JoinArray(JoinType.Inner, expression));
            var join = new JoinSqlExpression(Context, ex);
            _expressions.Add(join);
            return this;
        }

        public IDapperQueryable<T1, T2> RightJoin(Expression<Func<T1, T2, bool>> expression)
        {
            var ex = Expression.Constant(new JoinArray(JoinType.Inner, expression));
            var join = new JoinSqlExpression(Context, ex);
            _expressions.Add(join);
            return this;
        }

        public int Count(int? commandTimeout = null)
        {
            var sql = BuildQuerySql("COUNT(*)");
            return _provider.ExecuteScalar<int>(sql, Context.Arguments, commandTimeout);
        }

        public Task<int> CountAsync(int? commandTimeout = null)
        {
            var sql = BuildQuerySql("COUNT(*)");
            return _provider.ExecuteScalarAsync<int>(sql, Context.Arguments, commandTimeout);
        }

        public IDapperQueryable<T1, T2> GroupBy<TGroup>(Expression<Func<T1, T2, TGroup>> expression)
        {
            _expressions.Add(new GroupSqlExpression(Context, expression));
            return this;
        }

        public IDapperQueryable<T1, T2> Having(Expression<Func<T1, T2, bool>> expression)
        {
            _expressions.Add(new HavingSqlExpression(Context, expression));
            return this;
        }

        public IDapperQueryable<T1, T2> OrderBy<TGroup>(Expression<Func<T1, T2, TGroup>> expression)
        {
            _expressions.Add(new OrderSqlExpression(Context, expression));
            return this;
        }

        public IDapperQueryable<T1, T2> OrderByDescending<TGroup>(Expression<Func<T1, T2, TGroup>> expression)
        {
            _expressions.Add(new OrderSqlExpression(Context, expression, false));
            return this;
        }


        public List<TResult> Select<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null)
        {
            var columns = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql(columns);
            return _provider.Query<TResult>(sql, Context.Arguments, false, commandTimeout).ToList();
        }

        public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null)
        {
            var columns = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql(columns);
            var list = await _provider.QueryAsync<TResult>(sql, Context.Arguments, commandTimeout);
            return list.ToList();
        }

        public IDapperQueryable<T1, T2> Skip(int count)
        {
            _skipCount = count;
            return this;
        }

        public TResult Sum<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null)
        {
            var column = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql($"SUM({column})");
            return _provider.ExecuteScalar<TResult>(sql, Context.Arguments, commandTimeout);
        }

        public Task<TResult> SumAsync<TResult>(Expression<Func<T1, T2, TResult>> expression, int? commandTimeout = null)
        {
            var column = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql($"SUM({column})");
            return _provider.ExecuteScalarAsync<TResult>(sql, Context.Arguments, commandTimeout);
        }

        public IDapperQueryable<T1, T2> Take(int count)
        {
            _takeCount = count;
            return this;
        }

        public IDapperQueryable<T1, T2> Where(Expression<Func<T1, T2, bool>> expression)
        {
            _expressions.Add(new WhereSqlExpression(Context, expression));
            return this;
        }

        private string BuildQuerySql(string columns)
        {
            var view = _expressions.BuildView();
            var where = _expressions.BuildWhere();
            var group = _expressions.BuildGroup();
            var having = _expressions.BuildHaving();
            var order = _expressions.BuildOrder();
            var sb = new StringBuilder();
            if (_takeCount >= 0 && _skipCount == 0)
            {
                sb.AppendFormat("SELECT TOP {0}\n\t{1}\nFROM\n\t{2}", _takeCount, columns, view);
            }
            else
            {
                sb.AppendFormat("SELECT\n\t{0}\nFROM\n\t{1}", columns, view);
            }
            if (where.Length > 0)
            {
                sb.AppendFormat("\nWHERE\n\t{0}", where);
            }
            if (group.Length > 0)
            {
                sb.AppendFormat("\nGROUP BY\n\t{0}", group);
            }
            if (having.Length > 0)
            {
                sb.AppendFormat("\nHAVING\n\t{0}", having);
            }
            if (order.Length > 0)
            {
                sb.AppendFormat("\nORDER BY\n\t{0}", order);
            }
            if (_skipCount > 0)
            {
                if (order.Length == 0)
                {
                    sb.AppendFormat("\nORDER BY\n\t{0}", "(SELECT 1)");
                }
                sb.AppendFormat(" OFFSET {0} ROWS", _skipCount);
                if (_takeCount > 0)
                {
                    sb.AppendFormat(" FETCH NEXT {0} ROWS ONLY", _takeCount);
                }
            }
            return sb.ToString();
        }


    }
}
