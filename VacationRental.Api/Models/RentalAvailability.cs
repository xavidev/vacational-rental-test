using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models
{
    public class RentalAvailability
    {
        private readonly int units;
        private readonly Dictionary<int, int> reservations;

        public RentalAvailability(int units)
        {
            if (units < 0) throw new ArgumentException("Units must be positive");
            
            this.units = units;
            this.reservations = new Dictionary<int, int>();
        }
        private bool IsAvailableFor(DateTime @from, int nights)
        {
            if(this.reservations.TryGetValue(GetKey(@from, nights), out int unitsLeft))
            {
                return unitsLeft > 0;
            }

            return true;
        }
        
        public bool TryBook(DateTime from, int nights)
        {
            if (!IsAvailableFor(from, nights)) return false;
            
            var key = GetKey(@from, nights);
            if (this.reservations.ContainsKey(key))
            {
                this.reservations[key] -= 1;
            }
            else
            {
                this.reservations.Add(key, this.units - 1);
            }

            return true;
        }
        
        private static int GetKey(DateTime @from, int nights)
        {
            var to = @from.AddDays(nights);
            var key = @from.GetHashCode() + to.GetHashCode();
            
            return key;
        }
    }
}