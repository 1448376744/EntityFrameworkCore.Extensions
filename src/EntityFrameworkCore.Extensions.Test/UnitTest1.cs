using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFrameworkCore.Extensions.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = System.IO.File.ReadAllText(".\\connectionString.log");
            var logggerFactory = new LoggerFactory();
            logggerFactory.AddProvider(new DebugLoggerProvider());
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseLoggerFactory(logggerFactory)
                .UseSqlServer(str)
                .Options;

            var context = new MyDbContext(options);

            var list = context.Queryable(context.Students)
                .Where(a => a.Id > 0)
                .GroupBy(a => a.ClassId)
                .Select(s => new
                {
                    s.ClassId,
                    Count = SqlFunc.COUNT(s.Id)
                });

            var list1 = context.Queryable(context.Students, context.StudentClass)
                .LeftJoin((a, b) => a.ClassId == b.Id)
                .Select((a, b) => new { a.Name, Class = b.Name });
        }
    }
}