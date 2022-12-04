using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Controllers.Models;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            BookingBindingModel bookingRequest = CreateBookingRequest(
                await CreateRental(4),
                3,
                new DateTime(2001, 01, 01));
            
            await AssertBookingSuccess(bookingRequest);
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var rentalId = await CreateRental(1);
            
            BookingBindingModel bookingRequest = CreateBookingRequest(
                rentalId,
                3,
                new DateTime(2002, 01, 01));
            
            await AssertBookingSuccess(bookingRequest);
            
            bookingRequest = CreateBookingRequest(
                rentalId,
                1,
                new DateTime(2002, 01, 02));
            
            await AssertBookingFail(bookingRequest);
        }

        private async Task AssertBookingFail(BookingBindingModel bookingRequest)
        {
            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var postBooking2Response =
                       await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest))
                {
                }
            });
        }

        private async Task AssertBookingSuccess(BookingBindingModel bookingRequest)
        {
            var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{await BookRental(bookingRequest)}");

            Assert.True(getBookingResponse.IsSuccessStatusCode);

            var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();

            Assert.Equal(bookingRequest.RentalId, getBookingResult.RentalId);
            Assert.Equal(bookingRequest.Nights, getBookingResult.Nights);
            Assert.Equal(bookingRequest.Start, getBookingResult.Start);
        }

        private async Task<int> BookRental(BookingBindingModel bookingRequest)
        {
            var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest);

            Assert.True(postBookingResponse.IsSuccessStatusCode);
            var postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();

            return postBookingResult.Id;
        }

        private static BookingBindingModel CreateBookingRequest(int rentalId, int nights, DateTime @from)
        {
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = nights,
                Start = @from
            };
            return postBookingRequest;
        }

        private async Task<int> CreateRental(int rentalUnits)
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = rentalUnits
            };

            HttpResponseMessage postRentalResponse =
                await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest);

            Assert.True(postRentalResponse.IsSuccessStatusCode);
            ResourceIdViewModel postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();

            return postRentalResult.Id;
        }

        
    }
}