using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace MapAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        [HttpGet]
        public List<Point> Get()
        {
            using MySqlConnection connection = Connect();
            using SqlConnection sqlConnection = ConnectToDataBase();

            ListFunctions listFunctions = new ListFunctions();
            List<Point> points = new List<Point>();
            List<Home> homes = new List<Home>();

            sqlConnection.Open();
            for (int i = 0; i < 3; i++)
            {

                if(i == 0)
                {
                    SqlDataReader reader = Reader(sqlConnection, "address");
                    while (reader.Read())
                    {
                        homes.Add(new Home
                        {
                            Id = (int)reader["house_id"],
                            Street = (string)reader["street_name"],
                            Number = (int)reader["house_number"]
                        });
                        points.Add(new Point
                        {
                            Address = "Пермь, улица "+(string)reader["street_name"] + ", дом " + (int)reader["house_number"]
                        });
                    }
                    reader.Close();
                }
                if(i == 1)
                {
                    if(homes.Count %2 == 0)
                    {
                        int countOfThreads = 2;
                        for(int j = 0; j < countOfThreads; j++)
                        {

                        }
                    }
                    RatingCalculator(0, homes, points, sqlConnection);

                }
                if (i == 2)
                {
                    return points;
                }
            }

            return null;

        }
        private void RatingCalculator(int start, List<Home> homes, List<Point> points, SqlConnection sqlConnection)
        {
            for (int k = start; k < homes.Count; k++)
            {
                SqlDataReader reader = Reader(sqlConnection, "other", "SELECT dbo.Rating(" + homes[k].Id + ") rating");
                try
                {
                    while (reader.Read())
                    {
                        points[k].rating = (decimal)reader["rating"];
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    points[k].error = e.ToString();
                    reader.Close();
                }
            }
        }
        private SqlConnection ConnectToDataBase()
        {
            return new SqlConnection("Server=ibm-sql-cl2\\test;Database=general_minus_1;Trusted_Connection=True;");
        }
        private MySqlConnection Connect()
        {
            return new MySqlConnection("SERVER=localhost; DATABASE=homes;UID=root;PASSWORD=;");
        }

        private SqlDataReader Reader(SqlConnection connection, string state)
        {
            SqlCommand Command;
            string commandText;
            switch (state)
            {
                case "address":
                    commandText = "SELECT TOP 10 pgh.house_id, 'Пермь' city_nama, pgs.street_name, pgh.house_number, pgh.building FROM pes_geo_houses pgh, pes_geo_streets pgs WHERE pgh.street_id = pgs.street_id AND pgs.city_id =  653435 AND exists (SELECT 1 FROM pes_geo_apartments pga, pes_point_plugins ppp, pes_addendas pa WHERE pga.house_id = pgh.house_id AND pga.apartment_id = ppp.apartment_id AND ppp.addendum_id = pa.addendum_id)";
                    Command = new SqlCommand(commandText, connection);
                    return Command.ExecuteReader();

            }

            return null;
        }
        private SqlDataReader Reader(SqlConnection connection, string state, string command)
        {
            SqlCommand Command;
            string commandText;
            switch (state)
            {
                case "other":
                    commandText = command;
                    Command = new SqlCommand(commandText, connection);
                    return Command.ExecuteReader();
            }

            return null;
        }
    }
}
