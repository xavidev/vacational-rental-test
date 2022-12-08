using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using VacationRental.Api.Controllers.Models;
using VacationRental.Api.Tests.Mothers;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
    public class GetCalendarTests : TestBase
    {
        public GetCalendarTests(IntegrationFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var rentalId = await this.CreateRental(2);

            var postBooking1Result = await this.BookRental(BookingRequest.For(rentalId).From(2.January(2000)).Nights(2));
            var postBooking2Result = await this.BookRental(BookingRequest.For(rentalId).From(3.January(2000)).Nights(2));
            
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={rentalId}&start=2000-01-01&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(rentalId, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                
                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, 
                    x => x.Id == postBooking1Result && x.Unit > 0);
                
                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, 
                    x => x.Id == postBooking1Result && x.Unit > 0);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, 
                    x => x.Id == postBooking2Result && x.Unit > 0);
                
                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, 
                    x => x.Id == postBooking2Result && x.Unit > 0);
                
                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar_With_Preparation_Days()
        {
            var rentalId = await this.CreateRental(2, 2);
            
            var postBooking1Result = await this.BookRental(BookingRequest.For(rentalId).From(2.January(2000)).Nights(2));
            var postBooking2Result = await this.BookRental(BookingRequest.For(rentalId).From(3.January(2000)).Nights(2));
            
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={rentalId}&start=2000-01-01&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(rentalId, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                
                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, 
                    x => x.Id == postBooking1Result && x.Unit > 0);
                
                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, 
                    x => x.Id == postBooking1Result && x.Unit > 0);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, 
                    x => x.Id == postBooking2Result && x.Unit > 0);
                
                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Equal(2, getCalendarResult.Dates[3].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, 
                    x => x.Id == postBooking1Result && x.Unit > 0);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, 
                    x => x.Id == postBooking2Result && x.Unit > 0);
                
                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Single(getCalendarResult.Dates[4].Bookings);
                Assert.Contains(getCalendarResult.Dates[4].PreparationTimes, 
                    x => x.Unit > 0);
            }
        }
    }
}
