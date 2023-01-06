using ProjectLogistics.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLogistics.Data.DTOs
{
    public class WarehouseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public List<WarehouseSlotDTO> Slots { get; set; }
    }
}
