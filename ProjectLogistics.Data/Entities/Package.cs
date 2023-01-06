using ProjectLogistics.Data.Enums;

namespace ProjectLogistics.Data.Entities
{
    /// <summary>
    /// Package table
    /// </summary>
    public class Package: BaseEntity
    {
        public string TrackingNumber { get; set; }
        public int WarehouseSlotId { get; set; }
        public string Address { get; set; }
        public EPackageStatus Status { get; set; }
    }
}
