using System.Linq.Expressions;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class SelectSqlExpression : SqlExpression
    {
        public SelectSqlExpression(SqlExpressionContext context)
         : base(context)
        {
        }

        public SelectSqlExpression(SqlExpressionContext context, Expression expression)
            : base(context, expression)
        {
        }

        public string Build(Type type)
        {
            var properties = type.GetProperties();
            List<string> columns = new List<string>();
            foreach (var item in properties)
            {
                var column = Context.GetAliasColumnName(type, item.Name);
                if (column == null)
                    continue;
                if (column == item.Name)
                {
                    columns.Add(column);
                }
                else
                {
                    columns.Add($"{column} AS `{item.Name}`");
                }
            }
            var select = string.Join(",\n\t", columns);
            return select;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Members != null)
            {
                for (int i = 0; i < node.Members.Count; i++)
                {
                    if (i != 0)
                    {
                        Append("\n\t");
                    }
                    Visit(node.Arguments[i]);
                    AppendFormat(" AS {0}", node.Members[i].Name);
                    if (i != node.Members.Count - 1)
                    {
                        Append(',');
                    }
                }
            }
            return node;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            for (int i = 0; i < node.Bindings.Count; i++)
            {
                if (node.Bindings[i] is MemberAssignment memberBinding)
                {
                    Visit(memberBinding.Expression);
                    AppendFormat(" AS {0}", memberBinding.Member.Name);
                    if (i != node.Bindings.Count - 1)
                    {
                        Append(',');
                    }
                }
            }
            return node;
        }
    }
}
