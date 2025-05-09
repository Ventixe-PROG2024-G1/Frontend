namespace Frontend.Models
{
    public class InvoiceDto
    {
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Amount { get; set; }
        public bool Paid { get; set; }
    }
}
