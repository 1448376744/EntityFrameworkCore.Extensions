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
    }
}
