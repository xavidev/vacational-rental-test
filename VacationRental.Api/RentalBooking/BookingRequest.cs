using System;

namespace VacationRental.Api.RentalBooking
{
    public class BookingRequest
    {
        private readonly DateTime @from;
        private readonly DateTime to;
        private bool isFullfiled;

        public DateTime From => this.@from;
        public DateTime To => this.to;

        public BookingRequest(DateTime @from, int nights)
        {
            if (nights <= 0) throw new ArgumentException("Nigts must be positive");
            
            this.@from = @from;
            this.to = from.AddDays(nights);
            this.isFullfiled = false;
        }

        public void FulFill()
        {
            this.isFullfiled = true;
        }

        public bool IsFulFilled()
        {
            return this.isFullfiled;
        }
    }
}