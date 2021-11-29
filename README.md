# EntityFrameworkCore.Extensions

## 说明
  该框架共用EFCore的模型约定，内置高性能的表达式树解析方案，将解析好的sql交给dapper查询。进而获得超高的开发效率和性能
  
## 类型推断

``` C#
  //方式一
  var list1 = context.Queryable<Student, StudentClass>()     
       .ToList();
  //方式二
  var list2 = context.Queryable(context.Students)     
       .ToList();
```
## 多表链接

``` C#
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
```
## 函数支持
 ``` C#
 public class MyFunc
 {
      [SqlFunction]
      public static long Count<T>(T column)
      {
          throw Excption();
      }
 }
 
 var list = context.Queryable<Student>()
         .GroupBy(a=>a.Name)
         .Select(s=> new
         {
             s.Name,
             Count = MyFunc.Count(s.Id)
         });
 ```
 
