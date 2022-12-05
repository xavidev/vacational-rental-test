using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class RentalAvailabilityTests
    {
        [Fact]
        public void Check_Availability()
        {
            var sut = new RentalAvailability(-3);
        }
    }
}