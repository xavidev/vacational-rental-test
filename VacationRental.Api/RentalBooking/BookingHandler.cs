using System;
using VacationRental.Api.Models;

namespace VacationRental.Api.RentalBooking
{
    public class BookingHandler
    {
        private readonly RentalCatalog catalog;

        public BookingHandler(RentalCatalog catalog)
        {
            this.catalog = catalog;
        }

        public bool Book(int rentalId, DateTime from, int nights)
        {
            var request = new BookingRequest(from, nights);

            Rental rental = this.catalog.Get(rentalId);
            rental.Assign(request);

            return request.IsFulFilled();
        }
    }
}