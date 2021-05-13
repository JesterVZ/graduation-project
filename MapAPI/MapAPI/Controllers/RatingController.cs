using MapAPI.Geo;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Net;
using RestSharp;
using System.Globalization;

namespace MapAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        string API_key = "0af5c4bc-d53b-4478-86c2-a68ed6111a32";
        string Request = "https://geocode-maps.yandex.ru/1.x/?format=json&apikey=";
        [HttpGet]
        public List<Result> Get()
        {
            using MySqlConnection connection = Connect();
            using SqlConnection sqlConnection = ConnectToDataBase();
            ListFunctions listFunctions = new ListFunctions();
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
            for (int i = 0; i < 3; i++)
            {

                if(i == 0)
                {
                    HouseGetter(IsOnlySQL, sqlConnection, connection, homes, points);
                }
                if(i == 1)
                {
                    RatingCalculator(0, homes, points, sqlConnection, connection, IsOnlySQL);
                }
                if (i == 2)
                {
                    return points;
                }
            }

            return null;

        }
        private void HouseGetter(bool key, SqlConnection sqlConnection, MySqlConnection mySqlConnection, List<Home> homes, List<Result> points)
        {
            if (key)
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
                    points.Add(new Result
                    {
                        Address = "Пермь, улица " + (string)reader["street_name"] + ", дом " + (int)reader["house_number"]
                    });
                }
                reader.Close();
            } else
            {
                MySqlDataReader reader = LocalReader(mySqlConnection, "SELECT * FROM localdata");
                while (reader.Read())
                {
                    homes.Add(new Home
                    {
                        Id = (int)reader["house_id"],
                        Street = (string)reader["street_name"],
                        Number = (int)reader["house_number"]
                    });
                    points.Add(new Result
                    {
                        Address = "Пермь, улица " + (string)reader["street_name"] + ", дом " + (int)reader["house_number"]
                    });
                }
                reader.Close();
            }
        }
        private void RatingCalculator(int start, List<Home> homes, List<Result> points, SqlConnection sqlConnection, MySqlConnection mySqlConnection, bool key)
        {
            Answer answer = new Answer();

            if (key)
            {
                for (int k = start; k < homes.Count; k++)
                {
                    points[k].coords = new double[2];
                    SqlDataReader reader = Reader(sqlConnection, "other", "SELECT dbo.Rating(" + homes[k].Id + ") rating");
                    try
                    {
                        while (reader.Read())
                        {
                            string link = Request + API_key + "&geocode=" + points[k].Address;
                            string jsonString = new WebClient().DownloadString(link);
                            answer = JsonConvert.DeserializeObject<Answer>(jsonString);
                            string[] coords = answer.Response.GeoObjectCollection.FeatureMember[0].GeoObject.Point.Pos.Split(' ');

                            points[k].rating = (decimal)reader["rating"];
                            points[k].coords[0] = Convert.ToDouble(coords[1], CultureInfo.InvariantCulture);
                            points[k].coords[1] = Convert.ToDouble(coords[0], CultureInfo.InvariantCulture);
                        }
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        points[k].error = e.ToString();
                        reader.Close();
                    }
                }
            } else
            {
                

                for (int i = start; i < homes.Count; i++)
                {
                    points[i].coords = new double[2];
                    MySqlDataReader reader = LocalReader(mySqlConnection, "SELECT rating FROM rating WHERE Id = "+homes[i].Id);
                    while (reader.Read())
                    {

                        string link = Request + API_key + "&geocode=" + points[i].Address;
                        string jsonString = new WebClient().DownloadString(link);
                        answer = JsonConvert.DeserializeObject<Answer>(jsonString);
                        string[] coords = answer.Response.GeoObjectCollection.FeatureMember[0].GeoObject.Point.Pos.Split(' ');
                        points[i].rating = (int)reader["rating"];
                        points[i].coords[0] = Convert.ToDouble(coords[1], CultureInfo.InvariantCulture);
                        points[i].coords[1] = Convert.ToDouble(coords[0], CultureInfo.InvariantCulture);
                    }

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
        private MySqlDataReader LocalReader(MySqlConnection connection, string command)
        {
            MySqlCommand Command;
            string commandText = command;
            Command = new MySqlCommand(commandText, connection);
            return Command.ExecuteReader();
        }
    }
}
