namespace EntityFrameworkCore.Extensions.Metadata
{
    public interface IModelEx
    {
        string GetTableName(Type type);
        string? GetColumnName(Type type, string name);
    }
}
