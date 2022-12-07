using System;
using System.Threading;
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

        //See booking_scenario.excalidraw to see a graphic representation
        [Fact]
        public void Test_Rental_Booking_Use_Case()
        {
            var catalog = new RentalCatalog();
            var bookingCatalog = new BookingCatalog();
            var rentalId = catalog.CreateRental(4, 0);

            sut = new BookingHandler(catalog, bookingCatalog);

            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(1));
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(6.December(2022)).Nights(3));
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(2));
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(8.December(2022)).Nights(1));
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(1));
            
            AssertBookingFail(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(4));
            
        }

        [Fact]
        public void Test_Rental_With_Preparation_Booking_Use_Case()
        {
            var catalog = new RentalCatalog();
            var bookingCatalog = new BookingCatalog();
            var rentalId = catalog.CreateRental(4, 2);
            sut = new BookingHandler(catalog, bookingCatalog);

            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(1));
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(6.December(2022)).Nights(3));
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(2));
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
            
            AssertBookingSuccess(Mothers.BookingRequest.For(rentalId).From(8.December(2022)).Nights(1));
            
            AssertBookingFail(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(1));
            
            AssertBookingFail(Mothers.BookingRequest.For(rentalId).From(5.December(2022)).Nights(4));
        }

        private void AssertBookingFail(Mothers.BookingRequest request)
        {
            this.sut.Book(request.RentalId, request.RentalFrom, request.RentalNights).Success.Should().BeFalse();
        }

        private void AssertBookingSuccess(Mothers.BookingRequest request)
        {
            this.sut.Book(request.RentalId, request.RentalFrom, request.RentalNights).Success.Should().BeTrue();
        }
    }
}