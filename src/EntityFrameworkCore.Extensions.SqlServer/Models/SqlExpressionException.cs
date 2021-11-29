using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class SqlExpressionException : Exception
    {
        public Expression Expression { get; private set; }
      
        public SqlExpressionException(string message, Expression expression)
            : base(message)
        {
            Expression = expression;
        }
    }
}
