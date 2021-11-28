using System.Reflection;

namespace EntityFrameworkCore.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SqlFunctionAttribute : Attribute
    {
        public string Name { get; private set; }
        public SqlFunctionAttribute(string name)
        {
            Name = name;
        }
        public static bool Has(MethodInfo method)
        {
            return method.GetCustomAttributes<SqlFunctionAttribute>().Any();
        }
        public static string GetName(MethodInfo method, string name)
        {
            return method.GetCustomAttribute<SqlFunctionAttribute>()?.Name ?? name;
        }
    }
}
