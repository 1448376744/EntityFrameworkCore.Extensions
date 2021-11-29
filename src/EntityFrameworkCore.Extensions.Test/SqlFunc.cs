using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Extensions.Test
{
    public static class SqlFunc
    {
        [SqlFunction]
        public static int COUNT<T>(T column)
        {
            throw new NotImplementedException();
        }
    }
}
