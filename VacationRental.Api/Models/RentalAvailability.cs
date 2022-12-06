using System;
using System.Collections.Generic;
using VacationRental.Api.RentalBooking;

namespace VacationRental.Api.Models
{
    public class RentalAvailability
    {
        private readonly List<Booking> bookings;

        public RentalAvailability()
        {
            this.bookings = new List<Booking>();
        }
        
        public bool TryBook(BookingRequest bookingRequest)
        {
            var possibleBooking = new Booking(bookingRequest.From, bookingRequest.To);
            foreach (var booking in bookings)
            {
                if (booking.Overlap(possibleBooking)) return false;
            }
            
            bookings.Add(possibleBooking);
            
            return true;
        }
    }
}