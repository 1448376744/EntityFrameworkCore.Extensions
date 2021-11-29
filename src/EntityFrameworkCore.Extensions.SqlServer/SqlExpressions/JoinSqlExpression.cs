using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class JoinSqlExpression : ISqlExpression
    {
        private readonly SqlExpressionContext _context;

        private readonly List<Type> _types = new();

        private readonly Queue<JoinType> _joinTypes = new();

        private readonly Queue<Expression> _expressions = new();

        private readonly Expression _expression;

        public JoinSqlExpression(SqlExpressionContext context, Expression expression)
        {
            _expression = expression;
            _context = context;
        }

     
        public string Build()
        {
            Visit();
            var sb = new StringBuilder();
            var type1 = _types.First();
            var table1 = _context.GetAliasTableName(type1);
            sb.AppendFormat("{0} ", table1);
            foreach (var item in _types.Skip(1))
            {
                if (_joinTypes.Count > 0)
                {
                    var joinType = GetJoinType(_joinTypes.Dequeue());
                    sb.AppendFormat(" {0} ", joinType);
                }
                var table2 = _context.GetAliasTableName(item);
                sb.AppendFormat("{0}", table2);
                if (_expressions.Count > 0)
                {
                    var ex = _expressions.Dequeue();
                    var on = new OnSqlExpression(_context, ex).Build();
                    sb.AppendFormat(" ON {0}", on);
                }
            }
            return sb.ToString();
        }

        private void VisitConstant(ConstantExpression node)
        {
            if (node.Value is JoinArray joinArray)
            {
                _joinTypes.Enqueue((JoinType)joinArray.Joins[0]);
                var lambda = (LambdaExpression)joinArray.Joins[1];
                foreach (var item in lambda.Parameters)
                {
                    _types.Add(item.Type);
                }
                _expressions.Enqueue(lambda.Body);
            }
        }

        private void VisitLambda(LambdaExpression lambda)
        {
            foreach (var item in lambda.Parameters)
            {
                _types.Add(item.Type);
            }
            if (lambda.Body is NewExpression newExpression)
            {
                if (newExpression.Arguments[0] is NewArrayExpression newArrayExpression)
                {
                    foreach (Expression item in newArrayExpression.Expressions)
                    {
                        if (item is UnaryExpression unaryExpression)
                        {
                            if (unaryExpression.Operand is ConstantExpression constant)
                            {
                                if (constant.Value is JoinType joinType)
                                {
                                    _joinTypes.Enqueue(joinType);
                                }
                            }
                            else
                            {
                                _expressions.Enqueue(item);
                            }
                        }
                    }
                }
            }
        }

        private void Visit()
        {
            if (_expression is ConstantExpression constant)
            {
                VisitConstant(constant);
            }
            else if (_expression is LambdaExpression lambda)
            {
                VisitLambda(lambda);
            }
            else
            {
                throw new SqlExpressionException("Unsupported join expression", _expression);
            }
        }

        private string GetJoinType(JoinType joinType)
        {
            string type;
            switch (joinType)
            {
                case JoinType.Left:
                    type = "LEFT JOIN";
                    break;
                case JoinType.Right:
                    type = "RIGHT JOIN";
                    break;
                default:
                    type = "JOIN";
                    break;
            }
            return type;
        }
    }
}
