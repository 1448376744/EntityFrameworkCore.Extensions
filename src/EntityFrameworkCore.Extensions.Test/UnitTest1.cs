using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace EntityFrameworkCore.Extensions.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = "Data Source=*****;Initial Catalog=****;Persist Security Info=True;User ID=sa;Password=****";
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .LogTo(s =>
                {
                    Debug.WriteLine(s);
                })
                .UseSqlServer(str)
                .Options;

            var context = new MyDbContext(options);
            
            var list = context.CreateSingleQuery(context.Students)
                .ToList();

            var list1 = context.CreateSingleQuery(context.Students)
                .Join(context.StudentClass, (a, b) => a.ClassId == b.Id)
                .Select((a, b) => new { a.Name, Class = b.Name });
        }
    }
}