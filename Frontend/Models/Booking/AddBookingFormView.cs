using System.ComponentModel.DataAnnotations;

namespace Frontend.Models.Booking;

public class AddBookingFormView
{
    [Required(ErrorMessage = "Ticket quantity is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Ticket quantity must be at least 1.")]
    public int TicketQuantity { get; set; }

    [Required(ErrorMessage = "Event ID is required.")]
    [RegularExpression(
    @"^(\{)?[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\})?$",
    ErrorMessage = "Event ID must be a valid GUID string.")]
    public string EventId { get; set; } = null!;

    [Required(ErrorMessage = "Ticket ID is required.")]
    [RegularExpression(
    @"^(\{)?[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\})?$",
    ErrorMessage = "Ticket ID must be a valid GUID string.")]
    public string TicketId { get; set; } = null!;
}
