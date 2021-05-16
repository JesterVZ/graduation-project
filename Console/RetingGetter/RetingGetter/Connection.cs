using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetingGetter
{
    public class Connection
    {
        public SqlConnection ConnectToDataBase()
        {
            return new SqlConnection("Server=ibm-sql-cl2\\test;Database=general_minus_1;Trusted_Connection=True;");
        }
        public  MySqlConnection ConnectToLocalBase()
        {
            return new MySqlConnection("SERVER=localhost; DATABASE=homes;UID=root;PASSWORD=;");
        }
        public SqlDataReader Reader(SqlConnection connection, string state)
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
        public SqlDataReader Reader(SqlConnection connection, string state, string command)
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
        public MySqlDataReader LocalReader(MySqlConnection connection, string command)
        {
            MySqlCommand Command;
            string commandText = command;
            Command = new MySqlCommand(commandText, connection);
            return Command.ExecuteReader();
        }
    }
}
