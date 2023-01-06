namespace ProjectLogistics.Data.DTOs
{
    public class WarehouseSlotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WarehouseId { get; set; }
        public int? PackageId { get; set; }
        public string? PackageTrackingNumber { get; set; }
        public string PackageAddress { get; set; }

        // Helper properties calculated based on other values
        public bool IsFree
        {
            get
            {
                return !PackageId.HasValue;
            }
        }
        public string Aisle
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? "A" : Name.Contains('-') ? Name.Split('-')[0] : "A";
            }
        }
        public string Shelf
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? "01" : Name.Contains('-') ? Name.Split('-')[1] : "01";
            }
        }
    }
}