using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSTNReadLog.Util
{
    public abstract class SqlQueryExecutorBase
    {
        public string IP { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Port { get; set; }
        public string DataBase { get; set; }
        protected string ConnectionString { get; set; }
        protected abstract void BuildConnectionString();

        public SqlQueryExecutorBase(string IP, int Port, string DataBase, string UserName, string PassWord)
        {
            this.IP = IP;
            this.Port = Port;
            this.DataBase = DataBase;
            this.UserName = UserName;
            this.PassWord = PassWord;
        }

    }

}
