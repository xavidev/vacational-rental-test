using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Controllers.Models;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
    [Collection("Integration")]
    public class TestBase
    {
        protected readonly HttpClient _client;

        public TestBase(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }
        protected async Task<int> BookRental(BookingBindingModel bookingRequest)
        {
            var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", bookingRequest);

            Assert.True(postBookingResponse.IsSuccessStatusCode);
            var postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();

            return postBookingResult.Id;
        }
        
        protected async Task<int> CreateRental(int rentalUnits, int preparationDays = 0)
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = rentalUnits,
                PreparationTime = preparationDays
            };

            HttpResponseMessage postRentalResponse =
                await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest);

            Assert.True(postRentalResponse.IsSuccessStatusCode);
            ResourceIdViewModel postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();

            return postRentalResult.Id;
        }
    }
}