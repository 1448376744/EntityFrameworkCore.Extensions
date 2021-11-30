using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class JoinSqlExpression : SqlExpressionBase
    {
        private readonly SqlExpressionContext _context;

        private readonly List<Type> _types = new();

        private readonly Queue<JoinType> _joinTypes = new();

        private readonly Queue<Expression> _expressions = new();

        public JoinSqlExpression(SqlExpressionContext context, Expression expression)
            : base(context, expression)
        {
            _context = context;
        }

        public override string Build()
        {
            if (!IsBuild)
            {
                base.Build();
                var type1 = _types.First();
                var table1 = _context.GetAliasTableName(type1);
                ExpressionBuilder.AppendFormat("{0}", table1);
                foreach (var item in _types.Skip(1))
                {
                    if (_joinTypes.Count > 0)
                    {
                        var joinType = GetJoinType(_joinTypes.Dequeue());
                        ExpressionBuilder.AppendFormat(" {0} ", joinType);
                    }
                    var table2 = _context.GetAliasTableName(item);
                    ExpressionBuilder.AppendFormat("{0}", table2);
                    if (_expressions.Count > 0)
                    {
                        var ex = _expressions.Dequeue();
                        var on = new OnSqlExpression(_context, ex).Build();
                        ExpressionBuilder.AppendFormat(" ON {0}", on);
                    }
                }
            }
            return base.Build();
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            foreach (var item in node.Parameters)
            {
                _types.Add(item.Type);
            }
            Visit(node.Body);
            return node;
        }
        protected override Expression VisitNew(NewExpression node)
        {
            foreach (var item in node.Arguments)
            {
                Visit(item);
            }
            return node;
        }
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            foreach (var item in node.Expressions)
            {
                Visit(item);
            }
            return node;
        }
        protected override Expression VisitUnary(UnaryExpression node)
        {
            Visit(node.Operand);
            return node;
        }
        protected override Expression VisitBinary(BinaryExpression node)
        {
            _expressions.Enqueue(node);
            return node;
        }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value is JoinType joinType)
            {
                _joinTypes.Enqueue(joinType);
            }
            else if (node.Value is JoinArray joinArray)
            {
                _joinTypes.Enqueue((JoinType)joinArray.Joins[0]);
                var lambda = (LambdaExpression)joinArray.Joins[1];
                foreach (var item in lambda.Parameters)
                {
                    _types.Add(item.Type);
                }
                _expressions.Enqueue(lambda.Body);
            }
            return node;
        }

        private static string GetJoinType(JoinType joinType)
        {
            string type = joinType switch
            {
                JoinType.Left => "LEFT JOIN",
                JoinType.Right => "RIGHT JOIN",
                _ => "INNER JOIN",
            };
            return type;
        }
    }
}
