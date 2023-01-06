namespace ProjectLogistics.Data.DTOs
{
    public class CreatePackageDTO
    {
        public string TrackingNumber { get; set; }
        public string Address { get; set; }
        public int WarehouseId { get; set; }
        public int? WarehouseSlotId { get; set; }
    }
}
