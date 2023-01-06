namespace ProjectLogistics.Data.Entities
{
    // WarehouseSlot table
    public class WarehouseSlot: BaseEntity
    {
        public string Name { get; set; }
        public int WarehouseId { get; set; }
    }
}
