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
            var logggerFactory = LoggerFactory.Create(c => 
            {
                c.SetMinimumLevel(LogLevel.Debug);
                c.AddDebug();
            });
            var mysqlStr = "Data Source=localhost;Database=test;User Id=root;Password=1024";
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseLoggerFactory(logggerFactory)
                .UseMySql(mysqlStr,ServerVersion.AutoDetect(mysqlStr))
                //.UseMySql(mysqlStr,ServerVersion.AutoDetect(mysqlStr))
                .Options;

            var context = new MyDbContext(options);
            var students = context.Queryable<Student>()
                .Where(a => a.Id == 1)
                .First();


        }
    }
}