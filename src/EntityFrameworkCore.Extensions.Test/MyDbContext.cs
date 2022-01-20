using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore.Extensions.Test
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; internal set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
   
}
