using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUpdater.Class
{
    public class Connection
    {
        public SqlConnection ConnectToDataBase()
        {
            return new SqlConnection("Server=ibm-sql-cl2\\test;Database=general_minus_1;Trusted_Connection=True;");
        }
        public SqlDataReader Reader(SqlConnection connection, string query)
        {
            SqlCommand Command;
            string commandText;
            commandText = query;
            Command = new SqlCommand(commandText, connection);
            return Command.ExecuteReader();

        }
    }
}
