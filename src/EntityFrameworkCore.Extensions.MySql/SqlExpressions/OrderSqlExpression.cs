using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class OrderSqlExpression : SqlExpression
    {
        private readonly bool _asc;

        public OrderSqlExpression(SqlExpressionContext context, Expression expression, bool asc = true)
            : base(context, expression)
        {
            _asc = asc;
        }
       
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                AppendMemberName(node);
                if (_asc)
                {
                    Append(" ASC");
                }
                else
                {
                    Append(" DESC");
                }
            }
            return node;
        }
    }
}
