using System;
using System.Collections.Generic;
using System.Linq;


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

        public BookingCalendar GetBookingCalendarFor(int rentalId, DateTime from, int nights)
        {
            var result  = new BookingCalendar();
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDate
                {
                    Date = from.Date.AddDays(i),
                    Bookings = new List<DateBooking>()
                };

                foreach (var booking in bookings.Where(x => x.Value.RentalId == rentalId))
                {
                    if (booking.Value.HasReservationFor(date.Date))
                    {
                        date.Bookings.Add(new DateBooking() { Id = booking.Key, Unit = booking.Value.Unit });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }

    public class BookingCalendar
    {
        public List<CalendarDate> Dates { get; set; } = new List<CalendarDate>();
    }

    public class DateBooking
    {
        public int Id { get; set; }
        public int Unit { get; set; }
    }

    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public List<DateBooking> Bookings { get; set; } = new List<DateBooking>();
    }
}