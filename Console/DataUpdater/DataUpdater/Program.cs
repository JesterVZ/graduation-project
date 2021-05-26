using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataUpdater.Class;
using DataUpdater.Model;

namespace DataUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Connection Connection = new Connection();
            using SqlConnection sqlConnection = Connection.ConnectToDataBase();
            Update update = new Update();
            List<Result> results = new List<Result>();
            update.UpdateDate(sqlConnection, results);
        }
    }
}
