using ProjectLogistics.Data.DTOs;
using ProjectLogistics.Data.Entities;
using ProjectLogistics.Data.Enums;
using ProjectLogistics.Data.Repositories;

namespace ProjectLogistics.Core.Services
{
    /// <summary>
    /// <see cref="PackageService"/> Service used to contain the business logic for Packages
    /// </summary>
    public interface IPackageService
    {
        /// <summary>
        /// Get details for a specific package
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns><see cref="PackageDTO"/> object</returns>
        Task<PackageDTO> Get(int packageId);

        /// <summary>
        /// Creates a new Package and save to the database
        /// </summary>
        /// <param name="packageDTO"></param>
        /// <returns>The created <see cref="Package"/> object</returns>
        Task<Package> Create(CreatePackageDTO packageDTO);

        /// <summary>
        /// Get a list of packages stored in a given warehouse
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns>List of <see cref="Package"/></returns>
        Task<IEnumerable<Package>> GetStoredPackages(int warehouseId);

        /// <summary>
        /// Update the status of a package to Shipped
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns>True if successfull, false if not</returns>
        Task<bool> MarkAsShipped(int packageId);

        /// <summary>
        /// Delete a package
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns>True if successfull, false if not</returns>
        Task<bool> DeletePackage(int packageId);

        /// <summary>
        /// Get a list of all packages
        /// </summary>
        /// <returns>List of <see cref="Package"/></returns>
        Task<IEnumerable<PackageDTO>> GetAllPackages();
    }

    /// <summary>
    /// Service used to contain the business logic for Packages
    /// </summary>
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _repository;
        private readonly IWarehouseRepository _warehouseRepository;

        public PackageService(IPackageRepository repository, IWarehouseRepository warehouseRepository)
        {
            _repository = repository;
            _warehouseRepository = warehouseRepository;
        }

        /// <summary>
        /// Get details for a specific package
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns><see cref="PackageDTO"/> object</returns>
        public async Task<PackageDTO> Get(int packageId)
        {
            return await _repository.Get(packageId);
        }

        /// <summary>
        /// Creates a new Package and save to the database
        /// </summary>
        /// <param name="packageDTO"></param>
        /// <returns>The created <see cref="Package"/> object</returns>
        public async Task<Package> Create(CreatePackageDTO packageDTO)
        {
            // If no shelf was assigned, find one automatically
            if (!packageDTO.WarehouseSlotId.HasValue)
            {
                var freeSlot = await _warehouseRepository.GetRandomFreeSlot(packageDTO.WarehouseId);

                if (freeSlot != null) packageDTO.WarehouseSlotId = freeSlot.Id;
                else throw new Exception("There are no free shelves in this warehouse");
            }
            // If a shelf was assigned, ensure it is free
            else
            {
                var isFree = await _warehouseRepository.CheckIfSlotIsFree(packageDTO.WarehouseSlotId.Value);

                if (!isFree) throw new Exception("The provided shelf is not free");
            }

            // Creates a new instance of a Package object
            var package = new Package()
            {
                TrackingNumber = packageDTO.TrackingNumber,
                WarehouseSlotId = packageDTO.WarehouseSlotId.Value,
                Address = packageDTO.Address,
                Status = EPackageStatus.InStorage
            };

            return await _repository.Create(package);
        }

        /// <summary>
        /// Get a list of all packages
        /// </summary>
        /// <returns>List of <see cref="Package"/></returns>
        public async Task<IEnumerable<PackageDTO>> GetAllPackages()
        {
            return await _repository.GetAllPackages();
        }

        /// <summary>
        /// Get a list of packages stored in a given warehouse
        /// </summary>
        /// <param name="warehouseId">Id of the warehouse</param>
        /// <returns>List of <see cref="Package"/></returns>
        public async Task<IEnumerable<Package>> GetStoredPackages(int warehouseId)
        {
            return await _repository.GetStoredPackages(warehouseId);
        }

        /// <summary>
        /// Update the status of a package to Shipped
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns>True if successfull, false if not</returns>
        public async Task<bool> MarkAsShipped(int packageId)
        {
            var package = await _repository.Get(packageId);

            if (package != null)
            {
                await _repository.UpdateStatus(packageId, EPackageStatus.Shipped);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete a package
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <returns>True if successfull, false if not</returns>
        public async Task<bool> DeletePackage(int packageId)
        {
            var package = await _repository.Get(packageId);

            if (package != null)
            {
                await _repository.Delete(packageId);
                return true;
            }

            return false;
        }
    }
}
