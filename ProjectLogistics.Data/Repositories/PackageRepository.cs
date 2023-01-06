using Dapper;
using Microsoft.Extensions.Logging;
using ProjectLogistics.Data.DTOs;
using ProjectLogistics.Data.Entities;
using ProjectLogistics.Data.Enums;

namespace ProjectLogistics.Data.Repositories
{
    /// <summary>
    /// Handles all database-related tasks for packages
    /// </summary>
    public interface IPackageRepository
    {
        /// <summary>
        /// Get details for a specific package, including information
        /// about the warehouse and shelf it is stored
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns><see cref="PackageDTO"/> object</returns>
        Task<PackageDTO> Get(int packageId);

        /// <summary>
        /// Saves the new package into the database
        /// </summary>
        /// <param name="package"></param>
        /// <returns><see cref="Package"/> object</returns>
        Task<Package> Create(Package package);

        /// <summary>
        /// Get a list of all packages in all warehouses
        /// </summary>
        /// <returns>List of <see cref="Package"/></returns>
        Task<IEnumerable<PackageDTO>> GetAllPackages();

        /// <summary>
        /// Returns a list of all packages assigned to a specific warehouse
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns>List of <see cref="Package"/> object</returns>
        Task<IEnumerable<Package>> GetStoredPackages(int warehouseId);

        /// <summary>
        /// Updates the status of a package. Also handles unlinking of a package
        /// in case the new status is Shipped
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <param name="status">New status</param>
        Task UpdateStatus(int packageId, EPackageStatus status);

        /// <summary>
        /// Delete a package from the system
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        Task Delete(int packageId);
    }

    /// <summary>
    /// Handles all database-related tasks for packages
    /// </summary>
    public class PackageRepository : BaseRepository, IPackageRepository
    {
        public PackageRepository(ILogger<PackageRepository> log, DatabaseConfiguration dbConfig): base(log,dbConfig) 
        {
        }

        /// <summary>
        /// Get details for a specific package, including information
        /// about the warehouse and shelf it is stored
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns><see cref="PackageDTO"/> object</returns>
        public async Task<PackageDTO> Get(int packageId)
        {
            using (var connection = CreateConnection())
            {
                string sql = $"SELECT p.*,ws.Name AS WarehouseSlotName,ws.WarehouseId " +
                             $"FROM Package p " +
                             $"LEFT JOIN WarehouseSlot ws ON ws.Id = p.WarehouseSlotId " +
                             $"WHERE p.Id = @packageId;";

                return await connection.QueryFirstOrDefaultAsync<PackageDTO>(sql, new { packageId });
            }
        }

        /// <summary>
        /// Saves the new package into the database
        /// </summary>
        /// <param name="package"></param>
        /// <returns><see cref="Package"/> object</returns>
        public async Task<Package> Create(Package package)
        {
            using (var connection = CreateConnection())
            {
                package.Id = await connection.ExecuteAsync("INSERT INTO Package (WarehouseSlotId, TrackingNumber, Address, Status)" +
                "VALUES (@WarehouseSlotId, @TrackingNumber, @Address, @Status);", package);
            }

            return package;
        }

        /// <summary>
        /// Get a list of all packages in all warehouses
        /// </summary>
        /// <returns>List of <see cref="Package"/></returns>
        public async Task<IEnumerable<PackageDTO>> GetAllPackages()
        {
            using (var connection = CreateConnection())
            {
                string sql = $"SELECT p.*,ws.Name AS WarehouseSlotName, w.Name as WarehouseName, w.Id as WarehouseId " +
                             $"FROM Package p " +
                             $"LEFT JOIN WarehouseSlot ws ON ws.Id = p.WarehouseSlotId " +
                             $"LEFT JOIN Warehouse w ON w.Id = ws.WarehouseId;";

                return await connection.QueryAsync<PackageDTO>(sql);
            }
        }

        /// <summary>
        /// Returns a list of all packages assigned to a specific warehouse
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns>List of <see cref="Package"/> object</returns>
        public async Task<IEnumerable<Package>> GetStoredPackages(int warehouseId)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<Package>("SELECT * FROM Package WHERE WarehouseSlotId = @warehouseId;", new { warehouseId });
            }
        }

        /// <summary>
        /// Updates the status of a package. Also handles unlinking of a package
        /// in case the new status is Shipped
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <param name="status">New status</param>
        public async Task UpdateStatus(int packageId, EPackageStatus status)
        {
            using (var connection = CreateConnection())
            {
                if (status == EPackageStatus.Shipped)
                {
                    await connection.ExecuteAsync("UPDATE Package SET Status=@status,WarehouseSlotId=null WHERE Id = @packageId;", new { status, packageId });
                }
                else
                {
                    await connection.ExecuteAsync("UPDATE Package SET Status=@status WHERE Id = @packageId;", new { status, packageId });
                }
            }
        }

        /// <summary>
        /// Delete a package from the system
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        public async Task Delete(int packageId)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync("DELETE FROM Package WHERE Id = @packageId;", new { packageId });
            }
        }
    }
}
