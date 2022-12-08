using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Extensions;
using VacationRental.Api.Controllers.Models;
using VacationRental.Api.Tests.Mothers;
using Xunit;

namespace VacationRental.Api.Tests.Integration
{
    
    public class PostRentalTests : TestBase
    {
        public PostRentalTests(IntegrationFixture fixture) : base(fixture)
        {
            
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTime = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
            }
        }

        [Fact]
        public async Task Try_Update_Rental_With_More_Units()
        {
            var rentalId = await CreateRental(2, 1);
            await BookRental(BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
            await BookRental(BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));

            var response = await _client.PutAsJsonAsync($"/api/v1/rentals/{rentalId}", new RentalBindingModel()
            {
                Units = 3,
                PreparationTime = 1
            });
            
            Assert.True(response.IsSuccessStatusCode);
        }
        
        [Fact]
        public async Task Try_Update_Rental_With_Less_Units()
        {
            var rentalId = await CreateRental(2, 1);
            await BookRental(BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
            await BookRental(BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));

            var response = await _client.PutAsJsonAsync($"/api/v1/rentals/{rentalId}", new RentalBindingModel()
            {
                Units = 1,
                PreparationTime = 1
            });
            
            Assert.False(response.IsSuccessStatusCode);
        }
        
        [Fact]
        public async Task Try_Update_Rental_With_More_Preparation_Time()
        {
            var rentalId = await CreateRental(2, 1);
            await BookRental(BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
            await BookRental(BookingRequest.For(rentalId).From(5.December(2022)).Nights(3));
            await BookRental(BookingRequest.For(rentalId).From(9.December(2022)).Nights(3));

            var response = await _client.PutAsJsonAsync($"/api/v1/rentals/{rentalId}", new RentalBindingModel()
            {
                Units = 2,
                PreparationTime = 2
            });
            
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
