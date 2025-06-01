using Frontend.Models.Booking;
using Frontend.Models.Event.Responses;
using Frontend.Models.Event.ViewModels;
using Frontend.Models.Location;
using Frontend.Models.Ticket;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Models.EventDetails
{
    public class EventDetailsPageView
    {
        public EventResponseModel? Event { get; set; }
        public AddBookingFormView BookingForm { get; set; } = new AddBookingFormView();
        public LocationModel? EventLocation { get; set; }
        public List<TicketViewModel>? Tickets { get; set; } = new List<TicketViewModel>();
        public EventViewModel? EventDetails { get; set; }
        public IEnumerable<SelectListItem>? CategoryFilterOptions { get; set; }
        public List<MerchViewModel>? Merch { get; set; } = new List<MerchViewModel>();
    }
}
