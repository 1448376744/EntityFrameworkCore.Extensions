using EntityFrameworkCore.Extensions.Metadata;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class FunctionSqlExpression : SqlExpression
    {
        public FunctionSqlExpression(SqlExpressionContext context, Expression expression)
          : base(context, expression)
        {

        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (IsLikeFunction(node))
            {
                VisitLikeFunction(node);
            }
            else if (IsInFunction(node))
            {
                VisitInFunction(node);
            }
            else if (IsSqlFunction(node))
            {
                VisitSqlFunction(node);
            }
            else
            {
                throw new SqlExpressionException($"Expression is undefined", node);
            }
            return node;
        }

        #region like

        private static bool IsLikeFunction(MethodCallExpression node)
        {
            if (node.Method.DeclaringType != typeof(string) && node.Arguments.Count != 1)
            {
                return false;
            }
            return node.Method.Name == nameof(string.Contains)
                || node.Method.Name == nameof(string.StartsWith)
                || node.Method.Name == nameof(string.EndsWith);
        }

        private void VisitLikeFunction(MethodCallExpression node)
        {
            Expression? expression1 = node.Object;
            Expression? expression2 = node.Arguments[0];
            if (expression2 is MemberExpression memberExpression && memberExpression.Expression?.NodeType == ExpressionType.Parameter)
            {
                var temp = expression1;
                expression1 = expression2;
                expression2 = temp;
            }
            AppendLeftInClude();
            Visit(expression1);
            AppendSpace();
            AppendLike();
            AppendSpace();
            var value = ExpressionUtilities.ParseValue(expression2);
            if (node.Method.Name == nameof(string.StartsWith))
            {
                value = $"{value}%";
            }
            else if (node.Method.Name == nameof(string.EndsWith))
            {
                value = $"%{value}";
            }
            else
            {
                value = $"%{value}%";
            }
            AddArgument(Expression.Constant(value));
            AppendLeftRight();
        }
        #endregion

        #region in
        private static bool IsInFunction(MethodCallExpression node)
        {
            if (node.Method.Name != nameof(Enumerable.Contains))
            {
                return false;
            }
            if (node.Method.DeclaringType == typeof(string))
            {
                return false;
            }

            if (node.Method.DeclaringType != null && node.Method.DeclaringType.IsArray && node.Method.Name == nameof(string.Contains))
            {
                return true;
            }
            if (typeof(Enumerable) == node.Method.DeclaringType && node.Method.Name == nameof(string.Contains))
            {
                return true;
            }
            return false;
        }

        private void VisitInFunction(MethodCallExpression node)
        {
            Expression expression1;
            Expression? expression2;
            if (node.Method.IsStatic)
            {
                expression1 = node.Arguments[1];
                expression2 = node.Arguments[0];
            }
            else
            {
                expression1 = node.Arguments[0];
                expression2 = node.Object;
            }
            AppendLeftInClude();
            Visit(expression1);
            AppendSpace();
            AppendIn();
            AppendSpace();
            AddArgument(expression2);
            AppendLeftRight();
        }
        #endregion

        #region custom
        private static bool IsSqlFunction(MethodCallExpression node)
        {
            return SqlFunctionAttribute.Has(node.Method);
        }

        private void VisitSqlFunction(MethodCallExpression node)
        {
            var method = SqlFunctionAttribute.GetName(node.Method, node.Method.Name.ToUpper());
            Append(method);
            AppendLeftInClude();
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                if (node.Arguments[i] is NewArrayExpression newArrayExpression)
                {
                    for (int j = 0; j < newArrayExpression.Expressions.Count; j++)
                    {
                        Visit(newArrayExpression.Expressions[j]);
                        if (j + 1 != newArrayExpression.Expressions.Count)
                        {
                            Append(',');
                        }
                    }
                }
                else
                {
                    Visit(node.Arguments[i]);
                }
                if (i + 1 != node.Arguments.Count)
                {
                    Append(',');
                }
            }
            AppendLeftRight();
        }
        #endregion
    }
}
