using EntityFrameworkCore.Extensions.Metadata;
using System.Linq.Expressions;
using System.Text;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public abstract class SqlExpression : ExpressionVisitor, ISqlExpression
    {
        private readonly Expression? _expression;

        protected SqlExpressionContext Context { get; }
        
        private bool _isBuild = false;

        private readonly StringBuilder _sql = new();

        public SqlExpression(SqlExpressionContext context)
        {
            Context = context;
        }

        public SqlExpression(SqlExpressionContext context, Expression expression)
        {
            Context = context;
            _expression = expression;
        }

        public virtual string Build()
        {
            if (_isBuild)
            {
                return _sql.ToString();
            }
            if (_expression != null)
            {
                Visit(_expression);
            }
            _isBuild = true;
            return _sql.ToString();
        }

        #region visit
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                AppendMemberName(node);
            }
            else
            {
                AddArgument(node);
            }
            return node;
        }
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var expression = new FunctionSqlExpression(Context, node).Build();
            Append(expression);
            return node;
        }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node == null)
            {
                AppendSpace();
            }
            AddArgument(node);
            return node;
        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Right is ConstantExpression constantExpression && constantExpression.Value == null)
            {
                AppendLeftInClude();
                Visit(node.Left);
                AppendSpace();
                if (node.NodeType == ExpressionType.Equal)
                {
                    AppendIsNull();
                }
                else
                {
                    AppendIsNotNull();
                }
                AppendLeftRight();
            }
            else
            {
                AppendLeftInClude();
                Visit(node.Left);
                AppendSpace();
                AppendExpressionType(node.NodeType);
                AppendSpace();
                Visit(node.Right);
                AppendLeftRight();
            }
            return node;
        }
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                AppendNot();
                AppendSpace();
                AppendLeftInClude();
                Visit(node.Operand);
                AppendLeftRight();
            }
            else
            {
                Visit(node.Operand);
            }
            return node;
        }
        #endregion

        #region append
        protected void Append(char block)
        {
            _sql.Append(block);
        }
        protected void Append(string block)
        {
            _sql.Append(block);
        }
        protected void AppendFormat(string format, params object[] args)
        {
            _sql.AppendFormat(format, args);
        }
        protected void AppendLeftRight()
        {
            _sql.Append(')');
        }
        protected void AppendLeftInClude()
        {
            _sql.Append('(');
        }
        protected void AppendSpace()
        {
            _sql.Append(' ');
        }
        protected void AppendNot()
        {
            _sql.Append("NOT");
        }
        protected void AppendExpressionType(ExpressionType nodeType)
        {
            var type = ExpressionUtilities.ParseOperator(nodeType);
            _sql.Append(type);
        }
        protected void AppendIsNotNull()
        {
            _sql.Append("IS NOT NULL");
        }
        protected void AppendIsNull()
        {
            _sql.Append("IS NULL");
        }
        protected void AppendMemberName(MemberExpression node)
        {
            if (node.Expression != null)
            {
                var column = Context.GetAliasColumnName(node.Expression.Type, node.Member.Name);
                _sql.Append(column);
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
            _sql.Append(name);
            Context.Arguments.Add(name, value);
        }
        protected void AppendIn()
        {
            _sql.Append("IN");
        }
        protected void AppendLike()
        {
            _sql.Append("LIKE");
        }
        #endregion
    }
}
