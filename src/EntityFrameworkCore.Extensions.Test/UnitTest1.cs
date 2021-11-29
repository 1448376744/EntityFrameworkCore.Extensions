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
            var SqlStr = System.IO.File.ReadAllText(".\\SqlConnectionString.log");
            var mysqlStr = System.IO.File.ReadAllText(".\\MySqlConnectionString.log");
            var logggerFactory = new LoggerFactory();
            logggerFactory.AddProvider(new DebugLoggerProvider());
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseLoggerFactory(logggerFactory)
                //.UseSqlServer(SqlStr)
                .UseMySql(mysqlStr,ServerVersion.AutoDetect(mysqlStr))
                .Options;

            var context = new MyDbContext(options);
            var list1 = context.Queryable<Student, StudentClass>()
               .On((a, b) => new JoinArray
               (
                   JoinType.Left, a.ClassId == b.Id
               ))
               .Skip(1)
               .Take(1)
               .Select((a, b) => new { a.Name, Class = b.Name });

            var list2 = context.Queryable<Student, StudentClass, StudentGrade>()
                .On((a, b, c) => new JoinArray
                (
                    JoinType.Inner, a.ClassId == b.Id,
                    JoinType.Left, a.GradeId == c.Id
                ))
                .Skip(1)
                .Take(1)
                .Select((a, b, c) => new { a.Name, Class = b.Name, c.Grade });
        }
    }
}