using ProjectLogistics.Data.DTOs;
using ProjectLogistics.Data.Entities;
using ProjectLogistics.Data.Repositories;

namespace ProjectLogistics.Core.Services
{
    /// <summary>
    /// <see cref="WarehouseService"/> Service used to contain the business logic for Warehouses
    /// </summary>
    public interface IWarehouseService
    {
        /// <summary>
        /// Get a list of warehouses
        /// </summary>
        /// <returns>List of <see cref="Warehouse"/></returns>
        Task<IEnumerable<Warehouse>> GetWarehouses();

        /// <summary>
        /// Gets details of a given warehouse. 
        /// The returned object also brings all packages allocated to its shelves.
        /// If the warehouse is not found, null is returned.
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns><see cref="Warehouse"/> object if found, null if not found</returns>
        Task<WarehouseDTO?> GetWarehouse(int warehouseId);
    }

    /// <summary>
    /// Service used to contain the business logic for Warehouses
    /// </summary>
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _repository;

        public WarehouseService(IWarehouseRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get a list of warehouses
        /// </summary>
        /// <returns>List of <see cref="Warehouse"/></returns>
        public async Task<IEnumerable<Warehouse>> GetWarehouses()
        {
            return await _repository.GetWarehouses();
        }

        /// <summary>
        /// Gets details of a given warehouse. 
        /// The returned object also brings all packages allocated to its shelves.
        /// If the warehouse is not found, null is returned.
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns><see cref="Warehouse"/> object if found, null if not found</returns>
        public async Task<WarehouseDTO?> GetWarehouse(int warehouseId)
        {
            return await _repository.GetWarehouse(warehouseId);
        }
    }
}
