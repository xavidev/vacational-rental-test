using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class BookingCatalog
    {
        private static readonly IDictionary<int, Booking> bookings = new Dictionary<int, Booking>();
        public int Add(Booking booking)
        {
            var key = bookings.Count + 1;
            bookings.Add(key, booking);

            return key;
        }

        public Booking Get(int bookingId)
        {
            if (!bookings.ContainsKey(bookingId)) return null;

            return bookings[bookingId];
        }
    }
}