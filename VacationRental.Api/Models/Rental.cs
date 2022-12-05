using VacationRental.Api.Booking;

namespace VacationRental.Api.Models
{
    public class Rental
    {
        private readonly int id;
        private readonly RentalAvailability availability;
        public int Units { get; }

        public static Rental Create(int id, int units)
        {
            return new Rental(id, new RentalAvailability(units), units);
        }

        private Rental(int id, RentalAvailability availability, int units)
        {
            this.id = id;
            this.availability = availability;
            Units = units;
        }

        public void Assign(BookingRequest request)
        {
            this.availability.TryBook(request.From, request.Nights);
        }
    }
}