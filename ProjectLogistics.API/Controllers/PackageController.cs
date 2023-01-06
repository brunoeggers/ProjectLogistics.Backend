using Microsoft.AspNetCore.Mvc;
using ProjectLogistics.Core.Services;
using ProjectLogistics.Data.DTOs;
using ProjectLogistics.Data.Entities;

namespace ProjectLogistics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PackageController : ControllerBase
    {
        private readonly ILogger<PackageController> _logger;
        private readonly IPackageService _packageService;

        public PackageController(ILogger<PackageController> logger, IPackageService packageService)
        {
            _logger = logger;
            _packageService = packageService;
        }

        [HttpGet()]
        public async Task<IEnumerable<PackageDTO>> GetAllPackages()
        {
            // Get a list of all packages
            return await _packageService.GetAllPackages();
        }

        [HttpGet("{packageId}")]
        public async Task<PackageDTO> GetPackage(int packageId)
        {
            // Returns details of a specific package
            return await _packageService.Get(packageId);
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreatePackageDTO packageDTO)
        {
            try
            {
                // Basic validation
                if (packageDTO == null) throw new ArgumentNullException(nameof(packageDTO));

                if (string.IsNullOrEmpty(packageDTO.TrackingNumber)) throw new ArgumentException("Tracking Number is required");
                if (packageDTO.TrackingNumber.Length < 6) throw new ArgumentException("Tracking Number must be at least 6 characters");
                if (packageDTO.TrackingNumber.Length > 30) throw new ArgumentException("Tracking Number must not exceed 30 characters");

                if (string.IsNullOrEmpty(packageDTO.Address)) throw new ArgumentException("Address is required");
                if (packageDTO.Address.Length < 6) throw new ArgumentException("Address must be at least 6 characters");
                if (packageDTO.Address.Length > 350) throw new ArgumentException("Address must not exceed 350 characters");

                // Further validation will be done inside the service
                // Creates a new package
                return Ok(await _packageService.Create(packageDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{packageId}")]
        public async Task<bool> MarkPackageAsShipped(int packageId)
        {
            // Marks a package as shipped
            return await _packageService.MarkAsShipped(packageId);
        }

        [HttpDelete("{packageId}")]
        public async Task<bool> DeletePackage(int packageId)
        {
            // Deletes a specific package
            return await _packageService.DeletePackage(packageId);
        }
    }
}