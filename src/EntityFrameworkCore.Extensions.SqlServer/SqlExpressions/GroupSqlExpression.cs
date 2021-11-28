using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class GroupSqlExpression : SqlExpression
    {
        public GroupSqlExpression(SqlExpressionContext context, Expression expression)
            : base(context, expression)
        {

        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Members != null)
            {
                for (int i = 0; i < node.Members.Count; i++)
                {
                    Visit(node.Arguments[i]);
                    if (i != node.Members.Count - 1)
                    {
                        Append(',');
                    }
                }
            }
            return node;
        }
    }
}
