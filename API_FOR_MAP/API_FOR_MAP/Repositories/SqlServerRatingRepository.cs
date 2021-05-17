using API_FOR_MAP.Services;
using API_FOR_MAP.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API_FOR_MAP.Repositories
{
    public class SqlServerRatingRepository : IRatingRepository
    {
        private string _connectionString;
        public SqlServerRatingRepository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException();
            }
            _connectionString = connectionString;
        }
        public async Task<IReadOnlyCollection<Result>> GetHousesAsync()
        {
            using SqlConnection connection = CreateConnection();
            await connection.OpenAsync();
            string commandText = "SELECT * FROM Results";
            SqlCommand command = new SqlCommand(commandText, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            List<Result> results = new List<Result>();

            results = reader.Cast<IDataRecord>().Select(x => new Result(address: reader["Address"] as string, x: Convert.ToSingle(reader["X"]), y: Convert.ToSingle(reader["Y"]), rating: (decimal)reader["Rating"])).ToList<Result>();
            return results;
        }
        private SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }
    }
}
