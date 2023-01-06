using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ProjectLogistics.Data.Repositories
{
    /// <summary>
    /// This class serves as a base for all repositories.
    /// It contains helper methods that could be shared between
    /// multiple repositories
    /// </summary>
    public class BaseRepository
    {
        protected readonly ILogger _log;
        protected readonly DatabaseConfiguration _config;

        public BaseRepository(ILogger log, DatabaseConfiguration config)
        {
            _log = log;
            _config = config;
        }

        /// <summary>
        /// Creates a connection with the database
        /// </summary>
        /// <returns>A <see cref="SqliteConnection"/> object</returns>
        protected IDbConnection CreateConnection()
        {
            var connection = new SqliteConnection(_config.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}