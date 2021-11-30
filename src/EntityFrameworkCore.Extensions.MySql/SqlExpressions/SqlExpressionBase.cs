using System.Linq.Expressions;
using System.Text;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class SqlExpressionBase : ExpressionVisitor
    {
        protected bool IsBuild { get; private set; }
        protected SqlExpressionContext Context { get; }
        protected Expression? Expression { get; }
        protected StringBuilder ExpressionBuilder { get; }
        public SqlExpressionBase(SqlExpressionContext context, Expression? expression)
        {
            Context = context;
            Expression = expression;
            IsBuild = false;
            ExpressionBuilder = new StringBuilder();
        }
        public virtual string Build()
        {
            if (IsBuild)
            {
                return ExpressionBuilder.ToString();
            }
            if (Expression != null)
            {
                Visit(Expression);
            }
            IsBuild = true;
            return ExpressionBuilder.ToString();
        }
        protected void Append(char block)
        {
            ExpressionBuilder.Append(block);
        }
        protected void Append(string block)
        {
            ExpressionBuilder.Append(block);
        }
        protected void AppendFormat(string format, params object[] args)
        {
            ExpressionBuilder.AppendFormat(format, args);
        }
        protected void AppendLeftRight()
        {
            ExpressionBuilder.Append(')');
        }
        protected void AppendLeftInClude()
        {
            ExpressionBuilder.Append('(');
        }
        protected void AppendSpace()
        {
            ExpressionBuilder.Append(' ');
        }
        protected void AppendNot()
        {
            ExpressionBuilder.Append("NOT");
        }
        protected void AppendExpressionType(ExpressionType nodeType)
        {
            var type = ExpressionUtilities.ParseOperator(nodeType);
            ExpressionBuilder.Append(type);
        }
        protected void AppendIsNotNull()
        {
            ExpressionBuilder.Append("IS NOT NULL");
        }
        protected void AppendIsNull()
        {
            ExpressionBuilder.Append("IS NULL");
        }
        protected void AppendMemberName(MemberExpression node)
        {
            if (node.Expression != null)
            {
                var column = Context.GetAliasColumnName(node.Expression.Type, node.Member.Name);
                if (column != null)
                {
                    ExpressionBuilder.Append(column);
                }
            }
        }
        protected void AddArgument(Expression? node)
        {
            object? value = null;
            if (node != null)
            {
                value = ExpressionUtilities.ParseValue(node);
            }
            var name = $"@p_{Context.Arguments.Count}";
            ExpressionBuilder.Append(name);
            Context.Arguments.Add(name, value);
        }
        protected void AppendIn()
        {
            ExpressionBuilder.Append("IN");
        }
        protected void AppendLike()
        {
            ExpressionBuilder.Append("LIKE");
        }
    }
}
