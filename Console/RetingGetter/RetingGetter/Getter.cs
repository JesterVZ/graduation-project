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
    public class Getter
    {
        bool Key = false;
        public SqlConnection SqlConnection;
        MySqlConnection MySqlConnection;
        public List<Home> Homes;
        public List<Result> Results;
        Connection Connection;
        public Getter(bool key, SqlConnection sqlConnection, MySqlConnection mySqlConnection, List<Home> homes, List<Result> points)
        {
            Key = key;
            SqlConnection = sqlConnection;
            MySqlConnection = mySqlConnection;
            Homes = homes;
            Results = points;
            Connection = new Connection();
        }
        public void HomeGetter()
        {
            if (Key)
            {
                SqlDataReader reader = Connection.Reader(SqlConnection, "address");
                while (reader.Read())
                {
                    Homes.Add(new Home
                    {
                        Id = (int)reader["house_id"],
                        Street = (string)reader["street_name"],
                        Number = (int)reader["house_number"]
                    });
                    Results.Add(new Result
                    {
                        Address = "Пермь, улица " + (string)reader["street_name"] + ", дом " + (int)reader["house_number"]
                    });
                }
                reader.Close();
            } else
            {
                MySqlDataReader reader = Connection.LocalReader(MySqlConnection, "SELECT * FROM localdata");
                while (reader.Read())
                {
                    Homes.Add(new Home
                    {
                        Id = (int)reader["house_id"],
                        Street = (string)reader["street_name"],
                        Number = (int)reader["house_number"]
                    });
                    Results.Add(new Result
                    {
                        Address = "Пермь, улица " + (string)reader["street_name"] + ", дом " + (int)reader["house_number"]
                    });
                }
                reader.Close();
            }

        }
    }
}
