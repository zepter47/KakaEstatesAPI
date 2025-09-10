using JamilNativeAPI.Entities;

namespace JamilNativeAPI.Models
{
    public class WaterBillDto
    {
        public int TenantId { get; set; }

        public int HouseId { get; set; }

        public decimal PreviousReading { get; set; }

        public decimal CurrentReading { get; set; }

        public DateTime AddedOn { get; set; }

        public string TenantFirstName { get; set; }= null!;

        public string TenantLastName { get; set; } = null!;

        public string tenant { get; set; } = null!;

        public string house { get; set; } = null!;

        public decimal payment { get; set; }

        public int BillNumber { get; set; }

        public decimal UnitsUsed { get; set; }

    }
}
