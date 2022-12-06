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
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(1));
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(6.December(2022)).Nights(3));
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(2));
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(8.December(2022)).Nights(1));
            AssertBookingFail(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
        }

        private void AssertBookingFail(Mothers.BookingRequest request)
        {
            this.sut.Book(request.RentalId, request.RentalFrom, request.RentalNights).Should().BeFalse();
        }

        private void AssertBookingSuccess(Mothers.BookingRequest request)
        {
            this.sut.Book(request.RentalId, request.RentalFrom, request.RentalNights).Should().BeTrue();
        }
    }
}