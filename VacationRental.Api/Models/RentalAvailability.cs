using System;

namespace VacationRental.Api.Models
{
    public class RentalAvailability
    {
        private readonly int units;

        public RentalAvailability(int units)
        {
            this.units = units;
        }
        public bool IsAvailableFor(DateTime @from, int nights)
        {
            throw new NotImplementedException();
        }

        public void SetAvailability(DateTime requestFrom, int requestNights)
        {
            throw new NotImplementedException();
        }
    }
}