using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.Booking
{
    public class BookingHandler
    {
        private readonly RentalCatalog catalog;

        public BookingHandler(RentalCatalog catalog)
        {
            this.catalog = catalog;
        }

        public void Book(int rentalId, DateTime from, int nights)
        {
            var request = new BookingRequest(from, nights);

            Rental rental = this.catalog.Get(rentalId);
            rental.Assign(request);
        }
    }
}