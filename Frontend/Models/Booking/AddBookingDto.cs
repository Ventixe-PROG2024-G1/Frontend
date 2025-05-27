using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Booking;

public class AddBookingDto
{
    public int TicketQuantity { get; set; }
    public decimal TicketPrice { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string UserPhone { get; set; } = null!;
    public string UserStreetAddress { get; set; } = null!;
    public string UserPostalCode { get; set; } = null!;
    public string UserCity { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string TicketId { get; set; } = null!;
}
