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
            var logggerFactory = LoggerFactory.Create(c => 
            {
                c.SetMinimumLevel(LogLevel.Debug);
                c.AddDebug();
            });
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseLoggerFactory(logggerFactory)
                .UseSqlServer(SqlStr)
                //.UseMySql(mysqlStr,ServerVersion.AutoDetect(mysqlStr))
                .Options;

            var context = new MyDbContext(options);
        
            var query = context.Queryable<Student, StudentClass>()
              .On((a, b) => new JoinArray
              (
                  JoinType.Left, a.ClassId == b.Id
              ));
            
            var count = query.Count();
        
            var list1 = query
               .Skip(1)
               .Take(1)
               .OrderBy((a,b)=>a.Id)
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