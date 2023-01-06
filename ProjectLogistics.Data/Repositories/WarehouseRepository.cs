using Dapper;
using Microsoft.Extensions.Logging;
using ProjectLogistics.Data.DTOs;
using ProjectLogistics.Data.Entities;

namespace ProjectLogistics.Data.Repositories
{
    /// <summary>
    /// Handles all database-related tasks for warehouses
    /// </summary>
    public interface IWarehouseRepository
    {
        /// <summary>
        /// Retrieves a list of all warehouses
        /// </summary>
        /// <returns>List of <see cref="Warehouse"/></returns>
        Task<IEnumerable<Warehouse>> GetWarehouses();

        /// <summary>
        /// Get details for a specific warehouse, including information
        /// about all the shelves it has
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns><see cref="WarehouseDTO"/> object</returns>
        Task<WarehouseDTO?> GetWarehouse(int warehouseId);

        /// <summary>
        /// Returns a random free shelf for a specific warehouse
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns>A random free <see cref="WarehouseSlot"/> object</returns>
        Task<WarehouseSlot?> GetRandomFreeSlot(int warehouseId);

        /// <summary>
        /// Checks if a specific shelf is free
        /// </summary>
        /// <param name="warehouseSlotId">Id of the shelf</param>
        /// <returns>True if free, false if not</returns>
        Task<bool> CheckIfSlotIsFree(int warehouseSlotId);
    }

    /// <summary>
    /// Handles all database-related tasks for warehouses
    /// </summary>
    public class WarehouseRepository : BaseRepository, IWarehouseRepository
    {
        public WarehouseRepository(ILogger<WarehouseRepository> log, DatabaseConfiguration dbConfig) : base(log, dbConfig)
        {
        }

        /// <summary>
        /// Retrieves a list of all warehouses
        /// </summary>
        /// <returns>List of <see cref="Warehouse"/></returns>
        public async Task<IEnumerable<Warehouse>> GetWarehouses()
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Warehouse>("SELECT * FROM Warehouse;");
            }
        }

        /// <summary>
        /// Get details for a specific warehouse, including information
        /// about all the shelves it has
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns><see cref="WarehouseDTO"/> object</returns>
        public async Task<WarehouseDTO?> GetWarehouse(int warehouseId)
        {
            using (var connection = CreateConnection())
            {
                string sql = $"SELECT Id,Name,Latitude,Longitude " +
                             $"FROM Warehouse " +
                             $"WHERE Id = @warehouseId;" +

                             $"SELECT ws.*,p.Id AS PackageId,p.TrackingNumber AS PackageTrackingNumber,p.Address AS PackageAddress " +
                             $"FROM WarehouseSlot ws " +
                             $"LEFT JOIN Package p ON p.WarehouseSlotId=ws.Id " +
                             $"WHERE ws.WarehouseId = @warehouseId;";
                
                // Run multiple queries
                var query = await connection.QueryMultipleAsync(sql, new { warehouseId });

                // The first result is for our Warehouse object
                var warehouse = query.Read<WarehouseDTO>().FirstOrDefault();

                // If we found a warehouse, get the results of the second query
                if (warehouse != null)
                    warehouse.Slots = query.Read<WarehouseSlotDTO>().ToList();

                return warehouse;
            }
        }

        /// <summary>
        /// Returns a random free shelf for a specific warehouse
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns>A random free <see cref="WarehouseSlot"/> object</returns>
        public async Task<WarehouseSlot?> GetRandomFreeSlot(int warehouseId)
        {
            using (var connection = CreateConnection())
            {
                string sql = $"SELECT * FROM WarehouseSlot ws " +
                             $"WHERE WarehouseId = @warehouseId AND NOT EXISTS (SELECT 1 FROM Package WHERE WarehouseSlotId = ws.Id) " +
                             $"ORDER BY RANDOM() LIMIT 1;";

                return await connection.QueryFirstOrDefaultAsync<WarehouseSlot>(sql, new { warehouseId });
            }
        }

        /// <summary>
        /// Checks if a specific shelf is free
        /// </summary>
        /// <param name="warehouseSlotId">Id of the shelf</param>
        /// <returns>True if free, false if not</returns>
        public async Task<bool> CheckIfSlotIsFree(int warehouseSlotId)
        {
            using (var connection = CreateConnection())
            {
                string sql = $"SELECT NOT EXISTS (SELECT 1 FROM Package WHERE WarehouseSlotId = @warehouseSlotId);";

                return await connection.QueryFirstOrDefaultAsync<bool>(sql, new { warehouseSlotId });
            }
        }
    }
}
