using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceExtend.Util
{
    class SqlServerSqlQueryExecutor : SqlQueryExecutorBase, ISqlQueryExecutor
    {
        public SqlServerSqlQueryExecutor(string IP, int Port, string DataBase, string UserName, string PassWord) : base(IP, Port, DataBase, UserName, PassWord)
        {
        }
        public DataTable Execute(string Sql)
        {
            BuildConnectionString();
            DataTable dt = new DataTable();
            using (SqlConnection c = new SqlConnection(this.ConnectionString))
            {
                c.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(Sql, c))
                {
                    a.Fill(dt);
                }
                c.Close();
            }
            return dt;
        }
        protected override void BuildConnectionString()
        {
            this.ConnectionString = string.Format("Server = {0}; Database = {1}; User Id = {2};Password = {3};", this.IP, this.DataBase, this.UserName, this.PassWord);
        }
        public void ExecuteUpdate(string Sql)
        {
            BuildConnectionString();
            DataTable dt = new DataTable();
            using (SqlConnection c = new SqlConnection(this.ConnectionString))
            {
                c.Open();
                using (SqlCommand a = new SqlCommand(Sql, c))
                {
                    a.ExecuteNonQuery();
                }
                c.Close();
            }
        }
    }
}
