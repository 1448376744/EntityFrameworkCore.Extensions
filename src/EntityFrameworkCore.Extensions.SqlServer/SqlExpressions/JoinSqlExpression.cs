using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class JoinSqlExpression : ISqlExpression
    {
        private readonly SqlExpressionContext _context;

        private readonly JoinExpression[] _expressions;

        public JoinSqlExpression(SqlExpressionContext context, params JoinExpression[] expression)
        {
            _context = context;
            _expressions = expression;
        }

        public string Build()
        {
            var sb = new StringBuilder();
            foreach (var item in _expressions)
            {
                var on = new OnSqlExpression(_context, item.Expression).Build();
                var table1 = _context.GetAliasTableName(item.Table1);
                var table2 = _context.GetAliasTableName(item.Table2);
                sb.Append(table1);
                switch (item.JoinType)
                {
                    case JoinType.Inner:
                        sb.Append(" JOIN ");
                        break;
                    case JoinType.Left:
                        sb.Append(" LEFT JOIN ");
                        break;
                    case JoinType.Right:
                        sb.Append(" RIGHT JOIN ");
                        break;
                }
                sb.Append(table2);
                sb.AppendFormat(" ON {0}", on);
            }
            return sb.ToString();
        }
    }
}
