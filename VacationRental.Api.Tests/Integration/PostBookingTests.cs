﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Controllers.Models;
using VacationRental.Api.Tests.Mothers;
using Xunit;
using FluentAssertions.Extensions;

namespace VacationRental.Api.Tests.Integration
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
            await AssertBookingSuccess(BookingRequest.For(await CreateRental(4)).From(1.December(2022)).Nights(3));
        }
        
        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            var rentalId = await CreateRental(1);
            
            await AssertBookingSuccess(BookingRequest.For(rentalId).From(1.December(2022)).Nights(3));
            
            await AssertBookingFail(BookingRequest.For(rentalId).From(2.December(2022)).Nights(1));
        }

        [Fact]
        //See booking_scenario.excalidraw to see a graphic representation
        public async Task Test_Overlapping_Booking()
        {
            var rentalId = await CreateRental(4);
            
            await AssertBookingSuccess(BookingRequest.For(rentalId).From(5.December(2022)).Nights(1));
            
            await AssertBookingSuccess(BookingRequest.For(rentalId).From(6.December(2022)).Nights(3));
            
            await AssertBookingSuccess(BookingRequest.For(rentalId).From(5.December(2022)).Nights(2));
            
            await AssertBookingSuccess(BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));

            await AssertBookingSuccess(BookingRequest.For(rentalId).From(8.December(2022)).Nights(1));
            
            await AssertBookingSuccess(BookingRequest.For(rentalId).From(5.December(2022)).Nights(1));

            await AssertBookingFail(BookingRequest.For(rentalId).From(5.December(2022)).Nights(4));
        }

        private async Task AssertBookingFail(BookingBindingModel bookingRequest)
        {
            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest);
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