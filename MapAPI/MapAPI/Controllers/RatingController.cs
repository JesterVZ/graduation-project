using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
        public IEnumerable<Home> Get()
        {
            using MySqlConnection connection = Connect();
            connection.Open();
            MySqlDataReader reader = Reader(connection, "Data");
            int count = 0;
            while (reader.Read())
            {
                count++;
            }
            if(count > 0)
            {
                return Enumerable.Range(1, count).Select(index => new Home
                {
                    coordinates = new double[2]
                    {
                        (double)reader["X"],
                        (double)reader["Y"]
                    },
                    rating = (int)reader["Rating"]
                }).ToArray();
            }
            return null;

        }
        private MySqlConnection Connect()
        {
            return new MySqlConnection("SERVER=localhost; DATABASE=homes;UID=root;PASSWORD=;");
        }
        private MySqlDataReader Reader(MySqlConnection connection, string state)
        {
            MySqlCommand Command;
            string commandText;
            switch (state)
            {
                case "Data":
                    commandText = "SELECT * FROM data";
                    Command = new MySqlCommand(commandText, connection);
                    return Command.ExecuteReader();
            }
            return null;
        }
    }
}
