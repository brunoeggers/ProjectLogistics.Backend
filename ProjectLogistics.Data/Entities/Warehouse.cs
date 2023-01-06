namespace ProjectLogistics.Data.Entities
{
    // Warehouse table
    public class Warehouse : BaseEntity
    {
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
