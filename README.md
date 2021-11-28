# EntityFrameworkCore.Extensions

## 说明
  该框架共用EFCore的模型约定，内置高性能的表达式树解析方案，将解析好的sql交给dapper查询。进而获得超高的开发效率和性能
  
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
 
 var list = context.CreateSingle<Student>()
         .GroupBy(a=>a.Name)
         .Select(s=> new
         {
             s.Name,
             Count = MyFunc.Count(s.Id)
         });
 ```
 
