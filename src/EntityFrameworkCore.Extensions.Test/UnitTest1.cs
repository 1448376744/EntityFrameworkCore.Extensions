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


            var list1 = context.Queryable<Student, StudentClass, StudentGrade>()
                .On((a, b, c) => new JoinArray
                (
                    JoinType.Inner, a.ClassId == b.Id,
                    JoinType.Left, a.GradeId == c.Id
                ))
                .Select((a, b, c) => new { a.Name, Class = b.Name });
        }
    }
}