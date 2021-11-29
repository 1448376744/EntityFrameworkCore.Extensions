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

        public DbSet<StudentClass> StudentClass { get; internal set; }
    }

    public class Student
    {
        public int Id { get; set; }
        [Column("SName")]
        public string Name { get; set; }
        public int ClassId { get; set; }
        public int GradeId { get; set; }
    }
    public class StudentGrade
    {
        public int Id { get; set; }
        public string Grade { get; set; }
    }
    public class StudentClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
