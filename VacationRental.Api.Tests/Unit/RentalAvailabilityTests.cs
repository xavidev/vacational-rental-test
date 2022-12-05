using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class RentalAvailabilityTests
    {
        private RentalAvailability sut;
        
        [Fact]
        public void Test_Try_Book_With_Complex_Scenario()
        {
            sut = new RentalAvailability(4);
            AssertSuccessBooking(5.December(2022), 1);
            AssertSuccessBooking(6.December(2022), 3);
            AssertSuccessBooking(5.December(2022), 3);
            AssertSuccessBooking(5.December(2022), 3);
            AssertSuccessBooking(8.December(2022), 1);
            AssertSuccessBooking(5.December(2022), 3);
            AssertFailBooking(6.December(2022), 1);
        }

        private void AssertFailBooking(DateTime from, int nights)
        {
            sut.TryBook(from, nights).Should().BeFalse();
        }

        private void AssertSuccessBooking(DateTime from, int nights)
        {
            sut.TryBook(from, nights).Should().BeTrue();
        }
    }
}