using System;
using FluentAssertions;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class RentalAvailabilityTests
    {
        [Fact]
        public void Check_Availability()
        {
            var sut = new RentalAvailability(1);
            sut.TryBook(new DateTime(2022, 1, 1), 3);
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeFalse();
            
            sut = new RentalAvailability(2);
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeTrue();
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeTrue();
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeFalse();
            
            sut = new RentalAvailability(3);
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeTrue();
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeTrue();
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeTrue();
            sut.TryBook(new DateTime(2022, 1, 1), 3).Should().BeFalse();
        }
    }
}