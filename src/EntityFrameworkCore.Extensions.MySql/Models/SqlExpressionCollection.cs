using System.Collections;
using System.Text;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class SqlExpressionCollection : ICollection<ISqlExpression>
    {
        private readonly List<ISqlExpression> _list = new List<ISqlExpression>();

        public int Count => ((ICollection<SqlExpression>)_list).Count;

        public bool IsReadOnly => ((ICollection<SqlExpression>)_list).IsReadOnly;

        public void Add(ISqlExpression item)
        {
            ((ICollection<ISqlExpression>)_list).Add(item);
        }

        public void Clear()
        {
            ((ICollection<ISqlExpression>)_list).Clear();
        }

        public bool Contains(ISqlExpression item)
        {
            return ((ICollection<ISqlExpression>)_list).Contains(item);
        }

        public void CopyTo(ISqlExpression[] array, int arrayIndex)
        {
            ((ICollection<ISqlExpression>)_list).CopyTo(array, arrayIndex);
        }

        public IEnumerator<ISqlExpression> GetEnumerator()
        {
            return ((IEnumerable<ISqlExpression>)_list).GetEnumerator();
        }

        public bool Remove(ISqlExpression item)
        {
            return ((ICollection<ISqlExpression>)_list).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        public string BuildWhere()
        {
            var sb = new StringBuilder();
            var expressions = _list.Where(a => a is WhereSqlExpression);
            foreach (var item in expressions)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                var expression = item.Build();
                sb.Append(expression);
            }
            return sb.ToString();
        }

        public string BuildGroup()
        {
            var sb = new StringBuilder();
            var expressions = _list.Where(a => a is GroupSqlExpression);
            foreach (var item in expressions)
            {
                if (sb.Length > 0)
                {
                    sb.Append(',');
                }
                var expression = item.Build();
                sb.Append(expression);
            }
            return sb.ToString();
        }

        public string BuildOrder()
        {
            var sb = new StringBuilder();
            var expressions = _list.Where(a => a is OrderSqlExpression);
            foreach (var item in expressions)
            {
                if (sb.Length > 0)
                {
                    sb.Append(',');
                }
                var expression = item.Build();
                sb.Append(expression);
            }
            return sb.ToString();
        }

        public string BuildHaving()
        {
            var sb = new StringBuilder();
            var expressions = _list.Where(a => a is HavingSqlExpression);
            foreach (var item in expressions)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                var expression = item.Build();
                sb.Append(expression);
            }
            return sb.ToString();
        }

        public string BuildView()
        {
            var expression = _list
                .Where(a => a is JoinSqlExpression)
                .FirstOrDefault();
            if (expression == null)
            {
                throw new InvalidOperationException("Missing join expression");
            }
            return expression.Build();
        }
    }
}
