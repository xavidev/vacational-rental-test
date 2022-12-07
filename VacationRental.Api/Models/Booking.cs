using System;

namespace VacationRental.Api.Models
{
    public class Booking
    {
        private readonly int rentalId;
        private readonly DateTime @from;
        private readonly DateTime to;
        private readonly int nights;
        private bool reserved;
        
        public Booking(int rentalId, DateTime from, int nights)
        {
            this.rentalId = rentalId;
            this.@from = @from;
            this.nights = nights;
            this.to = from.AddDays(nights);
            this.reserved = false;
        }

        public bool Overlap(Booking other)
        {
            return SameFrom(other) || IsIn(other);
        }

        private bool IsIn(Booking other)
        {
            return this.to > other.@from || this.to > other.to;
        }

        private bool SameFrom(Booking other)
        {
            return this.@from == other.@from;
        }

        public void SetReserved()
        {
            this.reserved = true;
        }

        public bool IsReserved()
        {
            return this.reserved;
        }

        public bool HasReservationFor(int rentalId, DateTime date)
        {
            if (!this.IsReserved()) return false;
            if (this.rentalId != rentalId) return false;

            return this.@from <= date && this.to > date;
        }

        public BookingInfo GetInfo()
        {
            return new BookingInfo(this.rentalId, this.@from, this.nights);
        }
    }

    public class BookingInfo
    {
        public int RentalId { get; }
        public DateTime From { get; }
        public int Nights { get; }

        public BookingInfo(int rentalId, DateTime @from, int nights)
        {
            RentalId = rentalId;
            From = @from;
            Nights = nights;
        }
    }
}