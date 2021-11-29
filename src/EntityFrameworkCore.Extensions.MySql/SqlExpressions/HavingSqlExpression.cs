using EntityFrameworkCore.Extensions.Metadata;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class HavingSqlExpression : SqlExpression
    {
        public HavingSqlExpression(SqlExpressionContext context, Expression expression)
            : base(context, expression)
        {

        }
    }
}
