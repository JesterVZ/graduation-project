using MySql.Data.MySqlClient;
using RetingGetter.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RetingGetter
{
    class Program
    {
        static string API_key = "0af5c4bc-d53b-4478-86c2-a68ed6111a32";
        static string Request = "https://geocode-maps.yandex.ru/1.x/?format=json&apikey=";
        static void Main(string[] args)
        {
            Connection Connection = new Connection();
            using SqlConnection sqlConnection = Connection.ConnectToDataBase();
            using MySqlConnection connection = Connection.ConnectToLocalBase();
            List<Result> points = new List<Result>();
            List<Home> homes = new List<Home>();
            bool IsOnlySQL = false;

            try
            {
                sqlConnection.Open();
                IsOnlySQL = true;
            }
            catch
            {
                connection.Open();
                IsOnlySQL = false;
            }
            Getter getter = new Getter(IsOnlySQL, sqlConnection, connection, homes, points);
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    getter.HomeGetter();
                }
                if (i == 1)
                {
                    Calculator calculator = new Calculator(API_key, Request, IsOnlySQL, sqlConnection, connection, getter.Homes, getter.Results);
                    calculator.RatingCalculator();
                    getter.SqlConnection.Close();
                    calculator.SqlConnection.Close();
                }
                if (i == 2)
                {

                    InsertIntoTable insertIntoTable = new InsertIntoTable(sqlConnection, connection, homes, points);
                    insertIntoTable.Insert();
                }

            }
        }

    }
}
