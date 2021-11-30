using EntityFrameworkCore.Extensions.Metadata;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class SqlExpressionContext
    {
        public IModelEx Model { get; }

        public Dictionary<Type, string> TableAlias { get; } = new Dictionary<Type, string>();

        public Dictionary<string, object?> Arguments { get; } = new Dictionary<string, object?>();

        public SqlExpressionContext(IModelEx model)
        {
            Model = model;
        }

        public string GetAliasTableName(Type type)
        {
            var name = Model.GetTableName(type);
            return $"{NameFormat(name)} AS {NameFormat(TableAlias[type])}";
        }

        public string? GetAliasColumnName(Type type, string name)
        {
            var column = Model.GetColumnName(type, name);
            if (column == null)
            {
                return null;
            }
            return $"{NameFormat(TableAlias[type])}.{NameFormat(column)}";
        }

        public string NameFormat(string name)
        {
            return $"`{name}`";
        }
    }
}
