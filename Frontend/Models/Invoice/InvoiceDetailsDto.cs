using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Invoice;

public class InvoiceDetailsDto
{
    
    public DateTime CreatedDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Title { get; set; }
    public int Price { get; set; }
    public int Qty { get; set; }
    public decimal Amount { get; set; }

    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerPostalCode { get; set; }
    public string CustomerCity { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }

    public string EventName { get; set; }
    public string EventAddress { get; set; }
    public string EventCity { get; set; }
    public string EventEmail { get; set; }
    public string EventPhone { get; set; }

}
