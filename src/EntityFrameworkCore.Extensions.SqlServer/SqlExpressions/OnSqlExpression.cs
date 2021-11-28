using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class OnSqlExpression : SqlExpression
    {
        public OnSqlExpression(SqlExpressionContext context, Expression expression)
            : base(context, expression)
        {
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value != null)
            {
                Append(node.Value.ToString() ?? string.Empty);
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                AppendMemberName(node);
            }
            return node;
        }
    }
}
