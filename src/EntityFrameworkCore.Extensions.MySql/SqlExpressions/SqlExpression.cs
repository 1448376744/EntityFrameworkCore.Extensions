using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public abstract class SqlExpression : SqlExpressionBase
    {
        #region base
        public SqlExpression(SqlExpressionContext context)
            :base(context,null)
        {
        }

        public SqlExpression(SqlExpressionContext context, Expression expression)
            : base(context, expression)
        {
        }
        #endregion

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
#pragma warning disable CS8603 // 可能返回 null 引用。
            return node;
#pragma warning restore CS8603 // 可能返回 null 引用。
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

    }
}
