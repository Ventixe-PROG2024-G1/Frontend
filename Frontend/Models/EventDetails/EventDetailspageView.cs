using Frontend.Models.Booking;
using Frontend.Models.Event.Responses;

namespace Frontend.Models.EventDetails
{
    public class EventDetailsPageView
    {
        public EventResponseModel? Event { get; set; }
        public AddBookingFormView BookingForm { get; set; } = new AddBookingFormView();
    }
}
