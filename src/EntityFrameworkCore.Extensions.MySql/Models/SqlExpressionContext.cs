using EntityFrameworkCore.Extensions.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Extensions.SqlExpressions
{
    public class SqlExpressionContext
    {
        public IModelEx Model { get; }

        public Dictionary<Type, string> TableAlias { get; set; } = new Dictionary<Type, string>();

        public Dictionary<string, object?> Arguments { get; } = new Dictionary<string, object?>();

        public SqlExpressionContext(IModelEx model)
        {
            Model = model;
        }

        public string GetAliasTableName(Type type)
        {
            var name = Model.GetTableName(type);
            return $"`{name}` AS `{TableAlias[type]}`";
        }

        public string GetAliasColumnName(Type type, string name)
        {
            var column = Model.GetColumnName(type, name);
            return $"`{TableAlias[type]}`.`{column}`";
        }
    }
}
