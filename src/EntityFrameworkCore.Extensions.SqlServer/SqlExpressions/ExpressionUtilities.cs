using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public static class ExpressionUtilities
    {
        public static object? ParseValue(Expression? expression)
        {
            object? value = null;
            if (expression == null)
                return null;
            if (expression is ConstantExpression constant)
                value = constant.Value;
            else if (expression is MemberExpression)
            {
                var mxs = new Stack<MemberExpression>();
                var temp = expression;
                while (temp is MemberExpression memberExpression)
                {
                    mxs.Push(memberExpression);
                    temp = memberExpression.Expression;
                }
                foreach (var item in mxs)
                {
                    if (item.Expression is ConstantExpression cex)
                        value = cex.Value;
                    if (item.Member is PropertyInfo pif)
                        value = pif.GetValue(value);
                    else if (item.Member is FieldInfo fif)
                        value = fif.GetValue(value);
                }
            }
            else
            {
                value = Expression.Lambda(expression).Compile().DynamicInvoke();
            }
            if (value == null)
            {
                throw new SqlExpressionException($"The result of the expression cannot be null", expression);
            }
            return value;
        }

        public static string ParseOperator(ExpressionType expressionType)
        {
            var condition = string.Empty;
            switch (expressionType)
            {
                case ExpressionType.Add:
                    condition = "+";
                    break;
                case ExpressionType.Subtract:
                    condition = "-";
                    break;
                case ExpressionType.Multiply:
                    condition = "*";
                    break;
                case ExpressionType.Divide:
                    condition = "/";
                    break;
                case ExpressionType.Modulo:
                    condition = "%";
                    break;
                case ExpressionType.Equal:
                    condition = "=";
                    break;
                case ExpressionType.NotEqual:
                    condition = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    condition = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    condition = ">=";
                    break;
                case ExpressionType.LessThan:
                    condition = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    condition = "<=";
                    break;
                case ExpressionType.OrElse:
                    condition = "OR";
                    break;
                case ExpressionType.AndAlso:
                    condition = "AND";
                    break;
                case ExpressionType.Not:
                    condition = "NOT";
                    break;
                case ExpressionType.Or:
                    condition = "|";
                    break;
                case ExpressionType.And:
                    condition = "&";
                    break;
                case ExpressionType.ExclusiveOr:
                    condition = "^";
                    break;
                case ExpressionType.LeftShift:
                    condition = "<<";
                    break;
                case ExpressionType.RightShift:
                    condition = ">>";
                    break;
            }
            return condition;
        }
    }
}
