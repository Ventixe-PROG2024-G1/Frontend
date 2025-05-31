using Frontend.Models.Booking;
using Frontend.Models.Event.Responses;
using Frontend.Models.Location;
using Frontend.Models.Ticket;

namespace Frontend.Models.EventDetails
{
    public class EventDetailsPageView
    {
        public EventResponseModel? Event { get; set; }
        public AddBookingFormView BookingForm { get; set; } = new AddBookingFormView();
        public LocationModel? EventLocation { get; set; }
        public List<TicketViewModel>? Tickets { get; set; } = new List<TicketViewModel>();
    }
}
