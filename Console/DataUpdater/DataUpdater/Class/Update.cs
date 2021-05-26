using DataUpdater.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUpdater.Class
{
    public class Update
    {
        Connection connection = new Connection();
        public void UpdateDate(SqlConnection sqlConnection, List<Result> results)
        {
            sqlConnection.Open();
            SqlDataReader reader = connection.Reader(sqlConnection, "SELECT * FROM Results r");
            while (reader.Read())
            {
                results.Add(new Result
                {
                    Id = (int)reader["Id"],
                    rating = (decimal)reader["Rating"]
                });
            }
            reader.Close();
            for(int i = 0; i < results.Count; i++)
            {
                string commandText = "UPDATE Results SET Rating = dbo.Rating(@houseID) WHERE Id = @houseID";
                SqlCommand command = new SqlCommand(commandText, sqlConnection);
                SqlParameter houseId = new SqlParameter("@houseID", results[i].Id);
                command.Parameters.Add(houseId);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {

                }
            }
            sqlConnection.Close();
        }
    }
}
