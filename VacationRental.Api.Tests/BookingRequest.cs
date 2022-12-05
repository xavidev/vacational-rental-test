using NodaTime;
using VacationRental.Api.Controllers.Models;

namespace VacationRental.Api.Tests
{
    public class BookingRequest
    {
        private readonly int rentalId;
        private LocalDate @from;
        private int nights;

        private BookingRequest(int rentalId)
        {
            this.rentalId = rentalId;
        }

        public static BookingRequest For(int rentalId)
        {
            return new BookingRequest(rentalId);
        }

        public BookingRequest From(LocalDate from)
        {
            this.from = from;
            
            return this;
        }

        public BookingRequest Nights(int nights)
        {
            this.nights = nights;

            return this;
        }

        public static implicit operator BookingBindingModel(BookingRequest request) => new BookingBindingModel
        {
            RentalId = request.rentalId,
            Nights = request.nights,
            Start = request.@from.ToDateTimeUnspecified()
        };
    }
}