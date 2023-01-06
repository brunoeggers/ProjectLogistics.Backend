using ProjectLogistics.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLogistics.Data.DTOs
{
    public class PackageDTO
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string Address { get; set; }
        public EPackageStatus Status { get; set; }

        // Additional properties sent to the frontend
        public int WarehouseSlotId { get; set; }
        public string WarehouseSlotName { get; set; }
        public string WarehouseName { get; set; }
        public int WarehouseId { get; set; }
        public string StatusText { get { return Status.ToString(); } }
    }
}
