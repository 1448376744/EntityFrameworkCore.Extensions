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
            if (node.Value is JoinArray join)
            {
                var joinType = (JoinType)join.Joins[0];
                var expression = (Expression)join.Joins[1];
                Visit(expression);
            }
            return node;
        }
        //protected override Expression VisitMember(MemberExpression node)
        //{
        //    if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
        //    {
        //        var table1 = Context.TableAlias[node.Expression.Type];
        //        AppendMemberName(node);
        //    }
        //    return node;
        //}
    }
}
