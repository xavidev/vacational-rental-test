using System;

namespace VacationRental.Api.Booking
{
    public class BookingRequest
    {
        private readonly DateTime @from;
        private readonly int nights;

        public DateTime From => this.@from;
        public int Nights => this.nights;

        public BookingRequest(DateTime @from, int nights)
        {
            if (nights <= 0) throw new ArgumentException("Nigts must be positive");
            
            this.@from = @from;
            this.nights = nights;
        }
    }
}