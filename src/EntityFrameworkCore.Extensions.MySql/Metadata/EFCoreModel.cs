using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.Extensions.Metadata
{
    public class EFCoreModel : IModelEx
    {
        private readonly IModel _model;

        public EFCoreModel(IModel model)
        {
            _model = model;
        }

        public string GetTableName(Type type)
        {
            var entityType = _model.FindEntityType(type);
            if (entityType != null)
            {
                var table = entityType
                    .FindAnnotation(RelationalAnnotationNames.TableName);
                if (table != null )
                {
                    var tableName = table.Value;
                    if (tableName!=null)
                    {
                        return $"{tableName}".ToLower();
                    }
                }
            }
            return type.Name.ToLower();
        }

        public string? GetColumnName(Type type, string name)
        {
            var entityType = _model.FindEntityType(type);
            if (entityType != null)
            {
                var property = entityType.FindProperty(name);
                if (property == null)
                    return null;
                var column = property
                    .FindAnnotation(RelationalAnnotationNames.ColumnName);
                if (column != null && column.Value != null)
                {
                    return column.Value.ToString();
                }
            }
            return type.GetProperty(name)?.Name;
        }
    }
}
