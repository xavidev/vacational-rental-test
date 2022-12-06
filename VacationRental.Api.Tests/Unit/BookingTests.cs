using FluentAssertions;
using FluentAssertions.Extensions;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class BookingTests
    {
        [Fact]
        public void Test_booking_overlap()
        {
            var b1 = new Booking(5.December(2022), 8.December(2022));
            var b2 = new Booking(8.December(2022), 10.December(2022));
            AssertNotOverlap(b1, b2);

            b1 = new Booking(1.December(2022), 3.December(2022));
            b2 = new Booking(1.December(2022), 5.December(2022));
            AssertOverlap(b1, b2);

            b1 = new Booking(5.December(2022), 8.December(2022));
            b2 = new Booking(6.December(2022), 9.December(2022));
            AssertOverlap(b1, b2);
            
            b1 = new Booking(5.December(2022), 8.December(2022));
            b2 = new Booking(6.December(2022), 7.December(2022));
            AssertOverlap(b1, b2);
            
            b1 = new Booking(5.December(2022), 6.December(2022));
            b2 = new Booking(6.December(2022), 7.December(2022));
            AssertNotOverlap(b1, b2);
        }

        private void AssertNotOverlap(Booking b1, Booking b2)
        {
            b1.Overlap(b2).Should().BeFalse();
        }
        
        private void AssertOverlap(Booking b1, Booking b2)
        {
            b1.Overlap(b2).Should().BeTrue();
        }
    }
}