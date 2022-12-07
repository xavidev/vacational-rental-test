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
            var b1 = new Booking(1,5.December(2022), 3);
            var b2 = new Booking(2,8.December(2022), 2);
            AssertNotOverlap(b1, b2);

            b1 = new Booking(3,1.December(2022), 2);
            b2 = new Booking(4,1.December(2022), 4);
            AssertOverlap(b1, b2);

            b1 = new Booking(5,5.December(2022), 3);
            b2 = new Booking(6,6.December(2022), 3);
            AssertOverlap(b1, b2);
            
            b1 = new Booking(7,5.December(2022), 3);
            b2 = new Booking(8,6.December(2022), 1);
            AssertOverlap(b1, b2);
            
            b1 = new Booking(9,5.December(2022), 1);
            b2 = new Booking(10,6.December(2022), 1);
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