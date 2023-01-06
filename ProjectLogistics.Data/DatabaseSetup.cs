using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLogistics.Data
{
    /// <summary>
    /// Helper to setup the database if needed
    /// </summary>
    public interface IDatabaseSetup
    {
        void Setup();
    }

    /// <summary>
    /// Helper to setup the database if needed
    /// </summary>
    public class DatabaseSetup : IDatabaseSetup
    {
        private readonly DatabaseConfiguration _dbConfig;

        public DatabaseSetup(DatabaseConfiguration dbConfig)
        {
            _dbConfig = dbConfig;
        }

        /// <summary>
        /// This method creates all the tables and seeds default data
        /// if the database is not found
        /// </summary>
        public void Setup()
        {
            using var connection = new SqliteConnection(_dbConfig.ConnectionString);

            // Check if the Warehouse table exists
            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Warehouse';");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "Warehouse")
                return;

            // Create the Warehouse table
            connection.Execute("CREATE TABLE Warehouse (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Latitude REAL NOT NULL," +
                "Longitude REAL NOT NULL," +
                "Name VARCHAR(20) NOT NULL);");

            // Insert mock data
            connection.Execute("INSERT INTO Warehouse (Name, Latitude, Longitude) " +
                "VALUES " +
                "('San Francisco', 37.72585854879952, -122.38684218300128);");

            // Create the WarehouseSlot table
            connection.Execute("CREATE TABLE WarehouseSlot (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "WarehouseId INTEGER NOT NULL," +
                "Name VARCHAR(20) NOT NULL);");

            // Insert mock data
            connection.Execute("INSERT INTO WarehouseSlot (Name, WarehouseId) " +
                "VALUES " +
                "('A-01', 1)," +
                "('A-02', 1)," +
                "('A-03', 1)," +
                "('A-04', 1)," +
                "('A-05', 1)," +
                "('B-01', 1)," +
                "('B-02', 1)," +
                "('B-03', 1)," +
                "('B-04', 1)," +
                "('B-05', 1)," +
                "('C-01', 1)," +
                "('C-02', 1)," +
                "('C-03', 1)," +
                "('C-04', 1)," +
                "('D-01', 1)," +
                "('D-02', 1);");

            // Create the Package table
            connection.Execute("CREATE TABLE Package (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "WarehouseSlotId INTEGER NULL," +
                "Address VARCHAR(350) NOT NULL," +
                "Status TINYINT DEFAULT 0," +
                "TrackingNumber VARCHAR(32) NOT NULL);");

            // Insert mock data
            connection.Execute("INSERT INTO Package (WarehouseSlotId, TrackingNumber, Address) " +
                "VALUES " +
                "(1,'MX-1872BR', '398 Peninsula Ave, San Francisco, CA 94134, USA')," +
                "(3,'BR-1AJXA2BX', '580 Raymond Ave, San Francisco, CA 94134, USA')," +
                "(4,'JPAX-72BA6R', '1480 Golden Gate Ave, San Francisco, CA 94115, USA')," +
                "(7,'US-ABAC16R', '4 Carolina St, San Francisco, CA 94103, USA')," +
                "(8,'15612X-LB5A', '245 Brentwood Ave, San Francisco, CA 94127, USA')," +
                "(13,'1HY18-SAB3', '5937 Geary Blvd, San Francisco, CA 94121, USA');");
        }
    }
}
