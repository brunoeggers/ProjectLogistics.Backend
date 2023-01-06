using Microsoft.AspNetCore.Mvc;
using ProjectLogistics.Core.Services;
using ProjectLogistics.Data.DTOs;
using ProjectLogistics.Data.Entities;

namespace ProjectLogistics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(ILogger<WarehouseController> logger, IWarehouseService warehouseService)
        {
            _logger = logger;
            _warehouseService = warehouseService;
        }

        [HttpGet()]
        public async Task<IEnumerable<Warehouse>> GetWarehouses()
        {
            // Return all warehouses
            return await _warehouseService.GetWarehouses();
        }

        [HttpGet("{warehouseId}")]
        public async Task<WarehouseDTO?> GetWarehouse(int warehouseId)
        {
            // Gets data from a specific warehouse
            return await _warehouseService.GetWarehouse(warehouseId);
        }
    }
}