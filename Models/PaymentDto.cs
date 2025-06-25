namespace JamilNativeAPI.Models
{
    public class PaymentDto
    {
        public int WaterbillId { get; set; }

        public decimal AmountOwed { get; set; }

        public DateTime AddedOn { get; set; }

    }
}
