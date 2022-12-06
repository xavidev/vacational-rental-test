using System;

namespace VacationRental.Api.Models
{
    public class Booking
    {
        private readonly DateTime @from;
        private readonly DateTime to;

        public Booking(DateTime from, DateTime to)
        {
            this.@from = @from;
            this.to = to;
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
    }
}