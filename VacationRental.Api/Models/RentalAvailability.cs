using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class RentalAvailability
    {
        private readonly List<Booking> bookings;

        public RentalAvailability()
        {
            this.bookings = new List<Booking>();
        }
        
        public bool TryBook(Booking bookingRequest)
        {
            foreach (var booking in bookings)
            {
                if (booking.Overlap(bookingRequest)) return false;
            }
            
            bookings.Add(bookingRequest);
            
            return true;
        }
    }
}