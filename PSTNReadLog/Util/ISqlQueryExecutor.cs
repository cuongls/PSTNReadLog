using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSTNReadLog.Util
{
    public interface ISqlQueryExecutor
    {
        DataTable Execute(string Sql);
        void ExecuteUpdate(string Sql);
    }
}
