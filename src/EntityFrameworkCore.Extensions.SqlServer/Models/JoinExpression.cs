using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class JoinExpression
    {
        public Type Table1 { get; }
        public Type Table2 { get; }
        public JoinType JoinType { get; }
        public Expression Expression { get; }
        public JoinExpression(Type table1, Type table2, Expression expression, JoinType join)
        {
            Table1 = table1;
            Table2 = table2;
            Expression = expression;
            JoinType = join;
        }
    }
}
