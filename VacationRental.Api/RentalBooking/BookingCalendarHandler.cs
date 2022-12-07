using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.RentalBooking
{
    public class BookingCalendarHandler
    {
        private readonly RentalCatalog rentalCatalog;
        private readonly BookingCatalog bookingCatalog;

        public BookingCalendarHandler(RentalCatalog rentalCatalog, BookingCatalog bookingCatalog)
        {
            this.rentalCatalog = rentalCatalog;
            this.bookingCatalog = bookingCatalog;
        }

        public Calendar GetBookingCalendarFor(int rentalId, DateTime from, int nights)
        {
            if (nights < 0) throw new ApplicationException("Nights must be positive");

            Rental rental = this.rentalCatalog.Get(rentalId);
            
            return this.bookingCatalog.GetBookingCalendarFor(rental.Id, from, nights);
        }
    }
}