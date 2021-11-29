using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public class JoinArray
    {
        public object[] Joins { get; }
       
        public JoinArray(params object[] joins)
        {
            Joins = joins;
        }

        public static ConstructorInfo GetConstructor()
        {
            return typeof(JoinArray).GetConstructors()[0];
        }
    }
}
