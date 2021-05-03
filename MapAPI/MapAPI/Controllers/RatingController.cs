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

            RatingCalculator ratingCalculator = new RatingCalculator();
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
                    }
                    reader.Close();
                }
                if(i == 1)
                {
                    for(int j = 0; j < homes.Count; j++)
                    {
                        string command = "SELECT * FROM pes_geo_apartments pga WHERE pga.house_id = " + homes[j].Id;
                        SqlDataReader reader = Reader(sqlConnection, "apartments", command);
                        int count = 0;
                        while (reader.Read())
                        {
                            count++;
                        }
                        homes[j].ApartmentCount = count;
                        count = 0;
                        reader.Close();
                    }
                }
                /*
                if(i == 0)
                {
                    MySqlDataReader reader = Reader(connection, "Home");
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        listFunctions.FillList(new Home
                        {
                            Address = (string)reader["Address"],
                            ApartmentCount = (int)reader["Apartment_Count"],
                            Numberofdebetors = (int)reader["Number_of_debetors"],
                            RepairCount = (int)reader["RepairCount"]
                        });
                    }
                    reader.Close();
                }
                if(i == 1)
                {
                    MySqlDataReader reader = Reader(connection, "Meter");
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                        listFunctions.FillList(new Meter
                        {
                            Date_of_creation = (DateTime)reader["Date_of_creation"],
                            Delta = DateTime.Now - (DateTime)reader["Date_of_creation"]
                        });
                    }
                    reader.Close();
                    for(int k = 0; k < listFunctions.Home.Count; k++)
                    {
                        int rating = ratingCalculator.CalculateIndex(listFunctions.Home[k], listFunctions.Meters[k]);
                        points.Add(new Point
                        {
                            Address = listFunctions.Home[k].Address,
                            rating = rating
                        });
                    }
                    
                }
                */
                if (i == 2)
                {
                    return points;
                }
            }

            return null;

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
                    commandText = "SELECT pgh.house_id, 'Пермь' city_nama, pgs.street_name, pgh.house_number, pgh.building FROM pes_geo_houses pgh, pes_geo_streets pgs WHERE pgh.street_id = pgs.street_id AND pgs.city_id =  653435 AND exists (SELECT 1 FROM pes_geo_apartments pga, pes_point_plugins ppp, pes_addendas pa WHERE pga.house_id = pgh.house_id AND pga.apartment_id = ppp.apartment_id AND ppp.addendum_id = pa.addendum_id)";
                    Command = new SqlCommand(commandText, connection);
                    return Command.ExecuteReader();
                case "apartments":
                    commandText = "SELECT COUNT(*) FROM pes_geo_apartments pga WHERE pga.house_id = 628095";
                    Command = new SqlCommand(commandText, connection);
                    return Command.ExecuteReader();
                case "Home":
                    commandText = "SELECT * FROM home";
                    Command = new SqlCommand(commandText, connection);
                    return Command.ExecuteReader();
                case "Meter":
                    commandText = "SELECT * FROM meters";
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
                case "apartments":
                    commandText = command;
                    Command = new SqlCommand(commandText, connection);
                    return Command.ExecuteReader();
            }

            return null;
        }
    }
}
