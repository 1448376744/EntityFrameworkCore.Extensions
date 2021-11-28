using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class WhereSqlExpression : SqlExpression
    {
        public WhereSqlExpression(SqlExpressionContext context, Expression expression)
            : base(context, expression)
        {

        }
    }
}
