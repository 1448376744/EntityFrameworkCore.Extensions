using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.Extensions.Metadata
{
    public class EFCoreModel : IModelEx
    {
        private readonly IModel _model;

        private const string _format = "[{0}]";

        public EFCoreModel(IModel model)
        {
            _model = model;
        }

        public string GetTableName(Type type)
        {
            var entityType = _model.FindEntityType(type);
            if (entityType != null)
            {
                var table = entityType.FindAnnotation(RelationalAnnotationNames.TableName);
                if (table != null && table.Value != null)
                {
                    return table.Value.ToString() ?? throw new ArgumentNullException(); ;
                }
            }
            return type.Name;
        }

        public string? GetColumnName(Type type, string name)
        {
            var entityType = _model.FindEntityType(type);
            if (entityType != null)
            {
                var property = entityType.FindProperty(name);
                if (property == null)
                    return null;
                var column = property.FindAnnotation(RelationalAnnotationNames.ColumnName);
                if (column != null && column.Value != null)
                {
                    return column.Value.ToString();
                }
            }
            return type.GetProperty(name)?.Name;
        }
    }
}
