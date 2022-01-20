using EntityFrameworkCore.Extensions.Metadata;
using EntityFrameworkCore.Extensions.SqlExpressions;
using System.Linq.Expressions;
using System.Text;

namespace EntityFrameworkCore.Extensions.Query
{
    public class DapperQueryableImpl<T> : IDapperQueryable<T>
    {
        internal  DapperQueryProvider _provider { get; }

        internal SqlExpressionContext Context { get; }

        private readonly SqlExpressionCollection _expressions = new SqlExpressionCollection();

        private int _takeCount = 0;

        private int _skipCount = 0;

        public DapperQueryableImpl(DapperQueryProvider provider, IModelEx model)
        {
            _provider = provider;
            Context = new SqlExpressionContext(model);
            Context.TableAlias.Add(typeof(T), "t");
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

        public TResult Sum<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null)
        {
            var column = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql($"SUM({column})");
            return _provider.ExecuteScalar<TResult>(sql, Context.Arguments, commandTimeout);
        }

        public Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null)
        {
            var column = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql($"SUM({column})");
            return _provider.ExecuteScalarAsync<TResult>(sql, Context.Arguments, commandTimeout);
        }

        public List<T> Select(int? commandTimeout = null)
        {
            var columns = new SelectSqlExpression(Context).Build(typeof(T));
            var sql = BuildQuerySql(columns);
            return _provider.Query<T>(sql, Context.Arguments, false, commandTimeout).ToList();
        }

        public List<TResult> Select<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null)
        {
            var columns = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql(columns);
            return _provider.Query<TResult>(sql, Context.Arguments, false, commandTimeout).ToList();
        }

        public async Task<List<T>> SelectAsync(int? commandTimeout = null)
        {
            var columns = new SelectSqlExpression(Context).Build(typeof(T));
            var sql = BuildQuerySql(columns);
            var list = await _provider.QueryAsync<T>(sql, Context.Arguments, commandTimeout);
            return list.ToList();
        }

        public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> expression, int? commandTimeout = null)
        {
            var columns = new SelectSqlExpression(Context, expression).Build();
            var sql = BuildQuerySql(columns);
            var list = await _provider.QueryAsync<TResult>(sql, Context.Arguments, commandTimeout);
            return list.ToList();
        }
        public IDapperQueryable<T> GroupBy<TGroup>(Expression<Func<T, TGroup>> expression)
        {
            _expressions.Add(new GroupSqlExpression(Context, expression));
            return this;
        }

        public IDapperQueryable<T> Having(Expression<Func<T, bool>> expression)
        {
            _expressions.Add(new HavingSqlExpression(Context, expression));
            return this;
        }

        public IDapperQueryable<T> OrderBy<TGroup>(Expression<Func<T, TGroup>> expression)
        {
            _expressions.Add(new OrderSqlExpression(Context, expression));
            return this;
        }

        public IDapperQueryable<T> OrderByDescending<TGroup>(Expression<Func<T, TGroup>> expression)
        {
            _expressions.Add(new OrderSqlExpression(Context, expression, false));
            return this;
        }

        public IDapperQueryable<T> Skip(int count)
        {
            _skipCount = count;
            return this;
        }

        public IDapperQueryable<T> Take(int count)
        {
            _takeCount = count;
            return this;
        }

        public IDapperQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            _expressions.Add(new WhereSqlExpression(Context, expression));
            return this;
        }

        private string BuildQuerySql(string columns)
        {
            var view = Context.GetAliasTableName(typeof(T));
            var where = _expressions.BuildWhere();
            var group = _expressions.BuildGroup();
            var having = _expressions.BuildHaving();
            var order = _expressions.BuildOrder();
            var sb = new StringBuilder();
            if (_takeCount > 0 && _skipCount == 0)
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
