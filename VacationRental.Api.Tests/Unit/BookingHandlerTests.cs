using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using VacationRental.Api.Models;
using VacationRental.Api.RentalBooking;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class BookingHandlerTests
    {
        private BookingHandler sut;
        
        [Fact]
        public void Test_Rental_Booking_Use_Case()
        {
            var catalog = new RentalCatalog();
            var rentalId = catalog.CreateRental(4);

            sut = new BookingHandler(catalog);
            
            this.AssertSuccessBooking(rentalId, 5.December(2022), 1);
            this.AssertSuccessBooking(rentalId, 6.December(2022), 3);
        }

        private void AssertSuccessBooking(int rentalId, DateTime from, int nights)
        {
            sut.Book(rentalId, 5.December(2022), 1).Should().BeTrue();
        }
    }
}