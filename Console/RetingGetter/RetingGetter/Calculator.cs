using MapAPI.Geo;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RetingGetter.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RetingGetter
{
    public class Calculator
    {
        bool Key = false;
        public SqlConnection SqlConnection;
        MySqlConnection MySqlConnection;
        List<Home> Homes;
        List<Result> Results;
        Connection Connection;
        string Request;
        string API_key;

        public Calculator(string api, string request, bool key, SqlConnection sqlConnection, MySqlConnection mySqlConnection, List<Home> homes, List<Result> points)
        {
            Connection = new Connection();
            Request = request;
            API_key = api;
            SqlConnection = sqlConnection;
            MySqlConnection = mySqlConnection;
            Homes = homes;
            Results = points;
            Key = key;
        }
        public void RatingCalculator()
        {
            Answer answer = new Answer();
            if (Key)
            {
                for (int k = 0; k < Homes.Count; k++)
                {
                    Results[k].coords = new double[2];
                    SqlDataReader reader = Connection.Reader(SqlConnection, "other", "SELECT dbo.Rating(" + Homes[k].Id + ") rating");
                    try
                    {
                        while (reader.Read())
                        {
                            string link = Request + API_key + "&geocode=" + Results[k].Address;
                            string jsonString = new WebClient().DownloadString(link);
                            answer = JsonConvert.DeserializeObject<Answer>(jsonString);
                            string[] coords = answer.Response.GeoObjectCollection.FeatureMember[0].GeoObject.Point.Pos.Split(' ');

                            Results[k].rating = (decimal)reader["rating"];
                            Results[k].coords[0] = Convert.ToDouble(coords[1], CultureInfo.InvariantCulture);
                            Results[k].coords[1] = Convert.ToDouble(coords[0], CultureInfo.InvariantCulture);
                        }
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Results[k].error = e.ToString();
                        reader.Close();
                    }
                    reader = Connection.Reader(SqlConnection, "other", "SELECT dbo.MeterIndex("+Homes[k].Id+") meter");
                    try
                    {
                        while (reader.Read())
                        {
                            Results[k].MeterIndex = (decimal)reader["meter"];
                        }
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Results[k].error = e.ToString();
                        reader.Close();
                    }


                    reader = Connection.Reader(SqlConnection, "other", "SELECT dbo.DebetorIndex(" + Homes[k].Id + ") debetor");
                    try
                    {
                        while (reader.Read())
                        {
                            Results[k].DebetorIndex = (decimal)reader["debetor"];
                        }
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Results[k].error = e.ToString();
                        reader.Close();
                    }

                    reader = Connection.Reader(SqlConnection, "other", "SELECT dbo.RequestIndexFunc(" + Homes[k].Id + ") request");
                    try
                    {
                        while (reader.Read())
                        {
                            Results[k].RepairIndex = (decimal)reader["request"];
                        }
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        Results[k].error = e.ToString();
                        reader.Close();
                    }
                }
            } else
            {
                for (int i = 0; i < Homes.Count; i++)
                {
                    Results[i].coords = new double[2];
                    MySqlDataReader reader = Connection.LocalReader(MySqlConnection, "SELECT rating FROM rating WHERE Id = " + Homes[i].Id);
                    while (reader.Read())
                    {

                        string link = Request + API_key + "&geocode=" + Results[i].Address;
                        string jsonString = new WebClient().DownloadString(link);
                        answer = JsonConvert.DeserializeObject<Answer>(jsonString);
                        string[] coords = answer.Response.GeoObjectCollection.FeatureMember[0].GeoObject.Point.Pos.Split(' ');
                        Results[i].rating = (int)reader["rating"];
                        Results[i].coords[0] = Convert.ToDouble(coords[1], CultureInfo.InvariantCulture);
                        Results[i].coords[1] = Convert.ToDouble(coords[0], CultureInfo.InvariantCulture);
                    }

                }
            }
            //SqlConnection.Close();
        }
    }
}
