using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.RentalBooking
{
    public class BookingHandler
    {
        private readonly RentalCatalog rentalCatalog;
        private readonly BookingCatalog bookingCatalog;

        public BookingHandler(RentalCatalog rentalCatalog, BookingCatalog bookingCatalog)
        {
            this.rentalCatalog = rentalCatalog;
            this.bookingCatalog = bookingCatalog;
        }

        public BookingResult Book(int rentalId, DateTime from, int nights)
        {
            Rental rental = this.rentalCatalog.Get(rentalId);
            Booking booking = rental.CreateBooking(from, nights);
            rental.Assign(booking);

            if (booking.IsReserved())
            {
                var key = bookingCatalog.Add(booking);
                return BookingResult.Ok(key);
            }
            
            return BookingResult.Fail();
        }

        public BookingInfo GetBooking(int bookingId)
        {
            Booking booking = this.bookingCatalog.Get(bookingId);
            
            if(booking == null) throw new ApplicationException("Booking not found");

            return booking.GetInfo();
        }
    }

    public class BookingResult
    {
        private readonly int id;
        private readonly bool success;
        public int Id => this.id;
        public bool Success => this.success;

        private BookingResult()
        {
            success = false;
        }
        private  BookingResult(int id)
        {
            this.id = id;
            this.success = true;
        }
        public static BookingResult Ok(int id)
        {
            return new BookingResult(id);
        }

        public static BookingResult Fail()
        {
            return new BookingResult();
        }
    }
}