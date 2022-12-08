using System;

namespace VacationRental.Api.Models
{
    public class Booking
    {
        private readonly int rentalId;
        private readonly DateTime @from;
        private  DateTime to;
        private readonly int nights;
        private bool reserved;
        private int preparationDays;
        private int unit;

        public int RentalId => this.rentalId;
        public int Unit => this.unit;
        
        public Booking(int rentalId, DateTime from, int nights)
        {
            this.rentalId = rentalId;
            this.@from = @from;
            this.nights = nights;
            this.to = from.AddDays(nights);
            this.reserved = false;
        }

        public Booking(int rentalId, DateTime from, int nights, int preparationDays) 
            : this(rentalId, from, nights)
        {
            this.preparationDays = preparationDays;
            this.to = from.AddDays(this.nights + this.preparationDays);
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

        public void SetReserved(int unit)
        {
            this.reserved = true;
            this.unit = unit;
        }

        public bool IsReserved()
        {
            return this.reserved;
        }

        public bool HasReservationFor(DateTime date)
        {
            if (!this.IsReserved()) return false;
            
            return this.@from <= date && this.to > date;
        }

        public bool IsInPreparation(DateTime date)
        {
            if (!this.IsReserved()) return false;
            if (this.preparationDays == 0) return false;

            return date > this.@from.AddDays(this.nights) && date <= this.to;
        }

        public BookingInfo GetInfo()
        {
            return new BookingInfo(this.rentalId, this.@from, this.nights);
        }

        public void Release()
        {
            this.reserved = false;
        }

        public void SetPreparationDays(int preparationDays)
        {
            this.preparationDays = preparationDays;
            this.to = this.@from.AddDays(nights + this.preparationDays);
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