using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Extensions;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class RentalTests
    {
        [Fact]
        public void Get_Bookings_For_Dates()
        {
            var catalog = new RentalCatalog();
            var rentalId = catalog.CreateRental(3);

            var rental = catalog.Get(rentalId);
            
            rental.Assign(new Booking(rentalId, 10.December(2022), 2));

            IEnumerable<Booking> bookings = rental.GetBookings(10.December(2022), 13.December(2022));

            bookings.First().Should().NotBeNull();
        }
    }
}