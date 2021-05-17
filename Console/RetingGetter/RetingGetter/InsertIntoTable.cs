using log4net;
using MySql.Data.MySqlClient;
using RetingGetter.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetingGetter
{
    public class InsertIntoTable
    {
        SqlConnection SqlConnection;
        MySqlConnection MySqlConnection;
        List<Home> Homes;
        List<Result> Results;
        static ILog log = LogManager.GetLogger(typeof(InsertIntoTable));
        public InsertIntoTable(SqlConnection sqlConnection, MySqlConnection mySqlConnection, List<Home> homes, List<Result> points)
        {
            SqlConnection = sqlConnection;
            MySqlConnection = mySqlConnection;
            Homes = homes;
            Results = points;

        }
        public void Insert()
        {
            using SqlConnection sqlConnection = SqlConnection;
            SqlConnection.Open();
            for (int i = 0; i < Results.Count; i++)
            {
                string commandText = "INSERT INTO Results (Address, X, Y, Rating) VALUES (@Address, @X, @Y, @Rating);";
                SqlCommand command = new SqlCommand(commandText, sqlConnection);
                SqlParameter address = new SqlParameter("@Address", Results[i].Address);
                command.Parameters.Add(address);
                SqlParameter rating = new SqlParameter("@Rating", Results[i].rating);
                command.Parameters.Add(rating);
                SqlParameter X = new SqlParameter("@X", Results[i].coords[0]);
                command.Parameters.Add(X);
                SqlParameter Y = new SqlParameter("@Y", Results[i].coords[1]);
                command.Parameters.Add(Y);
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine(i);
                    //log.Info("Добавлен дом по адресу" + Results[i].Address + "С рейтингом " + Results[i].rating);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            SqlConnection.Close();

        }
    }
}
